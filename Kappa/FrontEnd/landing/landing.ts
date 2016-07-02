import * as Kappa     from './../kappa';
import { Swish, $ }   from './../ui/swish';
import Module         from './../ui/module';
import * as Tabs      from './../ui/tabs';

import * as Assets    from './../assets/assets';
import * as Summoner  from './../summoner/summoner';

import * as Meta      from './../meta/meta';

import ChatList       from './../chat/list/chatlist';

import Lobby          from './../playloop/lobby/lobby';
import Custom         from './../playloop/custom/custom';
import ChampSelect    from './../playloop/champselect/champselect';
import InGame         from './../playloop/ingame/ingame';

import * as Invite    from './../invite/invite';

import CollectionPage from './../collection/collection';
import PatcherPage    from './../patcher/game';
import PlaySelect     from './../playselect/playselect';
import ProfilePage    from './../profile/profile';


declare var Tetris;

const html = Module.import('landing');
const back = 'images/landing_background.png';

Util.preload(back);

export default class Landing extends Module {
    private loadedShop: boolean;
    private chatlist: ChatList;
    private patcher: PatcherPage;
    private tabChange: (index: number) => void;

    public constructor(accountState) {
        super(html);

        Invite.update.on(e => this.drawInvites(e));
        this.drawInvites(Invite.list());

        this.refs.background.src = back;

        let tabs = []
        for (var i = 0; i < 5; i++) tabs[i] = this.refs['tab' + i];
        this.tabChange = Tabs.create(tabs, 1, (old, now) => this.onTabChange(old, now));

        // Store tab
        this.refs.tab4.on('click', e => {
            if (!this.loadedShop) {
                Summoner.store().then(url => this.$('#shop-frame').src = url);
                this.loadedShop = true;
            }
        });
        this.$('#alerts-profile').on('click', e => Summoner.store().then(url => Meta.link(url)));

        this.subscribe(Summoner.me, this.onMe);

        if (accountState.inGame) {
            this.ingame();
        } else {
            PatcherPage.required().then(b => {
                if (b) {
                    this.patcher = new PatcherPage();
                    this.subscribe(this.patcher.complete, this.onPatched);
                    this.showModule(this.patcher, false);
                } else {
                    this.onPatched(true);
                }
            });
        }

        this.chatlist = new ChatList();
        this.chatlist.render(this.$('#friends-area'));

        let collection = new CollectionPage();
        collection.render(this.$('#landing-collection'));

        let profile = new ProfilePage();
        profile.render(this.$('#landing-profile'));
    }

    public dispose() {
        super.dispose();
    }

    private onPatched(force?: boolean) {
        if (!force && !this.patcher) return;
        this.patcher = null;
        this.playSelect();
    }

    private onMe(me) {
        this.$('#ip-balance').text = me.ip;
        this.$('#rp-balance').text = me.rp;
    }

    private drawInvites(list) {
        this.$('#invite-list').empty();
        for (let invite of list) {
            var control = new Invite.Control(invite);
            control.custom.on(e => this.custom());
            control.lobby.on(e => this.lobby());
            control.render(this.$('#invite-list'));
        }
    }

    private onTabChange(old: number, now: number) {
        this.refs.background.removeClass('tab-' + old);
        this.refs.background.addClass('tab-' + now);

        this.refs.mainScroller.addClass('faded');
        setTimeout(() => {
            this.refs.mainScroller.removeClass('faded');
            this.refs.mainScroller.css('left', -100 * now + '%');
        }, 150);
    }

    private playSelect() {
        var select = new PlaySelect();
        select.select.on(e => this.lobby());
        select.custom.on(e => this.custom());
        this.showModule(select, false)
        this.$('#play-button > span').text = 'PLAY';
    }

    private lobby() {
        var mod = new Lobby(this.chatlist);
        mod.start.on(e => this.champselect());
        this.showModule(mod, true);
        this.$('#play-button > span').text = 'LOBBY';
    }

    private custom() {
        var mod = new Custom(this.chatlist);
        mod.start.on(e => this.champselect());
        this.showModule(mod, true);
        this.$('#play-button > span').text = 'CUSTOM';
    }

    private champselect() {
        var mod = new ChampSelect();
        mod.start.on(() => this.ingame());
        mod.cancel.on(e => this.lobby());
        mod.custom.on(e => this.custom());
        this.showModule(mod, true);
        this.$('#play-button > span').text = 'GAME';
    }

    private ingame() {
        var mod = new InGame();
        this.$('#play-button > span').text = 'GAME';
        this.showModule(mod, true);
    }

    private module: Module;
    private showModule(mod: Module, show: boolean) {
        let fadein = () => {
            this.$('#landing-content').empty();
            this.module = mod;
            this.module.render(this.$('#landing-content'));
            setTimeout(() => {
                this.module.node.removeClass('faded-in');
                this.module.render(this.$('#landing-content'));
            }, 0);
        };

        mod.node.addClass('faded-in');

        if (this.module) {
            this.module.dispose();
            this.module.node.addClass('faded-out');
            let done = false;
            this.module.node.on('transitionend', () => {
                if (!done) fadein();
                done = true;
            });
        } else {
            fadein();
        }

        mod.close.on(e => this.playSelect());

        if (show) this.tabChange(0);
    }
}