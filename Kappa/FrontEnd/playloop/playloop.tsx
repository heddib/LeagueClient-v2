import { Swish }          from './../ui/swish';
import Module             from './../ui/module';
import * as Assets        from './../assets/assets';

import * as CustomService from './custom/service';
import * as LobbyService  from './lobby/service';
import * as Service       from './service';

import Lobby              from './lobby/lobby';
import Custom             from './custom/custom';
import ChampSelect        from './champselect/champselect';
import InGame             from './ingame/ingame';

let queueCache;

const template = (
    <module class="playloop">
        <div data-ref="body" class="main-content">
            <div class="playselect-map" data-ref="rift">
            </div>
            <div class="playselect-map" data-ref="treeline">
            </div>
            <div class="playselect-map" data-ref="abyss">
            </div>
            <container class="container" data-ref="container"/>
        </div>
    </module>
);

const titleTemplate = (
    <span class="map-title"/>
);

const queueTemplate = (
    <module class="playselect-queue">
        <div class="title">
            <span data-ref="title"/>
        </div>
    </module>
);

interface IInviteProvider {
    start(): void;
    stop(): void;
}

export default class Page extends Module {
    private mapkeys = {
        10: 'treeline',
        11: 'rift',
        12: 'abyss',
    };
    private provider: IInviteProvider;
    private queues: Domain.Game.AvailableQueue[];

    public state = this.create<string>();

    constructor(provider: IInviteProvider) {
        super(template);

        this.provider = provider;

        let names = {
            2: 'Blind Pick', // Rift
            8: 'Blind Pick', // TT
            // 41: 'Ranked Teams', // TT
            65: 'ARAM', // Abyss
            400: 'Normal', // Rift
            410: 'Ranked', // Rift
        };

        let featureds = {
            70: 'One for All',
            75: 'Hexakill',
            76: 'URF',
            96: 'Ascension',
            98: 'Twisted Treeline Hexakill',
            300: 'Legend of the Poro King',
            317: 'Definitely not Dominion'
        };

        for (var id in this.mapkeys) {
            this.refs[this.mapkeys[id]].setBackgroundImage(`images/playselect/tmp/${this.mapkeys[id]}-back.jpg`);
            let map = Assets.gamedata.maps.first(m => m.id == +id);
            let title = Module.create(titleTemplate);
            title.node.text = map.name;
            title.render(this.refs[this.mapkeys[id]]);
        }

        this.refs.rift.setBackgroundImage('images/playselect/tmp/rift-back.jpg');
        this.refs.abyss.setBackgroundImage('images/playselect/tmp/abyss-back.jpg');
        this.refs.treeline.setBackgroundImage('images/playselect/tmp/treeline-back.jpg');

        this.refs.container.css('display', 'none');
        Service.getAvailableQueues().then(queues => {
            this.queues = queues;
            for (let queue of queues) {
                if (featureds[queue.id]) continue;
                if (!names[queue.id]) continue;

                let dst = this.refs[this.mapkeys[queue.map]];
                let mod = Module.create(queueTemplate);
                mod.refs.title.text = names[queue.id];
                mod.render(dst);
                mod.node.on('click', () => this.choose(queue.map, queue));
            }

            for (let id in this.mapkeys) {
                let dst = this.refs[this.mapkeys[id]];
                let mod = Module.create(queueTemplate);
                mod.refs.title.text = 'Custom';
                mod.render(dst);
                mod.node.on('click', () => this.choose(+id, null));
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
        let map = this.refs[this.mapkeys[selected]];
        map.addClass('selected');
        let index = Array.prototype.indexOf.call(map.parent[0].childNodes, map[0]);
        for (var id in this.mapkeys) {
            let node = this.refs[this.mapkeys[id]];
            if (Array.prototype.indexOf.call(node.parent[0].childNodes, node[0]) < index) {
                node.addClass('unselected');
            }
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

    private module: Module;
    private show(mod: Module) {
        if (this.module) this.module.dispose();

        this.refs.container.empty();
        mod.closed.on(() => this.onClose());
        mod.render(this.refs.container);
        this.refs.container.css('display', null);

        this.module = mod;
    }

    private onClose() {
        this.module.dispose()
        this.refs.container.css('display', 'none');
        this.refs.container.empty();
        for (var id in this.mapkeys) {
            let node = this.refs[this.mapkeys[id]];
            node.removeClass('selected', 'unselected');
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