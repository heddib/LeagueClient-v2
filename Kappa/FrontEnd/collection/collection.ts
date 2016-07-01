import { Swish }      from './../ui/swish';
import Module         from './../ui/module';
import * as Assets    from './../assets/assets';
import ChampionsPage  from './champions/page';
import HextechPage    from './hextech/hextech';

const html = Module.import('collection');

export default class CollectionPage extends Module {
    public constructor() {
        super(html);

        this.refs.hextechButton.on('click', e => this.tab(0));
        this.refs.championsButton.on('click', e => this.tab(1));
        this.refs.masteriesButton.on('click', e => this.tab(2));
        this.refs.runesButton.on('click', e => this.tab(3));
        this.tab(0);

        let champs = new ChampionsPage();
        champs.render(this.refs.championsContainer);

        let hextech = new HextechPage();
        hextech.render(this.refs.hextechContainer);
    }

    public sleep() {
        this.tab(0);
    }

    private tab(index: number) {
        let ratio = index / this.refs.mainScroller.children.length;
        this.refs.mainScroller.css('transform', 'translateX(' + (-ratio * 100) + '%)');
    }
}
