import { Swish }      from './../ui/swish';
import Module         from './../ui/module';
import * as Tabs      from './../ui/tabs';
import * as Assets    from './../assets/assets';
import * as Summoner  from './../summoner/summoner';
import * as Champions from './../collection/champions/service';

import OverviewPage   from './overview';
import MatchesPage    from './matches/history';

var template = (
    <module class="profile">
        <div class="background" data-ref="background"/>
        <div class="header">
            <div class="tab-button" data-ref="tab0" id><span>OVERVIEW</span></div>
            <div class="tab-button" data-ref="tab1" id><span>MATCH HISTORY</span></div>
            <div class="tab-button" data-ref="tab2"><span>RANKINGS</span></div>
        </div>
        <div class="center" data-ref="mainScroller">
            <container data-ref="overviewContainer"/>
            <container data-ref="matchesContainer"/>
            <container data-ref="rankedContainer"/>
        </div>
    </module>
);

export default class ProfilePage extends Module {
    private tabChange: (index: number) => void;

    private overview: OverviewPage;
    private matches: MatchesPage;

    public constructor() {
        super(template);

        let tabs = []
        for (var i = 0; i < 3; i++) tabs[i] = this.refs['tab' + i];
        this.tabChange = Tabs.create(tabs, 0, (old, now) => this.onTabChange(old, now));

        Summoner.me.single(me => {
            Summoner.get(me.name).then(s => this.load(s));
        });
    }

    private load(summ: Domain.Summoner.SummonerSummary) {
        if (this.overview) this.overview.dispose();
        if (this.matches) this.matches.dispose();

        this.refs.overviewContainer.empty();
        this.refs.matchesContainer.empty();

        this.overview = new OverviewPage(summ);
        this.overview.render(this.refs.overviewContainer);

        this.matches = new MatchesPage(summ);
        this.matches.render(this.refs.matchesContainer);

        Champions.mastery(summ.summonerId).then(list => {
            this.refs.background.setBackgroundImage(Assets.champion.splash(list[0].championId, 0));
        });
    }

    private onTabChange(old: number, now: number) {
        this.refs.background.setClass(now > 0, 'blurred');

        let ratio = now / this.refs.mainScroller.children.length;
        this.refs.mainScroller.css('transform', 'translateX(' + (-ratio * 100) + '%)');
    }
}