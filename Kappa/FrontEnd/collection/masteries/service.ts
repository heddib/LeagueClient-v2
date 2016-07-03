import * as Kappa from './../../kappa'
const service = '/collection/masteries';

export function get() {
    return Kappa.invoke<Domain.Collection.MasteryBook>(service + '/get', []);
}

export function save(page: Domain.Collection.MasteryPage) {
    return Kappa.invoke(service + '/save', [page]);
}

export function select(page: Domain.Collection.MasteryPage) {
    return Kappa.invoke(service + '/select', [page.id]);
}