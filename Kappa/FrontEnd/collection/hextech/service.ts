import * as Kappa from './../../kappa'
const service = '/collection/hextech';

Kappa.subscribe(service + '/update', msg => {
    events.dispatch(update, msg);
});

var events = new EventModule();

export const update = events.create<Domain.Collection.HextechInventory>();

export function inventory() {
    return Kappa.invoke<Domain.Collection.HextechInventory>(service + '/inventory', []);
}