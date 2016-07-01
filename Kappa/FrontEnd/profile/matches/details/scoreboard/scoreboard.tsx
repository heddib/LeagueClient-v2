import { Swish }     from './../../../../ui/swish';
import Module        from './../../../../ui/module';

import Team          from './team';

var template = (
    <module class="scoreboard">
        <container class="blue-team" data-ref="blueTeam"></container>
        <container class="red-team" data-ref="redTeam"></container>
    </module>
);

export default class MatchScoreboard extends Module {
    public constructor(details: MatchHistory.MatchDetails) {
        super(template);

        let isBlue = (t: MatchHistory.Participant | MatchHistory.Team) => t.teamId == 100;
        let isRed = (t: MatchHistory.Participant | MatchHistory.Team) => t.teamId == 200;

        var blue = new Team(details.teams.first(isBlue), details.participants.where(isBlue), details.participantIdentities);
        var red = new Team(details.teams.first(isRed), details.participants.where(isRed), details.participantIdentities);

        blue.render(this.refs.blueTeam);
        red.render(this.refs.redTeam);
    }
}