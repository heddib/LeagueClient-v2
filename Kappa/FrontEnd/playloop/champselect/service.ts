import * as Kappa      from './../../kappa'
const service = '/playloop/champselect';

var events = new EventModule();

export const state = new AsyncValue<Domain.ChampSelectState>();
export const start = events.create<{}>();
export const returnToLobby = events.create<{}>();
export const returnToQueue = events.create<{}>();
export const returnToCustom = events.create<{}>();

Kappa.subscribe(service + '/matchmaking', s => {
    events.dispatch(returnToQueue, s);
});

Kappa.subscribe(service + '/custom', s => {
    events.dispatch(returnToCustom, s);
});

Kappa.subscribe(service + '/lobby', s => {
    events.dispatch(returnToLobby, s);
});

Kappa.subscribe(service + '/start', s => {
    events.dispatch(start, s);
});

Kappa.subscribe(service + '/state', s => {
    state.set(s);
});

export function selectChampion(champ: number) {
    return Kappa.invoke(service + '/selectChampion', [champ]);
}

export function selectSkin(skinId: number) {
    return Kappa.invoke(service + '/selectSkin', [skinId]);
}

export function selectSpells(one: number, two: number) {
    return Kappa.invoke(service + '/selectSpells', [one, two]);
}

export function lockIn() {
    return Kappa.invoke(service + '/lockIn', []);
}

export function trade(id) {
    return Kappa.invoke(service + '/trade', [id]);
}

export function decline(id) {
    return Kappa.invoke(service + '/decline', [id]);
}

export function reroll() {
    return Kappa.invoke(service + '/reroll', []);
}