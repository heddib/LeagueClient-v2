import * as Kappa from './../../kappa'
const service = '/collection/masteries';

export function get() {
    return Kappa.invoke<Domain.Masteries.MasteryBookDTO>(service + '/get', []);
}

export function save(page: Domain.Masteries.MasteryBookPageDTO) {
    return Kappa.invoke(service + '/save', [page]);
}

export function select(page: Domain.Masteries.MasteryBookPageDTO) {
    return Kappa.invoke(service + '/select', [page.pageId]);
}