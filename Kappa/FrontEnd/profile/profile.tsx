import { Swish }      from './../ui/swish';
import Module         from './../ui/module';
import * as Assets    from './../assets/assets';
import * as Summoner  from './../summoner/summoner';
import * as Champions from './../collection/champions/service';

import OverviewPage   from './overview';
import MatchesPage    from './matches/history';

var template = (
    <module class="profile">
        <div class="background" data-ref="background"/>
        <div class="header">
            <div class="tab-button" data-ref="overviewButton" id><span>OVERVIEW</span></div>
            <div class="tab-button" data-ref="matchesButton" id><span>MATCH HISTORY</span></div>
            <div class="tab-button" data-ref="rankedButton"><span>RANKED</span></div>
        </div>
        <div class="center" data-ref="mainScroller">
            <container data-ref="overviewContainer"></container>
            <container data-ref="matchesContainer"></container>
            <container data-ref="rankedContainer"></container>
        </div>
    </module>
);

export default class ProfilePage extends Module {
    public constructor() {
        super(template);

        this.refs.overviewButton.on('click', e => this.tab(0));
        this.refs.matchesButton.on('click', e => this.tab(1));
        this.refs.rankedButton.on('click', e => this.tab(2));

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

    private tab(index: number) {
        this.refs.background.setClass(index > 0, 'blurred');

        let ratio = index / this.refs.mainScroller.children.length;
        this.refs.mainScroller.css('transform', 'translateX(' + (-ratio * 100) + '%)');
    }
}