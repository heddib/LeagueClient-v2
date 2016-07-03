import { Swish }     from './../../ui/swish';
import Module        from './../../ui/module';
import * as Assets   from './../../assets/assets';
import * as Summoner from './../../summoner/summoner';

import * as PlayLoop from './../playloop';
import * as Service  from './service';

import ChatRoom      from './../../chat/room/chatroom';

const html = Module.import('playloop/custom');

export default class CustomLobby extends Module {
    private doDispose = true;
    private room: ChatRoom;

    private invite: IInviteProvider;
    private inviting: boolean;

    public start = this.create<{}>();

    public constructor(provider: IInviteProvider) {
        super(html);

        this.invite = provider;

        this.$('#start-game').on('click', (e: MouseEvent) => this.onStartGameClick(e));
        this.$('#quit-game').on('click', (e: MouseEvent) => this.onQuitGameClick(e));
        this.$('#switch-teams').on('click', (e: MouseEvent) => this.onSwitchTeamsClick(e));

        this.refs.inviteButton.on('mouseup', e => this.onInviteClick());

        this.subscribe(Service.state, this.onState);
        this.subscribe(Service.champselect, this.onStart);
    }

    public dispose() {
        super.dispose();
        if (this.doDispose)
            PlayLoop.abandon();
        if (this.inviting)
            this.invite.stop();
    }

    private onState(state: Domain.Game.CustomState) {
        if (!state.owner) return;

        var one = this.$('#team-one');
        var two = this.$('#team-two');
        one.empty();
        two.empty();

        if (!this.room && state.chatroom != guid.empty) {
            this.room = new ChatRoom(state.chatroom);
            this.refs.chatContainer.add(this.room.node);
        }

        for (var i = 0; i < state.blueTeam.length; i++) {
            this.drawMember(state.blueTeam[i], one);
        }

        for (var i = 0; i < state.redTeam.length; i++) {
            this.drawMember(state.redTeam[i], two);
        }

        if (!state.blueTeam.length) {
            let node = this.template('customlobbymember', {});
            node.addClass('blank');
            one.add(node)
        }

        if (!state.redTeam.length) {
            let node = this.template('customlobbymember', {});
            node.addClass('blank');
            two.add(node)
        }

        if (state.owner.id != state.me.id) {
            this.$('#start-game, #start-game-padd').css('display', 'none');
        } else {
            this.$('#start-game, #start-game-padd').css('display', null);
        }
    }

    private drawMember(member: Domain.Game.LobbyMember, team) {
        let node = this.template('customlobbymember', {
            id: member.id,
            name: member.name,
        });
        if (member.champ) {
            node.$('.icon').src = Assets.champion.icon(member.champ)
        } else {
            Summoner.get(member.name).then(s => {
                node.$('.icon').src = Assets.summoner.icon(s.icon)
            });
        }

        var old = this.$("#member-" + member.id);
        if (old.length > 0) {
            old.replace(node);
        } else {
            team.add(node);
        }
    }

    private onStart(game) {
        this.doDispose = false;
        this.dispatch(this.start, game);
    }

    private onInviteClick() {
        this.inviting = !this.inviting;

        this.refs.inviteButton.setClass(this.inviting, 'inviting');

        if (this.inviting)
            this.invite.start();
        else
            this.invite.stop();
    }

    private onSwitchTeamsClick(e: MouseEvent) {
        Service.switchTeams();
    }

    private onStartGameClick(e: MouseEvent) {
        Service.start();
    }

    private onQuitGameClick(e: MouseEvent) {
        PlayLoop.quit();
        this.doDispose = false;
        this.dispatch(this.close, {});
    }
}