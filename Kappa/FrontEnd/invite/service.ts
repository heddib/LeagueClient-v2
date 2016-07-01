import * as Kappa      from './../kappa'

const service = '/invite';

var events = new EventModule();

export const invite = events.create<any>();

Kappa.subscribe(service + '/invite', i => {
    events.dispatch(invite, i);
});

export function send(summId) {
    return Kappa.invoke(service + '/invite', [summId]);
}

export function accept(inviteId, accept: boolean) {
    return Kappa.invoke(service + '/accept', [inviteId, accept]);
}