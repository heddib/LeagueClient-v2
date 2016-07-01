import * as Kappa from './../../kappa'
const service = '/collection/masteries';

export function get() {
    return Kappa.invoke<MasteryBookDTO>(service + '/get', []);
}

export function save(page: MasteryBookPageDTO) {
    return Kappa.invoke(service + '/save', [page]);
}

export function select(page: MasteryBookPageDTO) {
    return Kappa.invoke(service + '/select', [page.pageId]);
}