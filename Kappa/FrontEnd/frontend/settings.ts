import * as Defer from  './../defer';

import { Meta as Service } from './../backend/services';

let userSettings;
let callbacks: Function[] = [];

Defer.auth(() => {
    Service.getSettings().then(sets => {
        userSettings = sets;
        while (callbacks[0]) callbacks.shift()();
    });
});

function save() {
    Service.saveSettings(userSettings);
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