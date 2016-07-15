import * as Service    from './service';
import * as Defer      from './../defer';

let summoners: { [name: string]: Promise<Domain.Summoner.SummonerSummary> } = {};
let icons: { [id: number]: number } = {};

Service.me.on((m) => me.set(m));

export var me = new AsyncValue<any>();

export function store(): Promise<string> {
    return Service.store();
}

export function icon(ids: number[]) {
    return new MuiltPromise<{ [id: number]: number }>((resolve, reject) => {
        resolve(icons);

        Service.icon(ids).then(map => {
            load(map);
            resolve(icons);
        });
    });
}

export function get(name: string): Promise<Domain.Summoner.SummonerSummary> {
    return new Promise<Domain.Summoner.SummonerSummary>((resolve, reject) => {
        if (!summoners[name])
            summoners[name] = Service.get(name);

        summoners[name].then(s => resolve(s));
    });
}

export function details(summary: Domain.Summoner.SummonerSummary): Promise<Domain.Summoner.SummonerSummary> {
    return Service.details(summary.accountId);
}

export function kudos(id: number): Promise<Domain.Summoner.SummonerKudos> {
    return Service.kudos(id);
}

function load(map) {
    if (map) {
        for (var id in map)
            icons[id] = map[id];
    }
}