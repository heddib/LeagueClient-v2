import { Defer as Service } from './../backend/services';

interface IHandler {
    done: boolean;
    handlers: (() => void)[];
}

let handlers: { [id: string]: IHandler } = {}
let deferable = ['auth'];

deferable.forEach(e => handlers[e] = { done: false, handlers: [] });

function defer(key: string, callback: () => void) {
    if (handlers[key].done) {
        callback();
    } else {
        handlers[key].handlers.push(callback);
    }
}

export function auth(callback: () => void) {
    defer('auth', callback);
}

function dispatch(name: string) {
    handlers[name].done = true;
    while (handlers[name].handlers.length)
        handlers[name].handlers.shift()();
}

Service.auth.on(() => dispatch('auth'));
