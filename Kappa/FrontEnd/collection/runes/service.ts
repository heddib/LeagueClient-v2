import * as Kappa from './../../kappa'
const service = '/collection/runes';

export function get() {
    return Kappa.invoke<Domain.Runes.SpellBookDTO>(service + '/get', []);
}

export function save(page: Domain.Runes.SpellBookPageDTO) {
    return Kappa.invoke(service + '/save', [page]);
}

export function select(page: Domain.Runes.SpellBookPageDTO) {
    return Kappa.invoke(service + '/select', [page.pageId]);
}