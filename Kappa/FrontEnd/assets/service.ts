import * as Kappa from './../kappa'

const service = '/assets';

export function loginVideo() {
    return Kappa.url(service + `/login/video`);
}

export function loginImage() {
    return Kappa.url(service + `/login/image`);
}

export function info() {
    return Kappa.invoke<string>(service + '/info', []);
}