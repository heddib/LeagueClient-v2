import http          from './../util/http';
import * as CEF      from './../cef';

namespace Socket {
    const ws = new WebSocket(`ws://${window.location.host}/async`, 'protocolTwo');
    ws.addEventListener('message', e => {
        var msg = JSON.parse(e.data);
        if (msg.type == 'async') {
            onAsync(msg.data);
        }
    });
    ws.addEventListener('close', () => {
        CEF.close();
    });
    ws.addEventListener('error', e => {
        console.log(e);
    });

    const asyncs: { [id: string]: Function[] } = {};
    function onAsync(data) {
        let subscribed = asyncs[data.source];
        if (!subscribed) {
            console.error('Unhandled async ' + JSON.stringify(data));
        } else {
            subscribed.forEach(a => a(data.body.value));
        }
    }

    const events = new EventModule();
    export function listen<T>(path: string) {
        var e = events.create<T>();

        if (!asyncs[path]) asyncs[path] = [];
        asyncs[path].push(data => events.dispatch(e, data));

        return e;
    }
}

function close() {
    invoke<void>('/meta', '/close', []);
}

function url(base: string, path: string) {
    return `/kappa${base + path}`;
}

function invoke<T>(base: string, path: string, args: any[]) {
    return new Promise<T>((resolve, reject) => {
        var start = new Date();

        var str = JSON.stringify(args);
        http(url(base, path)).post(str, http => {
            var data = http.json;
            if (data.error) {
                console.group('ERROR: ' + base + path);
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


function async<T>(base: string, path: string) {
    return Socket.listen<T>(url(base, path));
}

function asyncValue<T>(base: string, path: string) {
    var val = new AsyncValue<T>();
    async<T>(base, path).on(s => val.set(s));
    return val;
}

export namespace Assets {
    const base = '/assets';

    export function loginVideo() {
        return url(base, `/login/video`);
    }

    export function loginImage() {
        return url(base, `/login/image`);
    }

    export function masteries() {
        return invoke<Domain.GameData.MasteriesInfo>(base, `/game-data/masteries`, []);
    }

    export function runes() {
        return invoke<Domain.GameData.RuneDetails[]>(base, `/game-data/runes`, []);
    }

    export function items() {
        return invoke<Domain.GameData.ItemDetails[]>(base, `/game-data/items`, []);
    }

    export function champions() {
        return invoke<Domain.GameData.ChampionSummary[]>(base, `/game-data/champions`, []);
    }

    export function summonerspells() {
        return invoke<Domain.GameData.SummonerSpellDetails[]>(base, `/game-data/summonerspells`, []);
    }

    export function maps() {
        return invoke<Domain.GameData.MapSummary[]>(base, `/game-data/maps`, []);
    }

    export function wardskins() {
        return invoke<Domain.GameData.WardSkinSummary[]>(base, `/game-data/wardskins`, []);
    }

    export function champion(id: number) {
        return invoke<Domain.GameData.ChampionDetails[]>(base, `/game-data/champion`, [id]);
    }
    export function info() {
        return invoke<string>(base, '/info', []);
    }
}

export namespace Chat {
    const base = '/chat';

    export const status = async<any>(base, '/status');
    export const message = async<any>(base, '/message');

    export function send(user: string, msg: string) {
        return invoke<void>(base, '/message', [user, msg]);
    }
}

export namespace ChatRoom {
    const base = '/chat/rooms';

    export const join = async<Domain.Chat.MucFriend>(base, '/memberJoin');
    export const leave = async<Domain.Chat.MucFriend>(base, '/memberLeave');
    export const message = async<Domain.Chat.MucMessage>(base, '/message');

    export function send(room: string, msg: string) {
        return invoke<void>(base, '/message', [room, msg]);
    }
}

export namespace Champions {
    const base = '/collection/champions';

    export function inventory() {
        return invoke<Domain.Collection.ChampionInventory>(base, '/inventory', []);
    }

    export function mastery(summoner: number) {
        return invoke<any[]>(base, '/mastery', [summoner]);
    }

    export function skins() {
        return invoke<{ [id: number]: Domain.Collection.Skin[] }>(base, '/skins', []);
    }
}

export namespace Hextech {
    const base = '/collection/hextech';

    export const update = async<Domain.Collection.HextechInventory>(base, '/update');

    export function inventory() {
        return invoke<Domain.Collection.HextechInventory>(base, '/inventory', []);
    }
}

export namespace Masteries {
    const base = '/collection/masteries';

    export function get() {
        return invoke<Domain.Collection.MasteryBook>(base, '/get', []);
    }

    export function save(page: Domain.Collection.MasteryPage) {
        return invoke<void>(base, '/save', [page]);
    }

    export function select(page: Domain.Collection.MasteryPage) {
        return invoke<void>(base, '/select', [page.id]);
    }
}

export namespace Runes {
    const base = '/collection/runes';

    export function get() {
        return invoke<Domain.Collection.RuneBook>(base, '/get', []);
    }

    export function save(page: Domain.Collection.RunePage) {
        return invoke<void>(base, '/save', [page]);
    }

    export function select(page: Domain.Collection.RunePage) {
        return invoke<void>(base, '/select', [page.id]);
    }
}

export namespace Invite {
    const base = '/invite';

    export const invite = async<any>(base, '/invite');

    export function send(summId) {
        return invoke<void>(base, '/invite', [summId]);
    }

    export function accept(inviteId, accept: boolean) {
        return invoke<void>(base, '/accept', [inviteId, accept]);
    }
}

export namespace Defer {
    const base = '/defer';

    export const auth = async<void>(base, '/auth');
}

export namespace Authentication {
    const base = '/login';

    export const kicked = async<void>(base, '/kicked');

    export function saved() {
        return invoke<Domain.Authentication.SavedAccount[]>(base, '/saved', []);
    }

    export function load(user: string) {
        return invoke<any>(base, '/load', [user]);
    }

    export function auth(user: string, pass: string, save: boolean) {
        return invoke<any>(base, '/auth', [user, pass, save]);
    }

    export function login() {
        return invoke<Domain.Authentication.AccountState>(base, '/login', []);
    }
}

export namespace Meta {
    const base = '/meta';

    export function close() {
        return invoke<void>(base, '/close', []);
    }

    export function link(url: string) {
        return invoke<void>(base, '/link', [url]);
    }

    export function getSettings() {
        return invoke<any>(base, '/settings/user', [])
    }

    export function saveSettings(userSettings: {}) {
        return invoke<void>(base, '/settings/user/patch', [userSettings]);
    }
}

export namespace Patcher {
    const base = '/patcher';

    export function game() {
        return invoke<Domain.Patcher.PatcherState>(base, '/game', []);
    }

    export function launcher() {
        return invoke<Domain.Patcher.PatcherState>(base, '/launcher', []);
    }
}

export namespace PlayLoop {
    const base = '/playloop';

    export function getAvailableQueues() {
        return invoke<Domain.Game.AvailableQueue[]>(base, '/listqueues', []);
    }

    export function current() {
        return invoke<Domain.Game.PlayLoopState>(base, '/current', []);
    }

    export function abandon() {
        return invoke(base, '/abandon', []);
    }

    export function quit() {
        return invoke(base, '/quit', []);
    }

    export namespace ChampSelect {
        const base = '/playloop/champselect';

        var events = new EventModule();

        export const state = asyncValue<Domain.Game.ChampSelectState>(base, '/state');
        export const start = async<void>(base, '/start');
        export const returnToLobby = async<void>(base, '/lobby');
        export const returnToQueue = async<void>(base, '/matchmaking');
        export const returnToCustom = async<void>(base, '/custom');

        export function selectChampion(champ: number) {
            return invoke(base, '/selectChampion', [champ]);
        }

        export function selectSkin(skinId: number) {
            return invoke(base, '/selectSkin', [skinId]);
        }

        export function selectSpells(one: number, two: number) {
            return invoke(base, '/selectSpells', [one, two]);
        }

        export function lockIn() {
            return invoke(base, '/lockIn', []);
        }

        export function trade(id) {
            return invoke(base, '/trade', [id]);
        }

        export function decline(id) {
            return invoke(base, '/decline', [id]);
        }

        export function reroll() {
            return invoke(base, '/reroll', []);
        }
    }

    export namespace Custom {
        const base = '/playloop/custom';

        var events = new EventModule();

        export const state = asyncValue<Domain.Game.CustomState>(base, '/state');
        export const champselect = async<Domain.Game.CustomState>(base, '/champselect');


        export function create() {
            return invoke(base, '/create', []);
        }

        export function switchTeams() {
            return invoke(base, '/switchTeams', []);
        }

        export function start() {
            return invoke(base, '/start', []);
        }
    }

    export namespace InGame {
        const base = '/playloop/ingame';

        var events = new EventModule();

        export const state = asyncValue<Domain.Game.ActiveGameState>(base, '/state');
        export const finished = async<boolean>(base, '/finished');

        export function launch() {
            return invoke(base, '/launch', []);
        }
    }

    export namespace PostGame {
        const base = '/playloop/postgame';

        export const state = asyncValue<Domain.Game.PostGameState>(base, '/state');

        export function leave() {
            return invoke(base, '/leave', []);
        }
    }

    export namespace Lobby {
        const base = '/playloop/lobby';

        export const state = asyncValue<Domain.Game.LobbyState>(base, '/state');
        export const matchmaking = asyncValue<void>(base, '/matchmaking');

        export function create(id: number) {
            return invoke(base, '/create', [id]);
        }

        export function matchmake() {
            return invoke(base, '/matchmake', []);
        }

        export function selectRoles(one: string, two: string) {
            return invoke(base, '/selectRoles', [one, two]);
        }
    }

    export namespace MatchMaking {
        const base = '/playloop/matchmaking';

        export const state = asyncValue<Domain.Game.MatchmakingState>(base, '/state');
        export const lobby = asyncValue<void>(base, '/lobby');
        export const champselect = asyncValue<void>(base, '/champselect');

        export function cancel() {
            return invoke(base, '/cancel', []);
        }

        export function afkCheck(accept: boolean) {
            return invoke(base, '/afkcheck', [accept]);
        }
    }
}

export namespace MatchHistory {
    const base = '/profile/matches';

    export const match = async<Domain.MatchHistory.MatchDetails>(base, '/new');

    export function history(account: number) {
        return invoke<Domain.MatchHistory.PlayerHistory>(base, '/history', [account]);
    }

    export function deltas() {
        return invoke<Domain.MatchHistory.PlayerDeltas>(base, '/deltas', []);
    }

    export function details(gameid) {
        return invoke<Domain.MatchHistory.MatchDetails>(base, '/details', [gameid]);
    }
}

export namespace Summoner {
    const base = '/summoner';

    export const me = asyncValue<any>(base, '/me');

    export function get(name: string) {
        return invoke<Domain.Summoner.SummonerSummary>(base, '/get', [name]);
    }

    export function details(account: number) {
        return invoke<Domain.Summoner.SummonerDetails>(base, '/details', [account]);
    }

    export function kudos(id: number) {
        return invoke<Domain.Summoner.SummonerKudos>(base, '/kudos', [id]);
    }

    export function icon(ids: number[]) {
        return invoke<{ [id: number]: number }>(base, '/icon', [ids]);
    }

    export function store() {
        return invoke<string>(base, '/store', []);
    }

}