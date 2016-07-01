import * as Kappa from './../../kappa'
const service = '/collection/runes';

export function get() {
    return Kappa.invoke<SpellBookDTO>(service + '/get', []);
}

export function save(page: SpellBookPageDTO) {
    return Kappa.invoke(service + '/save', [page]);
}

export function select(page: SpellBookPageDTO) {
    return Kappa.invoke(service + '/select', [page.pageId]);
}