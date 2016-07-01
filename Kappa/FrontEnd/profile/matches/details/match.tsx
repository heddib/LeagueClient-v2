import { Swish }     from './../../../ui/swish';
import Module        from './../../../ui/module';
import Scoreboard    from './scoreboard/scoreboard';
import Stats         from './stats/stats';

var template = (
    <module class="matchdetails">
        <div class="header" data-ref="navHeader">
            <div class="tab-button" data-ref="scoreboardButton" id><span>SCOREBOARD</span></div>
            <div class="tab-button" data-ref="overviewButton" id><span>OVERVIEW</span></div>
            <div class="tab-button" data-ref="statsButton"><span>STATS</span></div>
            <x-flexpadd></x-flexpadd>
            <div class="tab-button" data-ref="backButton"><span>BACK</span></div>
        </div>
        <div class="center" data-ref="mainScroller">
            <container data-ref="scoreboardContainer"></container>
            <container data-ref="overviewContainer"></container>
            <container data-ref="statsContainer"></container>
        </div>
    </module>
);

export default class MatchDetails extends Module {
    public constructor(details: MatchHistory.MatchDetails) {
        super(template);

        this.refs.scoreboardButton.on('click', e => this.tab(0));
        this.refs.overviewButton.on('click', e => this.tab(1));
        this.refs.statsButton.on('click', e => this.tab(2));

        this.refs.backButton.on('click', e => this.dispatch(this.close, {}));

        var scoreboard = new Scoreboard(details);
        scoreboard.render(this.refs.scoreboardContainer);

        var stats = new Stats(details);
        stats.render(this.refs.statsContainer);
    }

    private tab(index: number) {
        let ratio = index / this.refs.mainScroller.children.length;
        this.refs.mainScroller.css('transform', 'translateX(' + (-ratio * 100) + '%)');
    }
}