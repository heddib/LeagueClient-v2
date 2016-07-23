import http          from './../util/http';

import Module        from './ui/module';
import LoginPage     from './login/login';
import PatcherPage   from './patcher/launcher';
import Landing       from './landing/landing';

import * as Assets   from './../frontend/assets';
import * as Audio    from './../frontend/audio';
import { Meta }      from './../backend/services';

import * as CEF      from './../cef';

let count = 0;

window['Assets'] = Assets;
window['MyAudio'] = Audio;

window.addEventListener('load', () => {
    PatcherPage.required().then(required => {
        (required ? onLoaded : onPatched)()
    });

    swish('#exit-button').on('mouseup', e => Meta.close());
    swish('#min-button').on('mouseup', e => CEF.minimize());

    swish(document.body).on('contextmenu', e => {
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

    swish('#client-area').empty();
    module = mod;
    module.render(swish('#client-area'));
}

function onLoaded() {
    let page = new PatcherPage();
    page.closed.on(() => onPatched());
    CEF.show();

    show(page);
}

function onPatched() {
    var page = new LoginPage();
    page.load.on(() => CEF.show());
    page.auth.on(state => onAuthed(state));

    show(page);
}

function onAuthed(accountState) {
    var page = new Landing(accountState);

    show(page);
}
