import * as Kappa      from './../../kappa'
const service = '/playloop/ingame';

var events = new EventModule();

export const state = events.create<Domain.Game.ActiveGameState>();

Kappa.subscribe(service + '/state', s => {
    events.dispatch(state, s)
});

export function launch() {
    return Kappa.invoke(service + '/launch', []);
}