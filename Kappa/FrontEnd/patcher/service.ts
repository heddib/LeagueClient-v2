import * as Kappa      from './../kappa'
const service = '/patcher';

export function game() {
    return Kappa.invoke<PatcherState>(service + '/game', []);
}

export function launcher() {
    return Kappa.invoke<PatcherState>(service + '/launcher', []);
}