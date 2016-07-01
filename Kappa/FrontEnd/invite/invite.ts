import { Swish }     from './../ui/swish';
import Module        from './../ui/module';
import * as Audio    from './../assets/audio';
import * as Assets   from './../assets/assets';
import * as Summoner from './../summoner/summoner';
import * as Service  from './service';

const html = Module.import('invite');

let invites = {};
var events = new EventModule();
export const update = events.create<any[]>();

Service.invite.on((invite) => {
    Audio.effect('home', 'invite');
    invites[invite.id] = invite;
    events.dispatch(update, list());
});

export function list() {
    var list = [];
    for (var id in invites) list.push(invites[id]);
    return list;
}

export function send(id: number) {
    return Service.send(id);
}

export function accept(invite) {
    delete invites[invite.id];
    events.dispatch(update, list());
    return Service.accept(invite.id, true);
}

export function decline(invite) {
    delete invites[invite.id];
    events.dispatch(update, list());
    return Service.accept(invite.id, false);
}

export class Control extends Module {
    public custom = this.create<any>();
    public lobby = this.create<any>();

    public constructor(invite) {
        super(html);

        this.refs.name.text = invite.from.name;
        this.refs.game.text = Assets.getQueueType(invite.game.queue).display;

        Summoner.get(invite.from.name).then(s => {
            this.node.$('.icon').src = Assets.image('profile', s.profileIconId)
        });

        this.refs.deny.on('mouseup', e => decline(invite));
        this.refs.accept.on('mouseup', e => {
            accept(invite).then(lobby => {
                if (invite.game.queue == 0) this.dispatch(this.custom, lobby);
                else this.dispatch(this.lobby, lobby);
            });
        });
    }
}