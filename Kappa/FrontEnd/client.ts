import * as Kappa    from './kappa';
import { Swish, $  } from './ui/swish';
import Module        from './ui/module';
import LoginPage     from './login/login';
import PatcherPage   from './patcher/launcher';
import Landing       from './landing/landing';

import * as Assets   from './assets/assets';
import * as Meta     from './meta/meta';

import * as Electron from './electron';
import * as Discord  from './discord/discord';
import * as Settings from './meta/settings';

let count = 0;

window['Electron'] = Electron;
window['Assets'] = Assets;
window['Discord'] = Discord;
window['Settings'] = Settings;
window['Kappa'] = Kappa;

window.addEventListener('load', () => {
    PatcherPage.required().then(required => (required ? onLoaded : onPatched)());

    $('#exit-button').on('mouseup', e => Kappa.close());
    $('#min-button').on('mouseup', e => Kappa.minimize());

    $(document.body).on('contextmenu', e => {
        e.preventDefault();
    });
});

window.addEventListener('keyup', e => {
    if (e.keyCode != 123) return;
    let url = `http://${window.location.host}/ui/diagnostics/info`;
    Meta.link(url);
});

let module: Module;
function show(mod: Module) {
    if (module) module.dispose();

    $('#client-area').empty();
    module = mod;
    module.render($('#client-area'));
}

function onLoaded() {
    let page = new PatcherPage();
    page.complete.on(() => onPatched());
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
