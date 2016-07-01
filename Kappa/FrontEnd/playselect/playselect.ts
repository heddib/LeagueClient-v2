import { Swish }     from './../ui/swish';
import Module        from './../ui/module';
import * as PlayLoop from './../playloop/playloop';
import * as Custom   from './../playloop/custom/service';
import * as Lobby    from './../playloop/lobby/service';

const html = Module.import('playselect');

class Group {
    private _name: string;
    private _image: string;
    private _queues: Queue[];

    public get name() { return this._name; }
    public get image() { return this._image; }
    public get queues() { return this._queues; }

    constructor(name: string, image: string, queues: Queue[]) {
        this._name = name;
        this._image = image;
        this._queues = queues;
    }
}

class Queue {
    private _id: number;
    private _name: string;
    private _action: Function;

    public get id() { return this._id; }
    public get name() { return this._name; }
    public get action() { return this._action; }

    constructor(id: number, name: string, action: Function) {
        this._id = id;
        this._name = name;
        this._action = action;
    }
}

export default class PlaySelect extends Module {
    private static loaded: boolean = false;
    private standard: Group[];
    private extra: Group[];

    public select = this.create<any>();
    public custom = this.create<any>();

    public constructor() {
        super(html);

        let maps = "images/playselect/";
        this.standard = [
            new Group("Summoner's Rift", maps + "SummonersRift.png", [
                new Queue(410, 'Ranked', this.TeambuilderDraft),
                new Queue(400, 'Normal', this.TeambuilderDraft),
                new Queue(2, 'Normal (Blind Pick)', this.Standard),
            ]),
            new Group("Twisted Treeline", maps + "TwistedTreeline.png", [
                new Queue(41, 'Ranked Teams - 3v3', this.RankedTeams),
                new Queue(8, 'Normal Blind Pick - 3v3', this.Standard),
            ]),
            new Group("Howling Abyss", maps + "HowlingAbyss.png", [
                new Queue(65, 'ARAM', this.Standard),
            ]),
        ];

        this.extra = [
            new Group("URF", maps + "urf.png", [
                new Queue(76, 'URF', this.Standard)
            ]),
            new Group("One for All", maps + "one for all.png", [
                new Queue(70, 'One for All', this.Standard)
            ]),
            new Group("Hexakill", maps + "hexakill.png", [
                new Queue(75, 'Hexakill', this.Standard)
            ]),
            new Group("Hexakill (Twisted Treeline)", maps + "hexakill tt.png", [
                new Queue(98, 'Hexakill (Twisted Treeline)', this.Standard)
            ]),
            new Group("Ascension", maps + "ascension.png", [
                new Queue(96, 'Ascension', this.Standard)
            ]),
            new Group("Poro King", maps + "poro king.png", [
                new Queue(300, 'Poro King', this.Standard)
            ]),
            new Group("URF", maps + "urf.png", [
                new Queue(76, 'URF', this.Standard)
            ]),
            new Group("Definitely not Dominion", maps + "dominion.png", [
                new Queue(317, 'Definitely not Dominion', this.Standard)
            ])
        ];

        this.showPage(this.refs.body);

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

    private showPage(node: Swish) {
        [this.refs.body, this.refs.loadingModal].forEach(q => q.css('display', 'none'));
        node.css('display', null);
    }

    private load<T>(async: Async<T>) {
        return new Async<T>((resolve, reject) => {
            this.showPage(this.refs.loadingModal);
            async.then(v => {
                resolve(v);
            }).catch(e => {
                this.showPage(this.refs.body);
            });
        });
    }
}
