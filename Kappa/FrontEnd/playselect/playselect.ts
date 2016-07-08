import { Swish }     from './../ui/swish';
import Module        from './../ui/module';
import * as PlayLoop from './../playloop/playloop';
import * as Custom   from './../playloop/custom/service';
import * as Lobby    from './../playloop/lobby/service';

const html = Module.import('playselect');

interface Group {
    name: string;
    image: string;
    queues: Queue[]
}

interface Queue {
    id: number;
    name: string;
    action: (queue: Queue) => void;
}

function Group(name: string, image: string, queues: Queue[]): Group {
    return { name: name, image: image, queues: queues };
}

function Queue(id: number, name: string, action: (queue: Queue) => void): Queue {
    return { id: id, name: name, action: action };
}

function Single(id: number, name: string, image: string, action: (queue: Queue) => void): Group {
    return Group(name, image, [Queue(id, name, action)]);
}

const maps = (() => {
    const mapBase = "images/playselect/";
    return {
        abyss: mapBase + "HowlingAbyss.png",
        rift: mapBase + "SummonersRift.png",
        tt: mapBase + "TwistedTreeline.png",

        urf: mapBase + "urf.png",
        oneforall: mapBase + "oneforall.png",
        hexakill: mapBase + "hexakill.png",
        hexakilltt: mapBase + "hexakilltt.png",
        ascension: mapBase + "ascension.png",
        poroking: mapBase + "poroking.png",
        dominion: mapBase + "dominion.png",
    };
})();

for (var key in maps) {
    Util.preload(maps[key]);
}

export default class PlaySelect extends Module {
    private static loaded: boolean = false;
    private standard: Group[];
    private extra: Group[];

    public select = this.create<any>();
    public custom = this.create<any>();

    public constructor() {
        super(html);

        this.standard = [
            Group("Summoner's Rift", maps.rift, [
                Queue(410, 'Ranked', this.TeambuilderDraft),
                Queue(400, 'Normal', this.TeambuilderDraft),
                Queue(2, 'Normal (Blind Pick)', this.Standard),
            ]),
            Group("Twisted Treeline", maps.tt, [
                Queue(41, 'Ranked Teams - 3v3', this.RankedTeams),
                Queue(8, 'Normal Blind Pick - 3v3', this.Standard),
            ]),
            Group("Howling Abyss", maps.abyss, [
                Queue(65, 'ARAM', this.Standard),
            ]),
        ];

        this.extra = [
            Single(76, "URF", maps.urf, this.Standard),
            Single(70, "One for All", maps.oneforall, this.Standard),
            Single(75, "Hexakill", maps.hexakill, this.Standard),
            Single(98, "Twisted Treeline Hexakill", maps.hexakilltt, this.Standard),
            Single(96, "Ascension", maps.ascension, this.Standard),
            Single(300, "Poro King", maps.poroking, this.Standard),
            Single(317, "Definitely not Dominion", maps.dominion, this.Standard),
        ];
        
        this.refs.body.css('display', null);
        this.refs.loadingModal.css('display', 'none');

        PlayLoop.queues().then(qs => this.start(qs));
    }

    public start(queues: any[]) {
        var map: { [id: number]: any } = {};
        queues.forEach(q => map[q.id] = q);
        var load = (src) => {
            for (var g = 0; g < src.length; g++) {
                var group = src[g];
                for (var q = 0; q < group.queues.length; q++) {
                    var i = map[group.queues[q].id];
                    if (!i) {
                        group.queues.splice(q, 1);
                        q--;
                    } else {
                        delete map[i.id];
                    }
                }
            }
        };

        load(this.standard);
        load(this.extra);

        if (!PlaySelect.loaded) {
            PlaySelect.loaded = true;

            for (var id in map) {
                console.log('Not implemented: ' + map[id].name + ' (' + id + ')');
            }
        }

        var max = this.standard.length;
        var unit = 300;
        var total = 1000;

        var load2 = (src: Group[], title: Swish, dst: Swish) => {
            var margin = (1000 - (max * 300)) / (max + 1);
            dst.css('padding-right', margin + 'px');
            title.css('padding-left', margin + 'px');
            dst.empty();

            for (var g = 0; g < src.length; g++) {
                let group = src[g];
                if (group.queues.length == 0) continue;
                let node = this.template('queue-group', { image: group.image });
                dst.add(node);
                node.css('margin-left', margin + 'px');
                let list = node.$('.list');
                for (let q = 0; q < group.queues.length; q++) {
                    let info = group.queues[q];
                    let sub = this.template('queue', { name: info.name });
                    sub.on('mouseup', e => info.action.bind(this)(info));
                    list.add(sub);
                }
            }

            if (dst.children.length == 0) {
                dst.css('display', 'none');
                title.css('display', 'none');
            }
        };

        load2(this.standard, this.refs.standardTitle, this.refs.standardQueues);
        load2(this.extra, this.refs.extraTitle, this.refs.extraQueues);
    }

    private TeambuilderDraft(queue: Queue) {
        this.Standard(queue);
    }

    private Standard(queue: Queue) {
        this.load(Lobby.create(queue.id)).then(() => {
            this.dispatch(this.select, {});
        })
    }

    private RankedTeams(queue: Queue) {
        debugger;
    }

    private onCreateCustomClick() {
        this.load(Custom.create()).then(() => {
            this.dispatch(this.custom, {});
        })
    }

    private onJoinCustomClick() {
    }

    private load<T>(async: Async<T>) {
        return new Async<T>((resolve, reject) => {

            this.refs.body.css('display', 'none');
            this.refs.loadingModal.css('display', null);

            async.then(v => {
                resolve(v);
            }).catch(e => {
                this.refs.body.css('display', null);
                this.refs.loadingModal.css('display', 'none');
            });
        });
    }
}
