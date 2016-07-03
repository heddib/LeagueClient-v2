import * as Kappa from './../../kappa'
const service = '/collection/skins';

export function owned() {
    return Kappa.invoke<{ [id: number]: Domain.Collection.Skin[]}>(service + '/owned', []);
}