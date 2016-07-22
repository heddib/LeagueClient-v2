import { PlayLoop as Service } from './../backend/services';

export const queueNames = {
    2: 'Blind Pick', // Rift
    8: 'Blind Pick', // TT
    // 41: 'Ranked Teams', // TT
    65: 'ARAM', // Abyss
    400: 'Normal', // Rift
    410: 'Ranked', // Rift
};

export const featuredNames = {
    70: 'One for All',
    75: 'Hexakill',
    76: 'URF',
    96: 'Ascension',
    98: 'Twisted Treeline Hexakill',
    300: 'Legend of the Poro King',
    // 317: 'Definitely not Dominion'
};

let queueCache;

export function queues() {
    return new Promise<Domain.Game.AvailableQueue[]>((resolve, reject) => {
        if (queueCache) resolve(queueCache);
        Service.getAvailableQueues().then(queues => {
            queueCache = queues;
            resolve(queues);
        });
    });
}

export function current() {
    return Service.current();
}

export function abandon() {
    return Service.abandon();
}

export function quit() {
    return Service.quit();
}