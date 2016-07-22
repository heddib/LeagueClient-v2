import * as Discord from 'discord-domain';

let token: string;
let socket: WebSocket;
let events = new EventModule();

export const ready = events.create<Discord.Gateway.ReadyData>();
export const presenceUpdate = events.create<Discord.Gateway.PresenceData>();

function api_get(path: string) {
    return new Promise<any>((resolve, reject) => {
        var req = new XMLHttpRequest();
        req.open('GET', 'https://discordapp.com/api' + path, true);
        req.setRequestHeader('Authorization', token);
        req.setRequestHeader('Content-Type', 'application/json');
        req.addEventListener('load', e => {
            resolve(JSON.parse(req.responseText));
        });
        req.send();
    });
}

function api_post(path: string, content: any) {
    return new Promise<any>((resolve, reject) => {
        var req = new XMLHttpRequest();
        req.open('POST', 'https://discordapp.com/api' + path, true);
        req.setRequestHeader('Authorization', token);
        req.setRequestHeader('Content-Type', 'application/json');
        req.addEventListener('load', e => {
            resolve(JSON.parse(req.responseText));
        });
        req.send(JSON.stringify(content));
    });
}

function api_send(data: Discord.Gateway.Payload) {
    socket.send(JSON.stringify(data));
}

const handlers: { [id: string]: X_Event<any> } = {
    'READY': ready,
    'PRESENCE_UPDATE': presenceUpdate
};

function onMessage(msg: MessageEvent) {
    let data: Discord.Gateway.Payload = JSON.parse(msg.data);
    let handler = handlers[data.t];
    if (handler) events.dispatch(handler, data.d);
    else {
        console.groupCollapsed(data.t);
        console.log(data.d);
        console.groupEnd();
    }
}

export namespace Auth {
    export function login(email: string, pass: string) {
        return new Promise<{}>((resolve, reject) => {
            api_post('/auth/login', {
                'email': email,
                'password': pass
            }).then(data => {
                token = data.token;
                resolve({});
            });
        })
    }
}

export namespace Gateway {
    export function connect() {
        api_get('/gateway').then(data => {
            socket = new WebSocket(data.url);
            socket.addEventListener('message', onMessage);
            socket.addEventListener('open', e => {
                api_send({
                    'op': 2,
                    'd': {
                        'token': token,
                        'v': 3,
                        'properties': {
                            '$os': 'Windows',
                            '$browser': 'Chrome',
                            '$device': '',
                            '$referrer': 'https://discordapp.com/@me',
                            '$referring_domani': 'discordapp.com'
                        }
                    }
                });
            });
        });
    }
}