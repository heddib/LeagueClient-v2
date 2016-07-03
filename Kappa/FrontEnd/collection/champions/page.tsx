import { Swish }      from './../../ui/swish';
import Module         from './../../ui/module';
import * as Assets    from './../../assets/assets';
import * as Service   from './service';

let template = (
    <module class="champions-page">
        <div class="left">
            <div class="search-form">
                <span>Search: </span>
                <input class="search-box" data-ref="search" type="text"/>
            </div>

            <x-flexpadd></x-flexpadd>

            <div class="view-settings">
            </div>
        </div>
        <div class="center">
            <div data-ref="champs" class="champs-grid"></div>
        </div>
    </module>
);

let icon = (
    <div class="champion-icon" style="background-image:url('{{iconurl}}')"></div>
);

export default class ChampionsPage extends Module {
    private champs: { [id: number]: Module } = {};

    public constructor() {
        super(template);

        this.refs.search.on('input', e => this.updateFilter());

        Service.inventory().then(all => {
            for (let champ of Assets.gamedata.champions) {
                let mod = Module.create(icon);
                mod.node.setBackgroundImage(Assets.champion.icon(champ.id));
                mod.render(this.refs.champs);
                this.champs[champ.id] = mod;
            }
        });
    }

    private updateFilter() {
        var search: RegExp;
        try {
            search = new RegExp(this.refs.search.value, 'i');
        } catch (e) {
            search = new RegExp('.*');
        }

        for (var id in this.champs) {
            let info = Assets.gamedata.champions.first(c => c.id == +id);
            var match = search.test(info.name);
            this.champs[id].node.setClass(!match, 'hidden');
        }
    }
}