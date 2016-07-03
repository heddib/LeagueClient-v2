import * as Kappa from './../../kappa'
const service = '/collection/runes';

export function get() {
    return Kappa.invoke<Domain.Collection.RuneBook>(service + '/get', []);
}

export function save(page: Domain.Collection.RunePage) {
    return Kappa.invoke(service + '/save', [page]);
}

export function select(page: Domain.Collection.RunePage) {
    return Kappa.invoke(service + '/select', [page.id]);
}