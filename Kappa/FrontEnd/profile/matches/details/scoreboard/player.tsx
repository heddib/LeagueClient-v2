import { Swish }     from './../../../../ui/swish';
import Module        from './../../../../ui/module';
import * as Assets   from './../../../../assets/assets';
import * as Summoner from './../../../../summoner/summoner';

const MapNames = {
    1: "Old Summoner's Rift",
    8: "Crystal Scar",
    9: "WIP Update",
    10: "Twisted Treeline",
    11: "Summoner's Rift",
    12: "Proving Grounds"
};

const MonthNames = [
    "January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];

var template = (
    <module class="matchdetailsplayer">
        <div class="spells">
            <img data-ref="spell1"/>
            <img data-ref="spell2"/>
        </div>
        <div class="level">
            <span data-ref="level"></span>
        </div>
        <div class="champ" data-ref="champ"></div>
        <div class="name">
            <span data-ref="name"></span>
        </div>
        <div class="items">
            <img data-ref="item0"/>
            <img data-ref="item1"/>
            <img data-ref="item2"/>
            <img data-ref="item3"/>
            <img data-ref="item4"/>
            <img data-ref="item5"/>
            <img data-ref="item6"/>
        </div>
        <div class="kda">
            <span data-ref="kda"></span>
        </div>
        <div class="minions">
            <span data-ref="minions"></span>
            <span class="icon"></span>
        </div>
        <div class="gold">
            <span data-ref="gold"></span>
            <span class="icon"></span>
        </div>
    </module>
);

export default class Player extends Module {
    public selected = this.create<any>();

    public constructor(src: MatchHistory.Participant, ident: MatchHistory.ParticipantIdentity) {
        super(template);

        Summoner.me.single(me => {
            if (ident.player.accountId == me.accountId) this.node.addClass('me');
        });

        this.refs.spell1.src = Assets.image('spell', src.spell1Id);
        this.refs.spell2.src = Assets.image('spell', src.spell2Id);
        this.refs.champ.css('background-image', 'url("' + Assets.image('champ', src.championId) + '")');
        this.refs.level.text = src.stats.champLevel;
        this.refs.name.text = ident.player.summonerName;

        this.refs.kda.text = `${src.stats.kills} / ${src.stats.deaths} / ${src.stats.assists}`;
        this.refs.minions.text = src.stats.totalMinionsKilled;
        this.refs.gold.text = (src.stats.goldEarned / 1000).toFixed(1) + 'k';

        for (var i = 0; i < 7; i++) {
            if (src.stats['item' + i])
                this.refs['item' + i].src = Assets.image('item', src.stats['item' + i]);
            else
                this.refs['item' + i].addClass('hidden')
        }

        this.node.on('click', e => this.dispatch(this.selected, {}));
    }
}