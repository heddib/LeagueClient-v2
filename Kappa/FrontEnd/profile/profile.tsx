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
            <div class="tab-button" data-ref="tab2"><span>RANKED</span></div>
        </div>
        <div class="center" data-ref="mainScroller">
            <container data-ref="overviewContainer"></container>
            <container data-ref="matchesContainer"></container>
            <container data-ref="rankedContainer"></container>
        </div>
    </module>
);

export default class ProfilePage extends Module {
    private tabChange: (index: number) => void;

    public constructor() {
        super(template);

        let tabs = []
        for (var i = 0; i < 3; i++) tabs[i] = this.refs['tab' + i];
        this.tabChange = Tabs.create(tabs, 0, (old, now) => this.onTabChange(old, now));

        let champs = new OverviewPage();
        champs.render(this.refs.overviewContainer);

        let hextech = new MatchesPage();
        hextech.render(this.refs.matchesContainer);

        Summoner.me.single(me => {
            Champions.mastery(me.summonerId).then(list => {
                this.refs.background.setBackgroundImage(Assets.splash.centered(list[0].championId, 0));
            });
        });
    }

    private onTabChange(old: number, now: number) {
        this.refs.background.setClass(now > 0, 'blurred');

        let ratio = now / this.refs.mainScroller.children.length;
        this.refs.mainScroller.css('transform', 'translateX(' + (-ratio * 100) + '%)');
    }
}