import { Swish }     from './../../../../ui/swish';
import Module        from './../../../../ui/module';

var template = (
    <module class="matchstatgroup">
        <div class="header" data-ref="header">
            <span class="title" data-ref="title"></span>
        </div>
        <div class="list" data-ref="list"></div>
    </module>
);

var stat = (
    <div class="matchstatrow">
        <div class="label"><span data-ref="name"/></div>
        <div class="numbers" data-ref="numbers"></div>
    </div>
);

var number = (
    <div class="matchstat">
        <span data-ref="value"/>
    </div>
);

export default class MatchStatGroup extends Module {
    public constructor(name: string, stats: { [id: string]: string }, parts: Domain.MatchHistory.Participant[], myPartId: number) {
        super(template);

        let active = true;
        this.node.setClass(active, 'active');
        this.refs.header.on('click', () => this.node.setClass(active = !active, 'active'));

        this.refs.title.text = name;
        for (let id in stats) {
            let node = Module.create(stat);
            node.refs.name.text = stats[id];

            for (var part of parts) {
                let val;
                switch (id) {
                    case '_KDA':
                        val = `${part.stats.kills}/${part.stats.deaths}/${part.stats.assists}`;
                        break;
                    case '_FIRSTBLOOD':
                        val = part.stats.firstBloodKill ? 'X' : '';
                        break;
                    default:
                        val = part.stats[id];
                }
                let num = Module.create(number);
                num.refs.value.text = val;
                num.node.setClass(part.participantId == myPartId, 'me');
                num.render(node.refs.numbers);
            }

            node.render(this.refs.list);
        }
    }
}