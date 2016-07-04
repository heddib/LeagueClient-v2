import { Swish }     from './../../ui/swish';
import Module        from './../../ui/module';
import * as Audio    from './../../assets/audio';
import * as Assets   from './../../assets/assets';
import * as Summoner from './../../summoner/summoner';
import * as PlayLoop from './../playloop';
import * as Service  from './service';
import * as Electron from './../../electron';

import ChatRoom      from './../../chat/room/chatroom';

const html = Module.import('playloop/lobby');

const CHAMPSELECT_PHASES = ['PLANNING', 'BANNING', 'PICKING', 'FINALIZING'];
export default class Lobby extends Module {
    private queueStart: number;
    private isRole1 = false;
    private role1: string;
    private role2: string;

    private inviting: boolean;
    private invite: IInviteProvider;

    private doDispose = true;
    private room: ChatRoom;

    public start = this.create<{}>();

    public constructor(inQueue: boolean, provider: IInviteProvider) {
        super(html);
        this.invite = provider;

        this.$('.role-selectable').on('mouseup', (e: MouseEvent) => this.onRoleMapClick(e));
        this.$('.role-selectable').on('mouseenter', (e: MouseEvent) => this.onRoleMapHover(e));
        this.$('.role-selectable').on('mouseleave', (e: MouseEvent) => this.onRoleMapHover(e));
        this.$('#enter-queue').on('click', (e: MouseEvent) => this.onStartQueueClick(e));
        this.$('#cancel-queue').on('click', (e: MouseEvent) => this.onCancelQueueClick(e));
        this.$('#leave-lobby').on('click', (e: MouseEvent) => this.onLeaveLobbyClick(e));
        this.$('#afk-accept').on('click', (e: MouseEvent) => this.onAfkAcceptClick(e));
        this.$('#afk-decline').on('click', (e: MouseEvent) => this.onAfkDeclineClick(e));

        this.subscribe(Service.lobbyState, this.onLobbyState);
        this.subscribe(Service.queueState, this.onMatchmakingState);
        this.subscribe(Service.matchmakingStop, () => this.onAdvance('LOBBY'));
        this.subscribe(Service.matchmakingStart, () => this.onAdvance('MATCHMAKING'));
        this.subscribe(Service.champselectStart, () => this.onAdvance('CHAMPSELECT'));

        this.refs.inviteButton.on('mouseup', e => this.onInviteClick());

        this.$('#queue-info').css('display', 'none');
        this.$('#afk-check').css('display', 'none');

        if (inQueue) {
            this.onAdvance('MATCHMAKING');
        }
    }

    public dispose() {
        super.dispose();
        this.queueStart = 0;
        if (this.doDispose)
            PlayLoop.abandon();
        if (this.inviting)
            this.invite.stop();
    }

    private onAdvance(state: string) {
        if (state == 'CHAMPSELECT') {
            this.doDispose = false;
            this.dispatch(this.start, {});
        } else {
            this.$('#lobby-controls').css('display', state == 'LOBBY' ? null : 'none');
            this.$('#queue-info').css('display', state == 'MATCHMAKING' ? null : 'none');
        }
    }

    private onLobbyState(state: Domain.Game.LobbyState) {
        if (!state.members) return;

        if (!this.room && state.chatroom != guid.empty) {
            this.room = new ChatRoom(state.chatroom);
            this.refs.chatContainer.add(this.room.node);
        }

        this.queueStart = 0;
        this.$('#enter-queue').css('display', state.isCaptain ? null : 'none');
        this.$('#invite-button').css('display', (state.isCaptain || state.canInvite) ? null : 'none');
        this.$('#enter-queue').disabled = !state.canMatch;

        this.$('#member-list').empty();

        var start = new Date();
        for (let i = 0; i < state.members.length; i++) {
            Summoner.get(state.members[i].name).then(s => {
                this.renderSlot(state.members[i], state.members.length, state.me, s.icon)
            });
        }
    }

    private onMatchmakingState(state: Domain.Game.MatchmakingState) {
        if (state.afkCheck) {
            if (this.$('#afk-check').css('display')) {
                Audio.effect('lobby', 'pop');
                this.$('#queue-info').css('display', 'none');
                this.$('#afk-check').css('display', null);
                Electron.focus();
            }

            this.queueStart = 0;
            var bar = this.$('#afk-timeout > div');
            bar.css('transition', 'none');
            bar.css('width', (state.afkCheck.remaining / state.afkCheck.duration * 100) + '%');
            bar[0].offsetHeight;
            bar.css('transition', 'width ' + state.afkCheck.remaining + 'ms linear');
            bar.css('width', 0);
            this.$('#afk-accept').disabled = this.$('#afk-decline').disabled = state.afkCheck.accepted != null;
        } else {
            if (this.$('#queue-info').css('display')) {
                this.$('#queue-info').css('display', null);
                this.$('#afk-check').css('display', 'none');
            }

            this.queueStart = new Date().getTime() - state.actual;
            this.tick();

            var seconds: any = Math.floor(state.estimate / 1000) % 60;
            if (seconds < 10) seconds = '0' + seconds;
            var format = Math.floor(state.estimate / 1000 / 60) + ':' + seconds;

            this.$('#queue-estimate').text = format;
        }
    }

    private chatRoom: ChatRoom;
    private onChatRoom(room) {
        if (this.chatRoom) this.chatRoom.dispose();
        this.chatRoom = new ChatRoom(room);
        this.$('#chat-area').empty();
        this.$('#chat-area').add(this.chatRoom.node);
    }

    private renderSlot(member: Domain.Game.LobbyMember, lobbySize: number, me: Domain.Game.LobbyMember, icon: number) {
        var data = {
            id: member.id,
            // class: mySlot == slot.slotId ? 'me' : 'friend',
            name: member.name,
            iconURL: Assets.summoner.icon(icon),
            role1: member.role1,
            role2: member.role2,
        };

        var old = this.$("#member-" + data.id);
        var node = this.template('member', data);
        if (old.length > 0)
            old.replace(node);
        else
            this.$('#member-list').append(node);

        if (member.id == me.id) {
            node.addClass('member-me');
            node.$('.member-roles > div').on('mouseup', (e: MouseEvent) => this.onRoleClck(e));
            this.role1 = member.role1;
            this.role2 = member.role2;
        }

        if (!data.role1) {
            node.addClass('no-roles');
        } else if (data.role1 == "FILL" || lobbySize == 5) {
            node.$('.role2').css('display', 'none');
        }
    }

    private onInviteClick() {
        this.inviting = !this.inviting;

        this.refs.inviteButton.setClass(this.inviting, 'inviting');

        if (this.inviting)
            this.invite.start();
        else
            this.invite.stop();
    }

    private onRoleMapHover(e: MouseEvent) {
        var src = <HTMLElement>e.target;
        var role = src.attributes['data-role'].value;

        this.$('.role-selectable').removeClass('active');
        if (e.type == 'mouseenter' && role == 'FILL') this.$('.role-selectable').addClass('active');
    }

    private onRoleMapClick(e: MouseEvent) {
        var src = <HTMLElement>e.target;
        var role = src.attributes['data-role'].value;
        if (this.isRole1) {
            if (role == this.role2) this.role2 = this.role1;
            this.role1 = role;
        } else {
            if (role == this.role1) this.role1 = this.role2;
            this.role2 = role;
        }
        Service.selectRoles(this.role1, this.role2);
        this.$('#role-selector').css('top', null);
        this.$('#role-selector').css('left', null);
    }

    private onRoleClck(e: MouseEvent) {
        var but = new Swish(<Node>e.target);
        this.isRole1 = but.hasClass('role1');
        var selector = this.$('#role-selector');
        selector.css('top', but.bounds.top + but.bounds.height / 2 - selector.bounds.height / 2 + 'px');
        selector.css('left', but.bounds.left + but.bounds.width / 2 - selector.bounds.width / 2 + 'px');
    }

    private onStartQueueClick(e: MouseEvent) {
        Service.startMatchmaking();
    }

    private onCancelQueueClick(e: MouseEvent) {
        Service.stopMatchmaking();
    }

    private tick() {
        if (this.queueStart > 0) {
            var actual = new Date().getTime() - this.queueStart;

            var seconds: any = Math.floor(actual / 1000) % 60;
            if (seconds < 10) seconds = '0' + seconds;
            var format = Math.floor(actual / 1000 / 60) + ':' + seconds;

            this.$('#queue-actual').text = format;
            setTimeout(() => this.tick(), 100);
        }
    }

    private onAfkAcceptClick(e: MouseEvent) {
        Service.afkCheck(true);
    }

    private onAfkDeclineClick(e: MouseEvent) {
        Service.afkCheck(false);
    }

    private onLeaveLobbyClick(e: MouseEvent) {
        PlayLoop.quit();
        this.doDispose = false;
        this.dispatch(this.close, {});
    }
}