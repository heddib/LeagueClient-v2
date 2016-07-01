import { Swish }      from './../ui/swish';
import Module         from './../ui/module';

import * as Assets    from './../assets/assets';
import * as Summoner  from './../summoner/summoner';
import * as Champions from './../collection/champions/service';


var template = (
    <module class="profile-overview">
        <x-flexpadd/>
        <div class="profile">
            <div class="icon">
                <img data-ref="summonerIcon"/>
            </div>
            <div class="text">
                <div class="name">
                    <span data-ref="name"></span>
                </div>
                <div class="stats">
                </div>
            </div>
        </div>
        <x-flexpadd/>
        <x-flexpadd/>
        <x-flexpadd/>
    </module>
);

export default class OverviewPage extends Module {
    public constructor() {
        super(template);

        Summoner.me.single(me => {
            this.refs.summonerIcon.src = Assets.image('profile', me.icon);
            this.refs.name.text = me.name;

            Champions.mastery(me.summonerId).then(list => {
                for (let mastery of list) {
                    let key = Assets.ddragon.champs.keys[mastery.championId];
                    let champ = Assets.ddragon.champs.data[key];
                }
            });
        });
    }
}