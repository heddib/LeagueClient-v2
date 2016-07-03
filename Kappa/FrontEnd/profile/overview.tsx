import { Swish }      from './../ui/swish';
import Module         from './../ui/module';

import * as Assets    from './../assets/assets';
import * as Summoner  from './../summoner/summoner';
import * as Champions from './../collection/champions/service';


var template = (
    <module class="profile-overview">
        <x-flexpadd/>
        <div class="profile shadow">
            <div class="icon">
                <img data-ref="summonerIcon"/>
            </div>
            <div class="text">
                <div class="name">
                    <span data-ref="name"></span>
                </div>
                <div class="stats">
                    <div class="kudos friendlies">
                        <span class="icon"/>
                        <span data-ref="friendlies"/>
                    </div>
                    <div class="kudos helpfuls">
                        <span class="icon"/>
                        <span data-ref="helpfuls"/>
                    </div>
                    <div class="kudos teamworks">
                        <span class="icon"/>
                        <span data-ref="teamworks"/>
                    </div>
                    <div class="kudos honorables">
                        <span class="icon"/>
                        <span data-ref="honorables"/>
                    </div>
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
            this.refs.summonerIcon.src = Assets.summoner.icon(me.icon);
            this.refs.name.text = me.name;

            Champions.mastery(me.summonerId).then(list => {
                for (let mastery of list) {
                    let champ = Assets.gamedata.champions.first(c => c.id == mastery.championId);
                }
            });

            Summoner.kudos(me.summonerId).then(kudos => {
                this.refs.friendlies.text = kudos.friendlies;
                this.refs.helpfuls.text = kudos.helpfuls;
                this.refs.teamworks.text = kudos.teamworks;
                this.refs.honorables.text = kudos.honorables;
            });
        });
    }
}