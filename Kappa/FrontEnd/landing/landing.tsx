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

const template = (
    <module class="landing">
        <div class="center">
            <div class="background">
                <img data-ref="background"/>
            </div>
            <div class="header">
                <div class="play-button" data-ref="tab0"><span data-ref="playButton">PLAY</span></div>

                <div class="tab-button" data-ref="tab1"><span>HOME</span></div>
                <div class="tab-button" data-ref="tab2"><span>PROFILE</span></div>
                <div class="tab-button" data-ref="tab3"><span>COLLECTION</span></div>
                <div class="tab-button" data-ref="tab4"><span>STORE</span></div>

                <x-flexpadd></x-flexpadd>

                <div class="alerts-profile" data-ref="balances">
                    <div class="balance">
                        <img src="images/rp.png" /><span data-ref="rp"></span>
                    </div>
                    <div class="balance">
                        <img src="images/ip.png" /><span data-ref="ip"></span>
                    </div>
                </div>
            </div>
            <div class="center" data-ref="mainScroller">
                <container class="content" data-ref="content"/>
                <div class="home">
                    <div class="left">
                        <div class="invite-list" data-ref="inviteList"></div>
                    </div>
                    <x-flexpadd></x-flexpadd>
                    <div class="right">
                        <container data-ref="discordContainer"/>
                    </div>
                </div>
                <container data-ref="profileContainer" class="profile"/>
                <container data-ref="collectionContainer" class="collection"/>
                <div class="landing-shop">
                    <iframe class="shop-frame" data-ref="shopFrame"></iframe>
                </div>
            </div>
        </div>
        <container class="right" data-ref="friendsContainer"/>
    </module>
);
const back = 'images/landing_background.png';

Util.preload(back);

export default class Landing extends Module {
    private loadedShop: boolean;
    private chatlist: ChatList;
    private patcher: PatcherPage;
    private tabChange: (index: number) => void;

    public constructor(accountState) {
        super(template);

        Invite.update.on(e => this.drawInvites(e));
        this.drawInvites(Invite.list());

        this.refs.background.src = back;

        let tabs = []
        for (var i = 0; i < 5; i++) tabs[i] = this.refs['tab' + i];
        this.tabChange = Tabs.create(tabs, 1, (old, now) => this.onTabChange(old, now));

        // Store tab
        this.refs.tab4.on('click', e => {
            if (!this.loadedShop) {
                Summoner.store().then(url => this.refs.shopFrame.src = url);
                this.loadedShop = true;
            }
        });
        this.refs.balances.on('click', e => Summoner.store().then(url => Meta.link(url)));

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
        this.chatlist.render(this.refs.friendsContainer);

        let collection = new CollectionPage();
        collection.render(this.refs.collectionContainer);

        let profile = new ProfilePage();
        profile.render(this.refs.profileContainer);
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
        this.refs.ip.text = me.ip;
        this.refs.rp.text = me.rp;
    }

    private drawInvites(list) {
        this.refs.inviteList.empty();
        for (let invite of list) {
            var control = new Invite.Control(invite);
            control.custom.on(e => this.custom());
            control.lobby.on(e => this.lobby());
            control.render(this.refs.inviteList);
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
        this.refs.playButton.text = 'PLAY';
    }

    private lobby() {
        var mod = new Lobby(this.chatlist);
        mod.start.on(e => this.champselect());
        this.showModule(mod, true);
        this.refs.playButton.text = 'LOBBY';
    }

    private custom() {
        var mod = new Custom(this.chatlist);
        mod.start.on(e => this.champselect());
        this.showModule(mod, true);
        this.refs.playButton.text = 'CUSTOM';
    }

    private champselect() {
        var mod = new ChampSelect();
        mod.start.on(() => this.ingame());
        mod.cancel.on(e => this.lobby());
        mod.custom.on(e => this.custom());
        this.showModule(mod, true);
        this.refs.playButton.text = 'GAME';
    }

    private ingame() {
        var mod = new InGame();
        this.refs.playButton.text = 'GAME';
        this.showModule(mod, true);
    }

    private module: Module;
    private showModule(mod: Module, show: boolean) {
        let fadein = () => {
            this.refs.content.empty();
            this.module = mod;
            this.module.render(this.refs.content);
            setTimeout(() => {
                this.module.node.removeClass('faded-in');
                this.module.render(this.refs.content);
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