import * as Service    from './service';

let queueCache;

export function queues() {
    return new Async<Domain.Game.AvailableQueue[]>((resolve, reject) => {
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