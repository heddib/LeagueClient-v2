import * as Kappa      from './../../kappa'
const service = '/playloop/custom';

var events = new EventModule();

export const state = new AsyncValue<Domain.Game.CustomState>();
export const champselect = events.create<{}>();

Kappa.subscribe(service + '/state', s => {
    state.set(s);
});

Kappa.subscribe(service + '/champselect', s => {
    events.dispatch(champselect, s);
});


export function create() {
    return Kappa.invoke(service + '/create', []);
}

export function switchTeams() {
    return Kappa.invoke(service + '/switchTeams', []);
}

export function start() {
    return Kappa.invoke(service + '/start', []);
}