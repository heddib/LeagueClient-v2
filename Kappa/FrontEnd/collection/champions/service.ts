import * as Kappa from './../../kappa'
const service = '/collection/champions';

export function inventory() {
    return Kappa.invoke<ChampionInventory>(service + '/inventory', []);
}

export function mastery(summoner: number) {
    return Kappa.invoke<ChampionMasteryDTO[]>(service + '/mastery', [summoner]);
}