import * as Kappa from './../../kappa'
const service = '/collection/champions';

export function inventory() {
    return Kappa.invoke<Domain.ChampionInventory>(service + '/inventory', []);
}

export function mastery(summoner: number) {
    return Kappa.invoke<Domain.ChampionMastery.ChampionMasteryDTO[]>(service + '/mastery', [summoner]);
}