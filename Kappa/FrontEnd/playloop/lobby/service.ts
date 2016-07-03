import * as Kappa      from './../../kappa'
const service = '/playloop';

Kappa.subscribe(service + '/lobby/state', s => {
    lobbyState.set(s);
});

Kappa.subscribe(service + '/lobby/matchmaking', s => {
    events.dispatch(matchmakingStart, s);
});

Kappa.subscribe(service + '/matchmaking/lobby', s => {
    events.dispatch(matchmakingStop, s);
});

Kappa.subscribe(service + '/matchmaking/state', s => {
    queueState.set(s);
});

Kappa.subscribe(service + '/matchmaking/champselect', s => {
    events.dispatch(champselectStart, s);
});

var events = new EventModule();

export const lobbyState = new AsyncValue<Domain.Game.LobbyState>();

export const matchmakingStart = events.create<{}>();
export const matchmakingStop = events.create<{}>();
export const champselectStart = events.create<{}>();

export const queueState = new AsyncValue<Domain.Game.MatchmakingState>();

export function create(id: number) {
    return Kappa.invoke(service + '/lobby/create', [id]);
}

export function startMatchmaking() {
    return Kappa.invoke(service + '/lobby/matchmake', []);
}

export function selectRoles(one: string, two: string) {
    return Kappa.invoke(service + '/lobby/selectRoles', [one, two]);
}

export function stopMatchmaking() {
    return Kappa.invoke(service + '/matchmaking/cancel', []);
}

export function afkCheck(accept: boolean) {
    return Kappa.invoke(service + '/matchmaking/afkcheck', [accept]);
}