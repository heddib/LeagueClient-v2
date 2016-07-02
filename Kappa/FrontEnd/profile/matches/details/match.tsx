import { Swish }     from './../../../ui/swish';
import Module        from './../../../ui/module';
import * as Tabs     from './../../../ui/tabs';

import Scoreboard    from './scoreboard/scoreboard';
import Stats         from './stats/stats';

var template = (
    <module class="matchdetails">
        <div class="header" data-ref="navHeader">
            <div class="tab-button" data-ref="tab0" id><span>SCOREBOARD</span></div>
            <div class="tab-button" data-ref="tab1" id><span>OVERVIEW</span></div>
            <div class="tab-button" data-ref="tab2"><span>STATS</span></div>
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
    private tabChange: (index: number) => void;

    public constructor(details: MatchHistory.MatchDetails) {
        super(template);

        let tabs = []
        for (var i = 0; i < 3; i++) tabs[i] = this.refs['tab' + i];
        this.tabChange = Tabs.create(tabs, 0, (old, now) => this.onTabChange(old, now));

        this.refs.backButton.on('click', e => this.dispatch(this.close, {}));

        var scoreboard = new Scoreboard(details);
        scoreboard.render(this.refs.scoreboardContainer);

        var stats = new Stats(details);
        stats.render(this.refs.statsContainer);
    }

    private onTabChange(old: number, now: number) {
        let ratio = now / this.refs.mainScroller.children.length;
        this.refs.mainScroller.css('transform', 'translateX(' + (-ratio * 100) + '%)');
    }
}