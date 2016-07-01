import * as Kappa      from './../kappa';
const service = '/summoner';

var events = new EventModule();
export const me = events.create<any>();

Kappa.subscribe(service + '/me', m => {
    events.dispatch(me, m);
})

export function get(name: string) {
    return Kappa.invoke<PublicSummoner>(service + '/get', [name]);
}

export function icon(ids: number[]) {
    return Kappa.invoke<{ [id: number]: number }>(service + '/icon', [ids]);
}

export function store() {
    return Kappa.invoke<string>(service + '/store', []);
}
