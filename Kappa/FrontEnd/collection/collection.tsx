import { Swish }      from './../ui/swish';
import Module         from './../ui/module';
import * as Tabs      from './../ui/tabs';

import * as Assets    from './../assets/assets';
import ChampionsPage  from './champions/page';
import HextechPage    from './hextech/hextech';

const template = (
    <module class="collection">
        <div class="header">
            <div class="tab-button" data-ref="tab0" id><span>HEXTECH</span></div>
            <div class="tab-button" data-ref="tab1" id><span>CHAMPIONS</span></div>
            <div class="tab-button" data-ref="tab2"><span>MASTERIES</span></div>
            <div class="tab-button" data-ref="tab3"><span>RUNES</span></div>
        </div>
        <div class="center" data-ref="mainScroller">
            <container data-ref="hextechContainer"></container>
            <container data-ref="championsContainer"></container>
            <container data-ref="masteriesContainer"></container>
            <container data-ref="runesContainer"></container>
        </div>
    </module>
);

export default class CollectionPage extends Module {
    private tabChange: (index: number) => void;

    public constructor() {
        super(template);

        let tabs = []
        for (var i = 0; i < 4; i++) tabs[i] = this.refs['tab' + i];
        this.tabChange = Tabs.create(tabs, 0, (old, now) => this.onTabChange(old, now));

        let champs = new ChampionsPage();
        champs.render(this.refs.championsContainer);

        let hextech = new HextechPage();
        hextech.render(this.refs.hextechContainer);
    }

    public sleep() {
        this.tabChange(0);
    }

    private onTabChange(old: number, now: number) {
        let ratio = now / this.refs.mainScroller.children.length;
        this.refs.mainScroller.css('transform', 'translateX(' + (-ratio * 100) + '%)');
    }
}
