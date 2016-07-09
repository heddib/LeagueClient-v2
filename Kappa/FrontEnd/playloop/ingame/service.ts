import * as Kappa      from './../../kappa'
const service = '/playloop';

var events = new EventModule();

export const activeState = events.create<Domain.Game.ActiveGameState>();
Kappa.subscribe(service + '/ingame/state', s => {
    events.dispatch(activeState, s)
});

export const postState = events.create<Domain.Game.PostGameState>();
Kappa.subscribe(service + '/postgame/state', s => {
    events.dispatch(postState, s)
});

export const finished = events.create<boolean>();
Kappa.subscribe(service + '/ingame/finished', s => {
    events.dispatch(finished, s);
});

export function launch() {
    return Kappa.invoke(service + '/ingame/launch', []);
}

export function leave() {
    return Kappa.invoke(service + '/postgame/leave', []);
}