import { Swish }     from './../../ui/swish';
import Module        from './../../ui/module';
import * as Assets   from './../../assets/assets';
import * as Summoner from './../../summoner/summoner';
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

    public constructor(summ: Domain.Summoner.SummonerSummary) {
        super(template);

        this.subscribe(Service.match, match => {
            console.log(match.gameId);
            this.history.games.games.unshift(match);
            this.update();
            this.details(match);
        });

        Service.history(summ.accountId).then(history => {
            this.history = history;
            this.update();
        }).catch(error => {
            let msg = <span class="service-error-msg">An error has occured</span>;
            this.refs.list.add(msg);
        });

        Summoner.me.single(m => {
            if (m.accountId != summ.accountId) return;

            Service.deltas().then(deltas => {
                this.deltas = deltas;
                if (this.history) this.update();
            });
        })
    }

    private update() {
        this.history.games.games.sort((a, b) => b.gameCreation - a.gameCreation);

        for (let match of this.history.games.games) {
            let delta = this.deltas ? this.deltas.deltas.first(d => d.gameId == match.gameId) : null;
            let summary = new MatchSummary(match, delta);
            summary.selected.on(() => {
                Service.details(match.gameId).then(details => this.details(details));
            });
            summary.render(this.refs.list);
        }
    }

    private details(details: Domain.MatchHistory.MatchDetails) {
        var page = new MatchDetails(details);
        this.refs.detailsPopup.empty();
        this.refs.detailsPopup.addClass('shown');
        page.render(this.refs.detailsPopup);
        page.closed.on(() => this.refs.detailsPopup.removeClass('shown'));
    }
}