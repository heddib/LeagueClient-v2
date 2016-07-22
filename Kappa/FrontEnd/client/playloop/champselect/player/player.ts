import Module         from './../../../ui/module';

import * as Assets    from './../../../../frontend/assets';

const html = Module.import('playloop/champselect/player');

interface Refs {
    name: Swish;
    spell1: Swish;
    spell2: Swish;
    role: Swish;
    champion: Swish;
    champImage: Swish;
}

export default class ChampSelectPlayer extends Module<Refs> {
    private last: Domain.Game.GameMember;

    public trade = this.create<{}>();
    public cancel = this.create<{}>();
    public respond = this.create<boolean>();

    public constructor() {
        super(html);
    }

    private onTradeClick() {
        this.dispatch(this.trade, {});
    }

    private onCancel() {
        this.dispatch(this.cancel, {});
    }

    private onAccept() {
        this.dispatch(this.respond, true);
    }

    private onDecline() {
        this.dispatch(this.respond, false);
    }

    public update(player: Domain.Game.GameMember, index: number, enemy: boolean) {
        this.refs.name.text = player.name || 'Summoner ' + (index + 1);

        this.node.setClass(!player.spell1, 'no-spells');
        if (player.spell1) {
            this.refs.spell1.src = Assets.summoner.spell(player.spell1);
            this.refs.spell2.src = Assets.summoner.spell(player.spell2);
        }

        this.node.setClass(!player.role || player.role == 'UNSELECTED', 'no-role')
        if (player.role && player.role != 'UNSELECTED') {
            this.refs.role.addClass('role-' + player.role);
        }

        this.node.setClass(!player.champion, 'no-champ');
        if (player.champion) {
            let champ = Assets.gamedata.champions.first(c => c.id == player.champion);

            this.refs.champion.text = champ.name;

            if (!this.last || this.last.champion != player.champion) {
                let url = Assets.champion.splash(player.champion, 0);
                this.refs.champImage.css('background-image', 'url("' + url + '")');
            }
        }

        this.node.setClass(player.active, 'active');
        this.node.setClass(player.intent, 'intent');
        this.node.setClass(enemy, 'reverse');

        ['invalid', 'cancelled', 'possible', 'sent', 'received', 'declined'].forEach(s => this.node.removeClass('trade-' + s));
        this.node.addClass('trade-' + player.trade.toLowerCase());

        this.last = player;

        if (player.reroll) {
            console.info(`${player.name}: ${player.reroll.points}`);
        }
    }
}