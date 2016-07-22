import Module        from './../../../../ui/module';

var template = (
    <module class="matchstatgroup">
        <div class="header" data-ref="header">
            <span class="title" data-ref="title"></span>
        </div>
        <div class="list" data-ref="list"></div>
    </module>
);


interface Refs {
    header: Swish;
    title: Swish;
    list: Swish;
}

export default class MatchStatGroup extends Module<Refs> {
    public constructor(name: string, stats: { [id: string]: string }, parts: Domain.MatchHistory.Participant[], myPartId: number) {
        super(template);

        let active = true;
        this.node.setClass(active, 'active');
        this.refs.header.on('click', () => this.node.setClass(active = !active, 'active'));

        this.refs.title.text = name;
        for (let id in stats) {
            let cols: Swish[] = [];

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
                let num = React.template(
                    <div class="matchstat">
                        <span>{ val }</span>
                    </div>
                );
                num.setClass(part.participantId == myPartId, 'me');
                cols.push(num);
            }

            this.refs.list.add(React.template(
                <div class="matchstatrow">
                    <div class="label">
                        <span data-ref="name">{ stats[id]}</span>
                    </div>
                    <div class="numbers">
                        { cols }
                    </div>
                </div>
            ));
        }
    }
}