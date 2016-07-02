import * as Service    from './service';
import * as Defer      from './../defer';
import Storage         from './../util/storage';

const storage = new Storage('summoner');

let summoners: { [name: string]: Async<PublicSummoner> } = {};
let icons: { [id: number]: number } = {};

Service.me.on((m) => me.set(m));

export var me = new AsyncValue<any>();

export function store(): Async<string> {
    return Service.store();
}

export function icon(ids: number[]) {
    return new Async<{ [id: number]: number }>((resolve, reject) => {
        resolve(icons);

        Service.icon(ids).then(map => {
            load(map);
            resolve(icons);
        });
    });
}

export function get(name: string): Async<PublicSummoner> {
    return new Async<PublicSummoner>((resolve, reject) => {
        if (!summoners[name])
            summoners[name] = Service.get(name);

        summoners[name].then(s => resolve(s));
    });
}

export function kudos(id: number): Async<Summoner.SummonerKudos> {
    return Service.kudos(id);
}

load(storage.get('icons'));
function load(map) {
    if (map) {
        for (var id in map)
            icons[id] = map[id];
        storage.set('icons', icons);
    }
}