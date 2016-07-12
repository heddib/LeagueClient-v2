import { Swish }     from './../ui/swish';
import Module        from './../ui/module';

const template = (
    <module class="invites-panel">
        <div class="header">
            <span class="window-button plus-button" data-ref="button"></span>
            <h2>Invited: </h2>
        </div>
        <div class="list" data-ref="list"></div>
    </module>
);

const inviteeTemplate = (
    <div class="lobby-invitee">
        <span data-ref="name"/>
        <span class="state" data-ref="state"/>
    </div>
);

export interface IProvider {
    start(): void;
    stop(): void;
    update(invites: Domain.Game.LobbyInvitee[]);
}

export class Panel extends Module {
    private provider: IProvider;
    private inviting: boolean;

    constructor(provider: IProvider) {
        super(template);

        this.provider = provider;
        this.refs.button.on('click', () => this.onInviteClick());
    }

    public dispose() {
        if (this.inviting)
            this.provider.stop();
    }

    public update(canInvite: boolean, invitees: Domain.Game.LobbyInvitee[]) {
        this.refs.button.css('display', canInvite ? null : 'none');

        if (!invitees) return;

        this.refs.list.empty();
        for (let invitee of invitees) {
            let mod = Module.create(inviteeTemplate);
            mod.refs.name.text = invitee.name;
            mod.refs.state.text = invitee.state[0].toUpperCase() + invitee.state.substring(1).toLowerCase();
            mod.node.addClass('invitee-' + invitee.state.toLowerCase());
            this.refs.list.add(mod.node);
        }

        this.provider.update(invitees);
    }

    private onInviteClick() {
        this.inviting = !this.inviting;

        this.refs.button.setClass(this.inviting, 'inviting');

        if (this.inviting) this.provider.start();
        else this.provider.stop();
    }
}