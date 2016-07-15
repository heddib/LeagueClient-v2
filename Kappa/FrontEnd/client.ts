import * as Kappa    from './kappa';
import { Swish, $  } from './ui/swish';
import Module        from './ui/module';
import http          from './util/http';
import LoginPage     from './login/login';
import PatcherPage   from './patcher/launcher';
import Landing       from './landing/landing';

import * as Assets   from './assets/assets';
import * as Audio    from './assets/audio';
import * as Meta     from './meta/meta';

import * as Electron from './electron';
import * as Discord  from './discord/discord';
import * as Settings from './meta/settings';

let count = 0;

window['Electron'] = Electron;
window['Assets'] = Assets;
window['MyAudio'] = Audio;
window['Discord'] = Discord;
window['Settings'] = Settings;
window['Kappa'] = Kappa;

window.addEventListener('load', () => {
    PatcherPage.required().then(required => {
        (required ? onLoaded : onPatched)()
    });

    $('#exit-button').on('mouseup', e => Kappa.close());
    $('#min-button').on('mouseup', e => Kappa.minimize());

    $(document.body).on('contextmenu', e => {
        e.preventDefault();
    });
});

window.addEventListener('keyup', e => {
    switch (e.keyCode) {
        case 123:
            Meta.link(`http://${window.location.host}/ui/diagnostics/info`);
            break;
    }
});

let module: Module<any>;
function show(mod: Module<any>) {
    if (module) module.dispose();

    $('#client-area').empty();
    module = mod;
    module.render($('#client-area'));
}

function onLoaded() {
    let page = new PatcherPage();
    page.closed.on(() => onPatched());
    Electron.show();

    show(page);
}

function onPatched() {
    var page = new LoginPage();
    page.load.on(() => Electron.show());
    page.auth.on(state => onAuthed(state));

    show(page);
}

function onAuthed(accountState) {
    var page = new Landing(accountState);

    show(page);
}
