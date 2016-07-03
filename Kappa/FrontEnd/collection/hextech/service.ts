import * as Kappa from './../../kappa'
const service = '/collection/hextech';

export function inventory() {
    return Kappa.invoke<Domain.HextechInventory>(service + '/inventory', []);
}