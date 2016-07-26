import Module        from './../../ui/module';
import * as Assets   from './../../../frontend/assets';
import * as Summoner from './../../../frontend/summoner';

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

interface Props {
    summ: Domain.Summoner.SummonerSummary;
    game: Domain.MatchHistory.MatchDetails;
    delta: Domain.MatchHistory.GameDeltaInfo;
    onSelected: () => void;
}

interface Refs {
    spell1: Swish;
    spell2: Swish;
    card: Swish;
    result: Swish;
    kda: Swish;
    minions: Swish;
    gold: Swish;
    type: Swish;

    ip: Swish;
    map: Swish;
    date: Swish;
}

export default class MatchSummary extends React.Component<Props, {}> {
    constructor(props: Props) {
        super(props);
    }

    render() {
        var ident = this.props.game.participantIdentities.first(p => p.player.accountId == this.props.summ.accountId);
        var part = this.props.game.participants.first(p => p.participantId == ident.participantId);

        var result = part.stats.win ? 'Victory' : 'Defeat';
        var ip;
        if (this.props.delta) {
            ip = (
                <div class="ip">
                    <span>{ this.props.delta.platformDelta.ipDelta }</span>
                    <span> IP</span>
                </div>
            );
        }
        var mode = { 'ARAM': 'ARAM', 'CLASSIC': 'Classic' }[this.props.game.gameMode];
        var date = new Date(this.props.game.gameCreation + this.props.game.gameDuration);
        var map = Assets.gamedata.maps.first(m => m.id == this.props.game.mapId);
        var items = [];
        for (var i = 0; i < 7; i++) {
            if (part.stats['item' + i])
                items[i] = <img src={ Assets.items.icon(part.stats['item' + i]) }/>
            else
                items[i] = <img class="hidden"/>
        }

        return (
            <module class="matchsummary" onClick={ s => this.props.onSelected() }>
                <div class="card" style={ `background-image: url("${Assets.champion.splash(part.championId, 0)}")` }>
                    <div class="spells">
                        <img src={ Assets.summoner.spell(part.spell1Id) }/>
                        <img src={ Assets.summoner.spell(part.spell2Id) }/>
                    </div>
                    <div class="text">
                        <span >{ mode }</span>
                        <span class={ 'result ' + result.toLowerCase() }>{ result }</span>
                    </div>
                </div>
                <div class="center">
                    <x-flexpadd/>
                    <div class="items">{ items }</div>
                    <x-flexpadd/>
                    <div class="stats">
                        <span class="kda">{ `${part.stats.kills} / ${part.stats.deaths} / ${part.stats.assists}` }</span>
                        <div class="minions">
                            <span>{ part.stats.totalMinionsKilled }</span>
                            <span class="icon"></span>
                        </div>
                        <div class="gold">
                            <span>{ (part.stats.goldEarned / 1000).toFixed(1) + 'k' }</span>
                            <span class="icon"></span>
                        </div>
                    </div>
                    <x-flexpadd/>
                </div>
                <x-flexpadd/>
                <div class="info">
                    { ip }
                    <div class="map">
                        <span>{ map.name }</span>
                    </div>
                    <div class="date">
                        <span>{ `${MonthNames[date.getMonth()]} ${date.getDate()}, ${date.getFullYear()}` }</span>
                    </div>
                </div>
            </module>
        );
    }
}
