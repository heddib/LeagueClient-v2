import * as Kappa from  './../kappa';
const service = '/meta';

export function link(url: string) {
    return Kappa.invoke(service + '/link', [url]);
}