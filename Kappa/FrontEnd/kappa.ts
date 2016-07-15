import http          from './util/http';
import * as electron from './electron';

const ws = new WebSocket(`ws://${window.location.host}/async`, 'protocolTwo');
ws.addEventListener('message', e => {
    var msg = JSON.parse(e.data);
    switch (msg.type) {
        case 'async':
            onAsync(msg.data);
            break;
        case 'callback':
            onCallback(msg.data);
            break;
    }
});
ws.addEventListener('close', () => {
    electron.close();
});
ws.addEventListener('error', e => {
    console.log(e);
});

const asyncs: { [id: string]: Function[] } = {};

export function close() {
    invoke('/meta/close', []);
}

export function url(path: string) {
    return `http://${window.location.host}/kappa${path}`;
}

export function minimize() {
    electron.minimize();
}

export function subscribe(source: string, callback: (arg: any) => void) {
    source = '/kappa' + source;
    if (!asyncs[source])
        asyncs[source] = [callback];
    else
        asyncs[source].push(callback);
}

export function invoke<T>(path: string, args: any[]) {
    return new Promise<T>((resolve, reject) => {
        var start = new Date();

        var str = JSON.stringify(args);
        http(url(path)).post(str, http => {
            var data = http.json;
            if (data.error) {
                console.group('ERROR: ' + path);
                console.error(data.error.message);
                console.error(data.error.trace);
                console.groupEnd();
                reject(data.error);
            } else {
                resolve(data.value);
            }
        });
    });
}

function onAsync(data) {
    let subscribed = asyncs[data.source];
    if (!subscribed) {
        console.error('Unhandled async ' + JSON.stringify(data));
    } else {
        subscribed.forEach(a => a(data.body.value));
    }
}

let callbacks = {};
function onCallback(data) {
    var id = data.id;
    var callback = callbacks[id];
    delete callbacks[id];
    callback(data);
}