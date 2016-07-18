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

interface Refs {
    list: Swish;
    detailsPopup: Swish;
}

export default class MatchHistory extends Module<Refs> {
    private history: Domain.MatchHistory.PlayerHistory;
    private deltas: Domain.MatchHistory.PlayerDeltas;
    private summ: Domain.Summoner.SummonerSummary;

    public constructor(summ: Domain.Summoner.SummonerSummary) {
        super(template);
        this.summ = summ;

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
            let summary = new MatchSummary(this.summ, match, delta);
            summary.selected.on(() => {
                let task = Service.details(match.gameId);
                task.then(dets => this.details(dets));
            });
            summary.render(this.refs.list);
        }
    }

    private details(details: Domain.MatchHistory.MatchDetails) {
        var page = new MatchDetails(this.summ, details);
        this.refs.detailsPopup.empty();
        this.refs.detailsPopup.addClass('shown');
        page.render(this.refs.detailsPopup);
        page.closed.on(() => this.refs.detailsPopup.removeClass('shown'));
    }
}