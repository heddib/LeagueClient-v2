import Module         from './../ui/module';
import InviteControl  from './../invite/invite';

import * as Invite    from './../../frontend/invite';

const template = (
    <module class="landing-home">
        <div class="left">
            <div class="invite-list" data-ref="inviteList"></div>
        </div>
        <x-flexpadd/>
        <div class="right">
        </div>
    </module>
);

interface Refs {
    inviteList: Swish,
}

export default class HomePage extends Module<Refs> {
    public custom = this.create<Domain.Game.Invitation>();
    public lobby = this.create<Domain.Game.Invitation>();

    constructor() {
        super(template);

        Invite.update.on(e => this.drawInvites(e));
        this.drawInvites(Invite.list());
    }

    private drawInvites(list) {
        this.refs.inviteList.empty();
        for (let invite of list) {
            var control = new InviteControl({
                invite,
                onCustom: e => this.dispatch(this.custom, e),
                onLobby: e => this.dispatch(this.lobby, e),
            });
            this.refs.inviteList.add(control.node);
        }
    }
}