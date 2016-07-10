import * as Kappa     from './../kappa';
import { Swish, $ }   from './../ui/swish';
import Module         from './../ui/module';
import * as Tabs      from './../ui/tabs';

import * as Assets    from './../assets/assets';
import * as Summoner  from './../summoner/summoner';

import * as Meta      from './../meta/meta';

import ChatList       from './../chat/list/chatlist';

import * as Invite    from './../invite/invite';

import CollectionPage from './../collection/collection';
import PatcherPage    from './../patcher/game';
import PlayLoop       from './../playloop/playloop';
import ProfilePage    from './../profile/profile';

const template = (
    <module class="landing">
        <div class="center">
            <div class="header">
                <div class="play-button" data-ref="mainTab"><span data-ref="playButton">PLAY</span></div>

                <div class="tab-button" data-ref="homeTab"><span>HOME</span></div>
                <div class="tab-button" data-ref="profileTab"><span>PROFILE</span></div>
                <div class="tab-button" data-ref="collectionTab"><span>COLLECTION</span></div>
                <div class="tab-button" data-ref="storeTab"><span>STORE</span></div>

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
            <div class="center">
                <container class="back" data-ref="containerBack"/>
                <container class="fore" data-ref="container"/>
            </div>
        </div>
        <container class="right" data-ref="friendsContainer"/>
    </module>
);

const homeTemplate = (
    <module class="landing-home">
        <div class="left">
            <div class="invite-list" data-ref="inviteList"></div>
        </div>
        <x-flexpadd></x-flexpadd>
        <div class="right"/>
    </module>
);

Util.preload('images/landing_background.png');

export default class Landing extends Module {
    private loadedShop: boolean;
    private chatlist: ChatList;
    private patcher: PatcherPage;
    // private tabChange: (index: number) => void;

    private play: PlayLoop;
    private home = Module.create(homeTemplate);
    private profile: ProfilePage;
    private collection: CollectionPage;

    public constructor(accountState: Domain.Authentication.AccountState) {
        super(template);

        this.chatlist = new ChatList();
        this.chatlist.render(this.refs.friendsContainer);

        this.play = new PlayLoop(this.chatlist);
        this.collection = new CollectionPage();
        this.profile = new ProfilePage();

        Invite.update.on(e => this.drawInvites(e));
        this.drawInvites(Invite.list());

        // this.refs.background.src = back;

        this.refs.mainTab.on('click', () => this.showTab(this.play));
        this.refs.homeTab.on('click', () => this.showTab(this.home));
        this.refs.profileTab.on('click', () => this.showTab(this.profile));
        this.refs.collectionTab.on('click', () => this.showTab(this.collection));
        // this.refs.storeTab.on('click', () => this.showTab(this.profile));

        // let tabs = []
        // for (var i = 0; i < 5; i++) tabs[i] = this.refs['tab' + i];
        // this.tabChange = Tabs.create(tabs, 1, (old, now) => this.onTabChange(old, now));

        // Store tab
        // this.refs.tab4.on('click', e => {
        //     if (!this.loadedShop) {
        //         Summoner.store().then(url => this.refs.shopFrame.src = url);
        //         this.loadedShop = true;
        //     }
        // });
        this.refs.balances.on('click', e => Summoner.store().then(url => Meta.link(url)));

        this.subscribe(Summoner.me, this.onMe);

        if (accountState.inGame) {
            this.play.map();
            this.play.ingame();
            this.showTab(this.play);
        } else {
            PatcherPage.required().then(b => {
                if (b) {
                    this.patcher = new PatcherPage();
                    this.subscribe(this.patcher.complete, this.onPatched);
                    this.showTab(this.patcher);
                } else {
                    this.onPatched(true);
                }
            });
            this.showTab(this.home);
        }
    }

    public dispose() {
        super.dispose();
    }

    private onPatched(force?: boolean) {
        if (!force && !this.patcher) return;
        this.patcher = null;
    }

    private onMe(me) {
        this.refs.ip.text = me.ip;
        this.refs.rp.text = me.rp;
    }

    private drawInvites(list) {
        this.home.refs.inviteList.empty();
        for (let invite of list) {
            var control = new Invite.Control(invite);
            control.custom.on(e => {
                this.play.map(e.game.map);
                this.play.custom();
                this.showTab(this.play);
            });
            control.lobby.on(e => {
                this.play.map(e.game.map);
                this.play.lobby(false);
                this.showTab(this.play);
            });
            control.render(this.home.refs.inviteList);
        }
    }

    private timeoutId: number;
    private showTab(mod: Module) {
        mod.render(this.refs.containerBack);
        this.refs.container.addClass('faded');

        const duration = 150;

        clearTimeout(this.timeoutId);
        this.timeoutId = setTimeout(() => {
            this.refs.container.empty();
            this.refs.container.removeClass('faded');

            this.timeoutId = setTimeout(() => {
                mod.render(this.refs.container);
            }, duration);
        }, duration);
    }
}