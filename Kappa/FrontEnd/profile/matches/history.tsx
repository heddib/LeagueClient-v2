import { Swish }     from './../../ui/swish';
import Module        from './../../ui/module';
import * as Assets   from './../../assets/assets';
import MatchDetails  from './details/match';
import MatchSummary  from './summary';

import * as Service  from './service';

var template = (
    <module class="matchhistory">
        <div class="center">
            <div class="title">
                <span>Recent Games</span>
            </div>
            <div class="list" data-ref="list"></div>
        </div>
        <container class="details-popup" data-ref="detailsPopup"></container>
    </module>
);

export default class MatchHistory extends Module {
    private history: Domain.MatchHistory.PlayerHistory;
    private deltas: Domain.MatchHistory.PlayerDeltas;

    public constructor() {
        super(template);

        Service.history().then(history => {
            this.history = history;
            this.check();
        }).catch(error => {
            let msg = <span class="service-error-msg">An error has occured</span>;
            this.refs.list.add(msg);
        });
        Service.deltas().then(deltas => {
            this.deltas = deltas;
            this.check();
        });
    }

    private check() {
        if (!this.history || !this.deltas) return;

        this.history.games.games.sort((a, b) => b.gameCreation - a.gameCreation);

        for (let match of this.history.games.games) {
            let delta = this.deltas.deltas.first(d => d.gameId == match.gameId);
            let summary = new MatchSummary(match, delta);
            summary.selected.on(() => this.details(match.gameId));
            summary.render(this.refs.list);
        }
    }

    private details(gameid: number) {
        Service.details(gameid).then(details => {
            var page = new MatchDetails(details);
            this.refs.detailsPopup.empty();
            this.refs.detailsPopup.addClass('shown');
            page.render(this.refs.detailsPopup);
            page.close.on(() => this.refs.detailsPopup.removeClass('shown'));
        });
    }
}