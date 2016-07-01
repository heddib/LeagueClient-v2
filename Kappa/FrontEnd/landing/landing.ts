import * as Kappa     from './../kappa';
import { Swish, $ }   from './../ui/swish';
import Module         from './../ui/module';

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

    public constructor(accountState) {
        super(html);

        Invite.update.on(e => this.drawInvites(e));
        this.drawInvites(Invite.list());

        this.refs.background.src = back;

        this.$('#play-button').on('mouseup', e => this.showTab(0));
        this.$('#home-button').on('click', e => this.showTab(1));
        this.$('#profile-button').on('click', e => this.showTab(2));
        this.$('#collection-button').on('click', e => this.showTab(3));
        this.$('#store-button').on('click', e => {
            if (!this.loadedShop) {
                Summoner.store().then(url => this.$('#shop-frame').src = url);
                this.loadedShop = true;
            }
            this.showTab(4)
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

        this.showTab(1);

        this.chatlist = new ChatList();
        this.chatlist.render(this.$('#friends-area'));

        let collection = new CollectionPage();
        collection.render(this.$('#landing-collection'));

        let profile = new ProfilePage();
        profile.render(this.$('#landing-profile'));

        // Kappa.invoke('/debug/draft', []);
        // this.lobby();

        // let canvas = this.$('#tetris-canvas');
        // Tetris.init(canvas[0], () => document.activeElement == document.body || canvas.hasFocus);
        // Tetris.on('scorechange', score => this.$('#tetris-score').text = score);
        // Tetris.start();
        // this.$('#tetris-score').css('width', canvas[0].width + 'px');

        /* News  {
            var list = this.$('#news-list');
            for (var i = 0; i < Math.min(Client.News.length, 10); i++) {
                let news = Client.News[i];
                var node = this.template('news', news);
                node.on('click', e => this.sharp.openLink(news.link));
                list.add(node);
            }
        }*/
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

    private tab: number;
    private showTab(index: number) {
        if (this.tab == index) return;

        for (var i = 0; i < 10; i++) {
            this.refs.background.removeClass('tab-' + i);
        }
        this.refs.background.addClass('tab-' + index);

        this.tab = index;
        this.refs.mainScroller.addClass('faded');
        setTimeout(() => {
            this.refs.mainScroller.removeClass('faded');
            this.refs.mainScroller.css('left', -100 * index + '%');
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

        if (show) this.showTab(0);
    }
}