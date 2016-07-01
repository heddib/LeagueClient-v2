import * as Kappa from './../../kappa'
const service = '/collection/skins';

export function owned() {
    return Kappa.invoke<{ [id: number]: Skin[]}>(service + '/owned', []);
}