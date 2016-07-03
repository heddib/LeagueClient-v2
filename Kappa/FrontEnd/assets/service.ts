import * as Kappa from './../kappa'

const service = '/assets';

export function loginVideo() {
    return Kappa.url(service + `/login/video`);
}

export function loginImage() {
    return Kappa.url(service + `/login/image`);
}

export function masteries() {
    return Kappa.invoke<GameData.MasteriesInfo>(service + `/masteries`, []);
}

export function runes() {
    return Kappa.invoke<GameData.RuneDetails[]>(service + `/runes`, []);
}

export function items() {
    return Kappa.invoke<GameData.ItemDetails[]>(service + `/items`, []);
}

export function champions() {
    return Kappa.invoke<GameData.ChampionSummary[]>(service + `/champions`, []);
}

export function summonerspells() {
    return Kappa.invoke<GameData.SummonerSpellDetails[]>(service + `/summonerspells`, []);
}

export function champion(id: number) {
    return Kappa.invoke<GameData.SummonerSpellDetails[]>(service + `/champion`, [id]);
}
export function info() {
    return Kappa.invoke<string>(service + '/info', []);
}