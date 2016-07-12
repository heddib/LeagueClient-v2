import { Swish }     from './../../ui/swish';
import Module        from './../../ui/module';
import * as Electron from './../../electron';
import * as Audio    from './../../assets/audio';
import * as Assets   from './../../assets/assets';
import * as Summoner from './../../summoner/summoner';
import * as PlayLoop from './../playloop';
import * as Service  from './service';

import * as Invite   from './../../invite/panel';
import ChatRoom      from './../../chat/room/chatroom';

const html = Module.import('playloop/lobby');

const CHAMPSELECT_PHASES = ['PLANNING', 'BANNING', 'PICKING', 'FINALIZING'];

interface Refs {
    invitesContainer: Swish;
    chatContainer: Swish;
    lobbyControls: Swish;
    queueName: Swish;
    mapName: Swish;
}

export default class Lobby extends Module<Refs> {
    private queueStart: number;
    private isRole1 = false;
    private role1: string;
    private role2: string;

    private inviting: boolean;
    private invite: Invite.Panel;

    private doDispose = true;
    private room: ChatRoom;

    public start = this.create<any>();

    public constructor(inQueue: boolean, provider: Invite.IProvider) {
        super(html);
        this.invite = new Invite.Panel(provider);
        this.invite.render(this.refs.invitesContainer);

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

        this.$('#queue-info').css('display', 'none');
        this.$('#afk-check').css('display', 'none');

        if (inQueue) {
            this.onAdvance('MATCHMAKING');
        }
    }

    public dispose() {
        super.dispose();

        this.queueStart = 0;
        this.invite.dispose();
        if (this.chatRoom)
            this.chatRoom.dispose();
        if (this.doDispose)
            PlayLoop.abandon();
    }

    private onAdvance(state: string) {
        if (state == 'CHAMPSELECT') {
            this.doDispose = false;
            this.dispatch(this.start, {});
        } else {
            this.refs.lobbyControls.setClass(state != 'LOBBY', 'hidden');
            this.$('#queue-info').css('display', state == 'MATCHMAKING' ? null : 'none');
        }
    }

    private onLobbyState(state: Domain.Game.LobbyState) {
        if (!state.members) return;

        if (!this.room && state.chatroom != guid.empty) {
            this.room = new ChatRoom(state.chatroom);
            this.refs.chatContainer.add(this.room.node);

            PlayLoop.current().then(state => {
                PlayLoop.queues().then(queues => {
                    let queue = queues.first(q => q.id == state.queueId);
                    let name = PlayLoop.queueNames[state.queueId] || PlayLoop.featuredNames[state.queueId];
                    let map = Assets.gamedata.maps.first(m => m.id == queue.map);
                    this.refs.mapName.text = map.name;
                    this.refs.queueName.text = name;
                });
            });
        }

        this.queueStart = 0;
        this.$('#enter-queue').css('display', state.isCaptain ? null : 'none');
        this.$('#invite-button').css('display', (state.isCaptain || state.canInvite) ? null : 'none');
        this.$('#enter-queue').disabled = !state.canMatch;

        this.$('#member-list').empty();
        for (let member of state.members) {
            Summoner.get(member.name).then(s => this.renderSlot(member, state.members.length, state.me, s.icon));
        }

        this.invite.update(state.isCaptain || state.canInvite, state.invitees);
    }

    private onMatchmakingState(state: Domain.Game.MatchmakingState) {
        if (!state) {
            this.$('#queue-info').css('display', 'none');
            this.$('#afk-check').css('display', 'none');
        } else if (state.afkCheck) {
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
            if (!state.afkCheck.accepted) {
                Service.afkCheck(true);
            }
            // this.$('#afk-accept').disabled = this.$('#afk-decline').disabled = state.afkCheck.accepted != null;
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
            role1: member.role1 ? member.role1.toLowerCase() : '',
            role2: member.role2 ? member.role2.toLowerCase() : '',
        };

        var old = this.$("#member-" + data.id);
        var node = this.template('member', data);
        if (old.length > 0)
            old.replace(node);
        else
            this.$('#member-list').append(node);

        if (member.id == me.id) {
            node.addClass('member-me');
            this.role1 = member.role1;
            this.role2 = member.role2;

            node.$('.member-roles > div').on('mouseup', (e: MouseEvent) => this.onRoleClck(e));
        }

        if (!data.role1) {
            node.addClass('no-roles');
        } else if (data.role1 == "fill" || lobbySize == 5) {
            node.$('.role2').css('display', 'none');
        }
    }

    private onRoleMapHover(e: MouseEvent) {
        var src = e.target as HTMLElement;
        var role = src.attributes['data-role'].value;

        this.$('.role-selectable').removeClass('active');
        if (e.type == 'mouseenter' && role == 'FILL') this.$('.role-selectable').addClass('active');
    }

    private onRoleMapClick(e: MouseEvent) {
        var src = e.target as HTMLElement;
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
        this.$('#role-selector').addClass('hidden');
    }

    private onRoleClck(e: MouseEvent) {
        var but = new Swish(e.target as Node);
        this.isRole1 = but.hasClass('role1');
        var selector = this.$('#role-selector');
        selector.css('top', but.bounds.top + but.bounds.height / 2 - selector[0].offsetHeight / 2 + 'px');
        selector.css('left', but.bounds.left + but.bounds.width / 2 - selector[0].offsetWidth / 2 + 'px');
        this.$('#role-selector').removeClass('hidden');
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
        this.dispatch(this.closed, {});
    }
}