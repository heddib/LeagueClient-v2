import * as Kappa      from './../kappa'
const service = '/patcher';

export function game() {
    return Kappa.invoke<Domain.Patcher.PatcherState>(service + '/game', []);
}

export function launcher() {
    return Kappa.invoke<Domain.Patcher.PatcherState>(service + '/launcher', []);
}