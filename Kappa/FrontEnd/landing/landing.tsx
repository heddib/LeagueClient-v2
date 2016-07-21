import Module         from './../ui/module';
import * as Tabs      from './../ui/tabs';

import * as Assets    from './../assets/assets';
import * as Summoner  from './../summoner/summoner';

import * as Meta      from './../meta/meta';

import ChatList       from './../chat/list/chatlist';
import SocialSidebar  from './../social/sidebar/list';

import CollectionPage from './../collection/collection';
import PlayLoop       from './../playloop/playloop';
import ProfilePage    from './../profile/profile';
import SocialPage     from './../social/page';
import HomePage       from './home';

const template = (
    <module class="landing">
        <div class="center">
            <div class="header">
                <div class="play-button" data-ref="mainTab"><span data-ref="playButton">PLAY</span></div>

                <div class="tab-button" data-ref="homeTab"><span>HOME</span></div>
                <div class="tab-button" data-ref="profileTab"><span>PROFILE</span></div>
                <div class="tab-button" data-ref="collectionTab"><span>COLLECTION</span></div>
                <div class="tab-button" data-ref="socialTab"><span>SOCIAL</span></div>
                <div class="tab-button" data-ref="storeTab"><span>STORE</span></div>

                <x-flexpadd/>

                <div class="alerts-profile">
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

Util.preload('images/landing_background.png');

interface Refs {
    playButton: Swish;

    mainTab: Swish;
    homeTab: Swish;
    profileTab: Swish;
    collectionTab: Swish;
    socialTab: Swish;
    storeTab: Swish;

    ip: Swish;
    rp: Swish;

    friendsContainer: Swish;

    container: Swish;
    containerBack: Swish;
}

export default class Landing extends Module<Refs> {
    private loadedShop: boolean;
    private tabChange: (index: number) => void;

    private play: PlayLoop;
    private home: HomePage;
    private profile: ProfilePage;
    private collection: CollectionPage;
    private social: SocialPage;

    private tabs: { node: Swish }[];

    public constructor(accountState: Domain.Authentication.AccountState) {
        super(template);

        this.social = new SocialPage({});
        this.refs.friendsContainer.add(this.social.sidebar.node);

        this.home = new HomePage();
        this.play = new PlayLoop(this.social.sidebar);
        this.profile = new ProfilePage();
        this.collection = new CollectionPage();

        this.refs.storeTab.on('click', e => {
            Summoner.store().then(url => Meta.link(url));
        });

        this.play.state.on(s => this.refs.playButton.text = s);

        this.subscribe(Summoner.me, this.onMe);

        this.tabs = [this.play, this.home, this.profile, this.collection, this.social];

        let tabs = [this.refs.mainTab, this.refs.homeTab, this.refs.profileTab, this.refs.collectionTab, this.refs.socialTab];
        this.tabChange = Tabs.create(tabs, 1, (old, now) => this.onTabChange(old, now));

        if (accountState.inGame) {
            this.play.map();
            this.play.ingame();

            this.tabChange(this.tabs.indexOf(this.play));
        }

        this.home.custom.on(e => {
            this.play.map(e.game.map);
            this.play.custom();
            this.tabChange(this.tabs.indexOf(this.play));
        });

        this.home.lobby.on(e => {
            this.play.map(e.game.map);
            this.play.lobby(false);
            this.tabChange(this.tabs.indexOf(this.play));
        });
    }

    public dispose() {
        super.dispose();
    }

    private onMe(me) {
        this.refs.ip.text = me.ip;
        this.refs.rp.text = me.rp;
    }

    private timeoutId: number;
    private onTabChange(old: number, now: number) {
        let node = this.tabs[now].node;

        this.refs.containerBack.add(node);
        this.refs.container.addClass('faded');

        const duration = 150;

        clearTimeout(this.timeoutId);
        this.timeoutId = setTimeout(() => {
            this.refs.container.empty();
            this.refs.container.removeClass('faded');

            this.timeoutId = setTimeout(() => {
                this.refs.container.add(node);
            }, duration);
        }, duration);
    }
}