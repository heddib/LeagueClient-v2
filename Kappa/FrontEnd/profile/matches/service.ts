import * as Kappa      from './../../kappa'
const service = '/profile/matches';

export function history() {
    return Kappa.invoke<Domain.MatchHistory.PlayerHistory>(service + '/history', []);
}

export function deltas() {
    return Kappa.invoke<Domain.MatchHistory.PlayerDeltas>(service + '/deltas', []);
}

export function details(gameid) {
    return Kappa.invoke<Domain.MatchHistory.MatchDetails>(service + '/details', [gameid]);
}