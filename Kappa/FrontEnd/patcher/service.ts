import * as Kappa      from './../kappa'
const service = '/patcher';

export function game() {
    return Kappa.invoke<Domain.PatcherState>(service + '/game', []);
}

export function launcher() {
    return Kappa.invoke<Domain.PatcherState>(service + '/launcher', []);
}