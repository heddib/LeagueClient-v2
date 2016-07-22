import Module        from './../../../../ui/module';
import * as Assets   from './../../../../../frontend/assets';

import Player        from './player';

var template = (
    <module class="matchscoreboard-team">
        <div class="center">
            <div class="team-header">
                <div class="team-label"><span>Blue Team</span></div>
                <div class="team-name-spacer"></div>
                <div class="team-result">
                    <span data-ref="result"></span>
                </div>
                <div class="team-kda"><span data-ref="kda"></span></div>
                <div class="team-minions">
                    <span data-ref="minions"></span>
                    <span class="icon"></span>
                </div>
                <div class="team-gold">
                    <span data-ref="gold"></span>
                    <span class="icon"></span>
                </div>
            </div>
            <div class="team-list" data-ref="list"></div>
        </div>
        <div class="right">
            <div class="objective towers">
                <span data-ref="towers"></span>
                <span class="icon"></span>
            </div>
            <div class="objective inhibs">
                <span data-ref="inhibs"></span>
                <span class="icon"></span>
            </div>
            <div class="objective barons">
                <span data-ref="barons"></span>
                <span class="icon"></span>
            </div>
            <div class="objective dragons">
                <span data-ref="dragons"></span>
                <span class="icon"></span>
            </div>
        </div>
    </module >
);

const WinStrings = {
    'Win': 'Victory',
    'Fail': 'Defeat'
};

interface Refs {
    result: Swish;
    kda: Swish;
    minions: Swish;
    gold: Swish;
    towers: Swish;
    inhibs: Swish;
    barons: Swish;
    dragons: Swish;
    list: Swish;
}

export default class MatchScoreboardTeam extends Module<Refs> {
    public constructor(summ: Domain.Summoner.SummonerSummary, team: Domain.MatchHistory.Team, players: Domain.MatchHistory.Participant[], idents: Domain.MatchHistory.ParticipantIdentity[]) {
        super(template);

        this.refs.result.text = WinStrings[team.win];
        this.refs.kda.text = `${players.sum(p => p.stats.kills)} / ${players.sum(p => p.stats.deaths)} / ${players.sum(p => p.stats.kills)}`;
        this.refs.minions.text = players.sum(p => p.stats.totalMinionsKilled);
        this.refs.gold.text = (players.sum(p => p.stats.goldEarned) / 1000).toFixed(1);

        this.refs.towers.text = team.towerKills;
        this.refs.inhibs.text = team.inhibitorKills;
        this.refs.barons.text = team.baronKills;
        this.refs.dragons.text = team.dragonKills;

        for (var part of players) {
            var player = new Player(summ, part, idents.first(p => p.participantId == part.participantId));
            player.render(this.refs.list);
        }
    }
}