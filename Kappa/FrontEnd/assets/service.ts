import * as Kappa from './../kappa'

const service = '/assets';

export function loginVideo() {
    return Kappa.url(service + `/login/video`);
}

export function loginImage() {
    return Kappa.url(service + `/login/image`);
}

export function masteries() {
    return Kappa.invoke<Domain.GameData.MasteriesInfo>(service + `/masteries`, []);
}

export function runes() {
    return Kappa.invoke<Domain.GameData.RuneDetails[]>(service + `/runes`, []);
}

export function items() {
    return Kappa.invoke<Domain.GameData.ItemDetails[]>(service + `/items`, []);
}

export function champions() {
    return Kappa.invoke<Domain.GameData.ChampionSummary[]>(service + `/champions`, []);
}

export function summonerspells() {
    return Kappa.invoke<Domain.GameData.SummonerSpellDetails[]>(service + `/summonerspells`, []);
}

export function champion(id: number) {
    return Kappa.invoke<Domain.GameData.SummonerSpellDetails[]>(service + `/champion`, [id]);
}
export function info() {
    return Kappa.invoke<string>(service + '/info', []);
}