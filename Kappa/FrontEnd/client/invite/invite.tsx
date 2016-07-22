import Module        from './../ui/module';

import * as Audio    from './../../frontend/audio';
import * as Assets   from './../../frontend/assets';
import * as Invite   from './../../frontend/invite';
import * as Summoner from './../../frontend/summoner';

const template = (
    <module class="invitation">
        <img class="icon"/>
        <div class="text">
            <span data-ref="name"></span>
            <span data-ref="game"></span>
        </div>
        <x-flexpadd></x-flexpadd>
        <div class="buttons">
            <span data-ref="accept" class="accept-button"></span>
            <span data-ref="deny" class="exit-button"></span>
        </div>
    </module>
);

Invite.received.on(() => {
    Audio.effect('home', 'invite');
});

interface Refs {
    name: Swish;
    game: Swish;
    deny: Swish;
    accept: Swish;
}

export default class Control extends Module<Refs> {
    public custom = this.create<any>();
    public lobby = this.create<any>();

    public constructor(invite) {
        super(template);

        this.refs.name.text = invite.from.name;
        this.refs.game.text = Assets.getQueueType(invite.game.queue).display;

        Summoner.get(invite.from.name).then(s => {
            this.node.$('.icon').src = Assets.summoner.icon(s.icon);
        });

        this.refs.deny.on('mouseup', e => Invite.decline(invite));
        this.refs.accept.on('mouseup', e => {
            Invite.accept(invite).then(lobby => {
                if (invite.game.queue == 0) this.dispatch(this.custom, invite);
                else this.dispatch(this.lobby, invite);
            });
        });
    }
}