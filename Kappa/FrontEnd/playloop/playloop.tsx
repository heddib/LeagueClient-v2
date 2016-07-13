import { Swish }          from './../ui/swish';
import Module             from './../ui/module';
import * as Assets        from './../assets/assets';
import PatcherPage        from './../patcher/game';

import * as CustomService from './custom/service';
import * as LobbyService  from './lobby/service';
import * as Service       from './service';

import Lobby              from './lobby/lobby';
import Custom             from './custom/custom';
import ChampSelect        from './champselect/champselect';
import InGame             from './ingame/ingame';
import * as Invite        from './invite/panel';


import * as Kappa from './../kappa';

let queueCache;

const template = (
    <module class="playloop">
        <div data-ref="body" class="main-content">
            <container class="container" data-ref="container"/>
        </div>
    </module>
);

const mapTemplate = (
    <div class="playselect-map">
        <span data-ref="title" class="map-title"/>
    </div>
)

const queueTemplate = (
    <module class="playselect-queue">
        <div class="title">
            <span data-ref="title"/>
        </div>
    </module>
);

export const queueNames = {
    2: 'Blind Pick', // Rift
    8: 'Blind Pick', // TT
    // 41: 'Ranked Teams', // TT
    65: 'ARAM', // Abyss
    400: 'Normal', // Rift
    410: 'Ranked', // Rift
};

export const featuredNames = {
    70: 'One for All',
    75: 'Hexakill',
    76: 'URF',
    96: 'Ascension',
    98: 'Twisted Treeline Hexakill',
    300: 'Legend of the Poro King',
    // 317: 'Definitely not Dominion'
};

interface Refs {
    body: Swish;
    container: Swish;
}

export default class Page extends Module<Refs> {
    private maps: { [id: number]: Swish } = {};
    private mapsList = [
        { id: 11, key: 'rift' },
        { id: 10, key: 'treeline' },
        { id: 12, key: 'abyss' },
    ];

    private provider: Invite.IProvider;
    private patcher: PatcherPage;
    private queues: Domain.Game.AvailableQueue[];

    public state = this.create<string>();

    constructor(provider: Invite.IProvider) {
        super(template);

        this.provider = provider;

        for (var info of this.mapsList) {
            let map = Assets.gamedata.maps.first(m => m.id == info.id);
            let mod = Module.create(mapTemplate);
            mod.refs.title.text = map.name;
            mod.node.setBackgroundImage(`images/playselect/${info.key}-back.jpg`);
            this.maps[info.id] = mod.node;
            this.refs.body[0].insertBefore(mod.node[0], this.refs.body.children.last[0]);
        }

        this.refs.container.css('display', 'none');

        PatcherPage.required().then(b => {
            if (b) {
                for (let info of this.mapsList) {
                    this.maps[info.id].addClass('hidden');
                }

                this.patcher = new PatcherPage();
                this.subscribe(this.patcher.closed, this.onPatched);
                this.show(this.patcher);
            } else {
                this.onPatched(true);
            }
        });
    }

    private onPatched(force?: boolean) {
        if (!force && !this.patcher) return;
        this.patcher = null;

        Service.getAvailableQueues().then(queues => {
            this.queues = queues;
            for (let queue of queues) {
                let featured = featuredNames[queue.id];

                if (!queueNames[queue.id] && !featured) {
                    console.info(queue.name);
                    continue;
                }

                let dst = this.maps[queue.map];
                let mod = Module.create(queueTemplate);
                mod.refs.title.text = featured || queueNames[queue.id];
                mod.node.on('click', () => this.choose(queue.map, queue));
                mod.node.setClass(featured, 'featured');
                if (featured) {
                    dst.prepend(mod.node);
                } else {
                    dst.append(mod.node);
                }
            }

            for (let info of this.mapsList) {
                let mod = Module.create(queueTemplate);
                mod.refs.title.text = 'Custom';
                mod.render(this.maps[info.id]);
                mod.node.on('click', () => this.choose(info.id, null));
            }
        });
    }

    private choose(map: number, queue: Domain.Game.AvailableQueue) {
        this.map(map);

        let async: Async<any>;
        if (queue) {
            async = LobbyService.create(queue.id);
            async.then(() => this.lobby(false));
        } else {
            async = CustomService.create();
            async.then(() => this.custom());
        }
        async.catch(() => this.onClose());
    }

    public map(selected = 0) {
        if (!selected) {
            Service.current().then(c => {
                debugger;
                let queue = this.queues.first(q => q.id == c.queueId);
                this.map(queue.map);
            });
            return;
        }
        let map = this.maps[selected];
        map.addClass('selected');
        let index = Array.prototype.indexOf.call(map.parent[0].childNodes, map[0]);

        for (let info of this.mapsList) {
            if (info.id == selected) break;

            this.maps[info.id].addClass('unselected');
        }
    }

    public lobby(queue: boolean) {
        this.dispatch(this.state, 'LOBBY');

        let lobby = new Lobby(false, this.provider);
        lobby.start.on(() => this.champselect());

        this.show(lobby);
    }

    public custom() {
        this.dispatch(this.state, 'CUSTOM');

        let custom = new Custom(this.provider);
        custom.start.on(() => this.champselect());

        this.show(custom);
    }

    public champselect() {
        this.dispatch(this.state, 'GAME');

        let champselect = new ChampSelect();
        champselect.cancel.on(q => this.lobby(q));
        champselect.custom.on(() => this.custom());
        champselect.start.on(() => this.ingame());

        this.show(champselect);
    }

    public ingame() {
        this.dispatch(this.state, 'GAME');

        var ingame = new InGame();

        this.show(ingame);
    }

    private module: Module<any>;
    private show(mod: Module<any>) {
        if (this.module) this.module.dispose();

        this.refs.container.empty();
        mod.closed.on(() => this.onClose());
        mod.render(this.refs.container);
        this.refs.container.css('display', null);

        this.module = mod;
    }

    private onClose() {
        if (this.module)
            this.module.dispose()
        this.module = null;

        this.dispatch(this.state, 'PLAY');

        this.refs.container.css('display', 'none');
        this.refs.container.empty();

        for (let info of this.mapsList) {
            this.maps[info.id].removeClass('hidden', 'selected', 'unselected');
        }
    }
}

export function queues() {
    return new Async<Domain.Game.AvailableQueue[]>((resolve, reject) => {
        if (queueCache) resolve(queueCache);
        Service.getAvailableQueues().then(queues => {
            queueCache = queues;
            resolve(queues);
        });
    });
}

export function current() {
    return Service.current();
}

export function abandon() {
    return Service.abandon();
}

export function quit() {
    return Service.quit();
}