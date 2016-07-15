import * as Kappa from  './../kappa';
import * as Defer from  './../defer';
const service = '/meta/settings';

let userSettings;
let callbacks: Function[] = [];

Defer.auth(() => {
    Kappa.invoke<any>(service + '/user', []).then(sets => {
        userSettings = sets;
        while (callbacks[0]) callbacks.shift()();
    });
});

function save() {
    Kappa.invoke(service + '/user/patch', [userSettings]);
}

export default class Settings<T> {
    private key: string;
    constructor(key: string) {
        this.key = key;
    }
    public load() {
        return new Promise<T>((resolve, reject) => {
            callbacks.push(() => resolve(userSettings[this.key] as T));
        });
    }
    public save(value: T) {
        userSettings[this.key] = value;
        save();
    }
}