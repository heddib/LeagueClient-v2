import * as Kappa from './../kappa'

const service = '/assets';

export function loginVideo() {
    return Kappa.url(service + `/login/video`);
}

export function loginImage() {
    return Kappa.url(service + `/login/image`);
}

export function masteries() {
    return Kappa.invoke<Domain.GameData.MasteriesInfo>(service + `/game-data/masteries`, []);
}

export function runes() {
    return Kappa.invoke<Domain.GameData.RuneDetails[]>(service + `/game-data/runes`, []);
}

export function items() {
    return Kappa.invoke<Domain.GameData.ItemDetails[]>(service + `/game-data/items`, []);
}

export function champions() {
    return Kappa.invoke<Domain.GameData.ChampionSummary[]>(service + `/game-data/champions`, []);
}

export function summonerspells() {
    return Kappa.invoke<Domain.GameData.SummonerSpellDetails[]>(service + `/game-data/summonerspells`, []);
}

export function maps() {
    return Kappa.invoke<Domain.GameData.MapSummary[]>(service + `/game-data/maps`, []);
}

export function wardskins() {
    return Kappa.invoke<Domain.GameData.WardSkinSummary[]>(service + `/game-data/wardskins`, []);
}

export function champion(id: number) {
    return Kappa.invoke<Domain.GameData.ChampionDetails[]>(service + `/game-data/champion`, [id]);
}
export function info() {
    return Kappa.invoke<string>(service + '/info', []);
}