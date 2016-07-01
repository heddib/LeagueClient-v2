import { Swish }     from './../../ui/swish';
import Module        from './../../ui/module';
import * as Assets   from './../../assets/assets';
import * as Summoner from './../../summoner/summoner';

const MapNames = {
    1: "Old Summoner's Rift",
    8: "Crystal Scar",
    9: "WIP Update",
    10: "Twisted Treeline",
    11: "Summoner's Rift",
    12: "The Howling Abyss"
};

const MonthNames = [
    "January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];

var template = (
    <module class="matchsummary">
        <div class="card" data-ref="card">
            <div class="spells">
                <img data-ref="spell1"/>
                <img data-ref="spell2"/>
            </div>
            <div class="text">
                <span data-ref="type"></span>
                <span class="result" data-ref="result"></span>
            </div>
        </div>
        <div class="center">
            <x-flexpadd></x-flexpadd>
            <div class="items">
                <img data-ref="item0"/>
                <img data-ref="item1"/>
                <img data-ref="item2"/>
                <img data-ref="item3"/>
                <img data-ref="item4"/>
                <img data-ref="item5"/>
                <img data-ref="item6"/>
            </div>
            <x-flexpadd></x-flexpadd>
            <div class="stats">
                <span class="kda" data-ref="kda"></span>
                <div class="minions">
                    <span data-ref="minions"></span>
                    <span class="icon"></span>
                </div>
                <div class="gold">
                    <span data-ref="gold"></span>
                    <span class="icon"></span>
                </div>
            </div>
            <x-flexpadd></x-flexpadd>
        </div>
        <x-flexpadd></x-flexpadd>
        <div class="info">
            <div class="ip">
                <span data-ref="ip"></span>
                <span> IP</span>
            </div>
            <div class="map">
                <span data-ref="map"></span>
            </div>
            <div class="date">
                <span data-ref="date"></span>
            </div>
        </div>
    </module>
);

export default class MatchSummary extends Module {
    public selected = this.create<any>();

    public constructor(game: MatchHistory.MatchDetails, delta: MatchHistory.GameDeltaInfo) {
        super(template);

        Summoner.me.single(me => {
            var ident = game.participantIdentities.first(p => p.player.accountId == me.accountId);
            var part = game.participants.first(p => p.participantId == ident.participantId);

            this.refs.spell1.src = Assets.image('spell', part.spell1Id);
            this.refs.spell2.src = Assets.image('spell', part.spell2Id);
            this.refs.card.css('background-image', `url("${Assets.splash.centered(part.championId, 0)}")`);
            this.refs.result.text = part.stats.win ? 'Victory' : 'Defeat';
            this.refs.result.addClass(this.refs.result.text.toLowerCase());

            this.refs.kda.text = `${part.stats.kills} / ${part.stats.deaths} / ${part.stats.assists}`;
            this.refs.minions.text = part.stats.totalMinionsKilled;
            this.refs.gold.text = (part.stats.goldEarned / 1000).toFixed(1) + 'k';

            this.refs.ip.text = delta.platformDelta.ipDelta;

            switch (game.gameMode) {
                case 'ARAM':
                    this.refs.type.text = 'ARAM';
                    break;
                case 'CLASSIC':
                    this.refs.type.text = 'Classic';
                    break;
            }

            var date = new Date(game.gameCreation + game.gameDuration);
            this.refs.map.text = MapNames[game.mapId] || 'Error';
            this.refs.date.text = `${MonthNames[date.getMonth()]} ${date.getDate()}, ${date.getFullYear()}`

            for (var i = 0; i < 7; i++) {
                if (part.stats['item' + i])
                    this.refs['item' + i].src = Assets.image('item', part.stats['item' + i]);
                else
                    this.refs['item' + i].addClass('hidden')
            }

        });

        this.node.on('click', e => this.dispatch(this.selected, {}));
    }
}