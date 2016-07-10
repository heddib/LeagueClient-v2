import * as Kappa from './../kappa'
const service = '/login';

export function saved() {
    return Kappa.invoke<any[]>(service + '/saved', []);
}


export function load(user: string) {
    return Kappa.invoke<any>(service + '/load', [user]);
}


export function auth(user: string, pass: string, save: boolean) {
    return Kappa.invoke<any>(service + '/auth', [user, pass, save]);
}

export function login() {
    return Kappa.invoke<Domain.Authentication.AccountState>(service + '/login', []);
}
