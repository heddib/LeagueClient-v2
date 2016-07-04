import * as Kappa      from './../../kappa'
const service = '/profile/matches';

Kappa.subscribe(service + '/new', msg => {
    events.dispatch(match, msg);
});

var events = new EventModule();

export const match = events.create<Domain.MatchHistory.MatchDetails>();

export function history(account: number) {
    return Kappa.invoke<Domain.MatchHistory.PlayerHistory>(service + '/history', [account]);
}

export function deltas() {
    return Kappa.invoke<Domain.MatchHistory.PlayerDeltas>(service + '/deltas', []);
}

export function details(gameid) {
    return Kappa.invoke<Domain.MatchHistory.MatchDetails>(service + '/details', [gameid]);
}