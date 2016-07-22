import { Invite as Service } from './../backend/services';

let invites = {};
var events = new EventModule();
export const received = events.create<{}>();
export const update = events.create<any[]>();

Service.invite.on((invite) => {
    events.dispatch(received, {});
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