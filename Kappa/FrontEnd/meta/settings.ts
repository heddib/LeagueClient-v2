import * as Kappa from  './../kappa';
import * as Defer from  './../defer';
const service = '/meta/settings';

let userSettings: Settings;

Defer.auth(() => {
    Kappa.invoke<Settings>(service + '/user', []).then(sets => userSettings = sets);
});

export function save() {
    Kappa.invoke(service + '/user/patch', [userSettings]);
}

interface Settings {
    chat: ChatSettings;
    playloop: PlayLoopSettings;
}

interface ChatSettings {
    showOffline: boolean;
}

interface PlayLoopSettings {
    autoAfkCheck: boolean;
    autoLockIn: boolean;
}