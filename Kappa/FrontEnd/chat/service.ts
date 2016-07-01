import * as Kappa from './../kappa'

const service = '/chat';

Kappa.subscribe(service + '/status', stat => {
    events.dispatch(status, stat);
});

Kappa.subscribe(service + '/message', msg => {
    events.dispatch(message, msg);
});

var events = new EventModule();

export const status = events.create<any>();
export const message = events.create<any>();

export function send(user: string, msg: string) {
    return Kappa.invoke(service + '/message', [user, msg]);
}
