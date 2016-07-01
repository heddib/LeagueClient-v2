import * as Kappa      from './../../kappa'
const service = '/profile/matches';

export function history() {
    return Kappa.invoke<MatchHistory.PlayerHistory>(service + '/history', []);
}

export function deltas() {
    return Kappa.invoke<MatchHistory.PlayerDeltas>(service + '/deltas', []);
}

export function details(gameid) {
    return Kappa.invoke<MatchHistory.MatchDetails>(service + '/details', [gameid]);
}