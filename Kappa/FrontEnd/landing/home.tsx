import { Swish }   from './../ui/swish';
import Module         from './../ui/module';

import * as Invite    from './../invite/invite';

const template = (
    <module class="landing-home">
        <div class="left">
            <div class="invite-list" data-ref="inviteList"></div>
        </div>
        <x-flexpadd></x-flexpadd>
        <div class="right"/>
    </module>
);

interface Refs {
    inviteList: Swish,
}

export default class HomePage extends Module<Refs> {
    public custom = this.create<any>();
    public lobby = this.create<any>();

    constructor() {
        super(template);

        Invite.update.on(e => this.drawInvites(e));
        this.drawInvites(Invite.list());
    }

    private drawInvites(list) {
        this.refs.inviteList.empty();
        for (let invite of list) {
            var control = new Invite.Control(invite);
            control.custom.on(e => this.dispatch(this.custom, e));
            control.lobby.on(e => this.dispatch(this.lobby, e));
            control.render(this.refs.inviteList);
        }
    }
}