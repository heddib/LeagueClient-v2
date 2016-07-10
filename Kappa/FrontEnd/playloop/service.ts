import * as Kappa      from './../kappa'
const service = '/playloop';

export function getAvailableQueues() {
    return Kappa.invoke<Domain.Game.AvailableQueue[]>(service + '/listqueues', []);
}

export function current() {
    return Kappa.invoke<Domain.Game.PlayLoopState>(service + '/current', []);
}

export function abandon() {
    return Kappa.invoke(service + '/abandon', []);
}

export function quit() {
    return Kappa.invoke(service + '/quit', []);
}