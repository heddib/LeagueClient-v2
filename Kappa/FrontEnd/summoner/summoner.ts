import * as Service    from './service';
import * as Defer      from './../defer';

let summoners: { [name: string]: Async<Domain.Summoner.SummonerSummary> } = {};
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

export function get(name: string): Async<Domain.Summoner.SummonerSummary> {
    return new Async<Domain.Summoner.SummonerSummary>((resolve, reject) => {
        if (!summoners[name])
            summoners[name] = Service.get(name);

        summoners[name].then(s => resolve(s));
    });
}

export function kudos(id: number): Async<Domain.Summoner.SummonerKudos> {
    return Service.kudos(id);
}

function load(map) {
    if (map) {
        for (var id in map)
            icons[id] = map[id];
    }
}