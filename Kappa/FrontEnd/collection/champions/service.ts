import * as Kappa from './../../kappa'
const service = '/collection/champions';

export function inventory() {
    return Kappa.invoke<Domain.Collection.ChampionInventory>(service + '/inventory', []);
}

export function mastery(summoner: number) {
    return Kappa.invoke<any[]>(service + '/mastery', [summoner]);
}