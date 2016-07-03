import { Swish }      from './../../ui/swish';
import Module         from './../../ui/module';
import * as Assets    from './../../assets/assets';
import * as Audio     from './../../assets/audio';
import * as Masteries from './../../collection/masteries/masteries';
import * as Runes     from './../../collection/runes/runes';
import * as Skins     from './../../collection/skins/skins';

import * as PlayLoop  from './../playloop';
import * as Service   from './service';

import ChatRoom       from './../../chat/room/chatroom';

import Player         from './player/player';

const html = Module.import('playloop/champselect');
const musics = {
    2: 'draftpick',
    18: 'draftpick',
};

export default class ChampSelect extends Module {
    private timerEnd: number;
    private intervalId: number;
    private header: string;

    private spell1: number;
    private spell2: number;
    private isSpell1: boolean;

    private room: ChatRoom;

    private masteries: { [id: number]: MasteryBookPageDTO } = {};
    private runes: { [id: number]: SpellBookPageDTO } = {};
    private champs: { [id: number]: Swish } = {};

    private doDispose = true;

    private eventHandlers: [X_Event<any>, (t: any) => void][] = [];
    private music: HTMLAudioElement;

    public start = this.create<{}>();
    public cancel = this.create<{}>();
    public custom = this.create<{}>();

    public constructor() {
        super(html);

        this.fetchBooks();
        this.refs.champBackground.addClass('faded-out');
        this.refs.skinsControls.addClass('hidden');

        this.refs.champs.empty();

        for (let champ of Assets.gamedata.champions) {
            let node = this.template('champicon', {
                imageurl: Assets.champion.splash(champ.id, 0)
            });
            node.css('display', 'none');
            node.on('click', e => this.onChampClick(champ.id));
            this.champs[champ.id] = node;
            this.refs.champs.add(node);
            // node = this.template('champpopupitem', {
            //   imageurl: `http://l3cdn.riotgames.com/releases/live/projects/lol_air_client/releases/0.0.1.186/files/assets/images/champions/${key}_Splash_Tile_0.jpg`
            //   // imageurl: Riot.image('champ_profile', key, 0)
            // });
            // this.$('#champs-popup .champs').add(node);
        }

        this.subscribe(Service.state, this.onState);
        this.subscribe(Service.start, () => this.onStart());
        this.subscribe(Service.returnToLobby, () => this.onAdvance('LOBBY'));
        this.subscribe(Service.returnToQueue, () => this.onAdvance('MATCHMAKING'));
        this.subscribe(Service.returnToCustom, () => this.onAdvance('CUSTOM'));

        this.refs.skinLeftArrow.on('click', () => {
            let skins = this.skins[this.lastState.me.champion];
            var i = skins.firstIndex(c => c.selected);
            skins[i].selected = false;
            skins[i - 1].selected = true;
            this.drawSkins(this.lastState.me);
            Service.selectSkin(skins[i - 1].id);
        });
        this.refs.skinRightArrow.on('click', () => {
            let skins = this.skins[this.lastState.me.champion];
            var i = skins.firstIndex(c => c.selected);
            skins[i].selected = false;
            skins[i + 1].selected = true;
            this.drawSkins(this.lastState.me);
            Service.selectSkin(skins[i + 1].id);
        });
        this.refs.reroll.on('click', () => {
            Service.reroll();
        });
    }

    public dispose() {
        super.dispose();
        this.music.pause();
        window.clearInterval(this.intervalId);
        if (this.doDispose)
            PlayLoop.abandon();
    }

    private onAdvance(state: string) {
        if (state == 'LOBBY' || state == 'MATCHMAKING') {
            this.doDispose = false;
            this.dispatch(this.cancel, {});
        } else if (state == 'CUSTOM') {
            this.doDispose = false;
            this.dispatch(this.custom, {});
        }
    }

    private onStart() {
        this.doDispose = false;
        this.dispatch(this.start, {});
    }

    private fetchBooks() {
        let select: HTMLSelectElement = this.refs.masteriesList[0];
        let masteries = Masteries.list();

        while (select.options.length > 0) select.remove(0);
        for (let i = 0; i < masteries.length; i++) {
            this.masteries[masteries[i].pageId] = masteries[i];

            let option = document.createElement('option');

            option.value = masteries[i].pageId.toString();
            option.text = masteries[i].name;
            select.add(option);
            if (masteries[i].current)
                select.selectedIndex = i;
        }

        select = this.refs.runesList[0];
        let runes = Runes.list();

        while (select.options.length > 0) select.remove(0);
        for (let i = 0; i < runes.length; i++) {
            this.runes[runes[i].pageId] = runes[i];

            let option = document.createElement('option');

            option.value = runes[i].pageId.toString();
            option.text = runes[i].name;
            select.add(option);
            if (runes[i].current)
                select.selectedIndex = i;
        }
    }

    private alliedPlayers: Player[] = [];
    private enemyPlayers: Player[] = [];

    private lastState: ChampSelectState;
    private taken: number[];
    private skins: { [id: number]: Skin[] };

    private onState(state: ChampSelectState) {
        if (!state.allies) return;

        if (!this.music) {
            PlayLoop.current().then(state => {
                if (state.queueId != 0)
                    this.refs.exitButton.css('display', 'none');
                let music = musics[state.queueConfigId] || 'defaultpick';
                this.music = Audio.music('champselect', music);
            });
        }

        if (!this.room && state.chatroom != guid.empty) {
            this.room = new ChatRoom(state.chatroom);
            this.refs.chatContainer.add(this.room.node);
        }

        if (state.phase == 'FINALIZING') {
            this.drawSkins(state.me);
        }

        if (this.lastState && state.turn != this.lastState.turn) {
            if (state.me.active && !this.lastState.me.active)
                Audio.effect('champselect', 'you');
            else if (this.lastState.phase == 'BANNING')
                Audio.effect('champselect', 'ban');
            else if (this.lastState.phase == 'PICKING')
                Audio.effect('champselect', 'pick');
        }

        this.taken = [];
        this.timerEnd = new Date().getTime() + state.remaining;
        if (!this.intervalId) this.intervalId = setInterval(() => this.onTimerTick(), 100);

        this.$('x-banlist').empty();
        for (var i = 0; i < state.alliedBans.length; i++) {
            var node = this.template('banicon', {
                imageurl: Assets.champion.icon(state.alliedBans[i])
            });
            this.taken.push(state.alliedBans[i]);
            node.addClass('taken');
            this.refs.allyBans.add(node);
        }
        for (var i = 0; i < state.enemyBans.length; i++) {
            var node = this.template('banicon', {
                imageurl: Assets.champion.icon(state.enemyBans[i])
            });
            this.taken.push(state.enemyBans[i]);
            node.addClass('taken');
            this.refs.enemyBans.add(node);
        }

        let team = (enemy) => {
            let src = enemy ? state.enemies : state.allies;
            let dst = enemy ? this.refs.enemyTeam : this.refs.alliedTeam;
            let players = enemy ? this.enemyPlayers : this.alliedPlayers;
            for (let i = 0; i < src.length; i++) {
                let player = players[i];
                if (!player) {
                    player = players[i] = new Player();
                    player.trade.on(() => {
                        Service.trade(src[i].id);
                    });
                    player.cancel.on(() => {
                        Service.decline(src[i].id);
                    })
                    player.respond.on(yes => {
                        if (yes) Service.trade(src[i].id);
                        else Service.decline(src[i].id);
                    });
                }

                if (state.phase == 'FINALIZING' && src[i].id == state.me.id)
                    src[i].champion = 0;
                player.update(src[i], i, enemy);

                dst.insert(player.node, i);

                if (!src[i].intent)
                    this.taken.push(src[i].champion);
            }
        };

        team(false);
        team(true);
        this.updateSpells(state.me.spell1, state.me.spell2);

        if (state.isBlue) {
            this.refs.allyTitle.addClass('blue');
            this.refs.enemyTitle.addClass('red');
        } else {
            this.refs.allyTitle.addClass('red');
            this.refs.enemyTitle.addClass('blue');
        }

        switch (state.phase) {
            case 'PLANNING':
                this.header = 'Preparing';
                break;
            case 'BANNING':
                this.header = 'Banning';
                break;
            case 'PICKING':
                this.header = 'Picking';
                break;
            case 'FINALIZING':
                this.header = 'Game will begin soon';
                break;

            case 'STARTING':
            case 'IN_PROGRESS':
                this.doDispose = false;
                this.dispatch(this.start, {});
                break;
        }

        this.lastState = state;
        this.fillChampGrid();
    }

    private fillChampGrid() {
        var selector = this.refs.spellSelector;
        selector.empty();

        for (let spell of Assets.gamedata.summoners) {
            if (this.lastState.inventory.availableSpells.indexOf(spell.id) < 0) continue;
            else {
                let node = this.template('spell-selectable', {
                    imgurl: Assets.summoner.spell(spell.id)
                });
                node.on('click', e => this.onSpellSelectClick(spell.id));
                selector.add(node);
            }
        }

        this.refs.lockIn.disabled = !this.lastState.me.active;

        var search: RegExp;
        try {
            search = new RegExp(this.refs.searchBox.value, 'i');
        } catch (e) {
            search = new RegExp('.*');
        }

        let active = this.lastState.me.active || this.lastState.me.champion == 0 || this.lastState.me.intent;

        for (let champ of Assets.gamedata.champions) {
            let node: Swish = this.champs[champ.id];
            let match = champ.name.match(search);

            let check;
            if (this.lastState.phase == 'BANNING' && this.lastState.me.active)
                check = this.lastState.inventory.bannableChamps
            else
                check = this.lastState.inventory.pickableChamps;

            if (!check) return;

            if (!match || check.indexOf(champ.id) < 0)
                node.css('display', 'none');
            else
                node.css('display', null);

            node.setClass(!active, 'inactive');
            node.setClass(this.lastState.me.champion == champ.id, 'active');
            node.setClass(this.taken.indexOf(champ.id) >= 0, 'taken');
        }
    }

    private updateSpells(spell1, spell2) {
        this.spell1 = spell1;
        this.spell2 = spell2;
        this.refs.spell1.src = Assets.summoner.spell(spell1);
        this.refs.spell2.src = Assets.summoner.spell(spell2);
    }

    private drawSkins(me: GameMember) {
        let canRoll = me.reroll && me.reroll.points >= me.reroll.cost;
        this.refs.reroll.setClass(!canRoll, 'hidden');

        let callback = (map: { [id: number]: Skin[] }) => {
            let skins = map[me.champion];
            let init = !skins.any(s => s.id % 1000 == 0);

            if (init)
                skins.unshift({ id: me.champion * 1000, selected: !skins.any(s => s.selected) });

            let i = skins.firstIndex(s => s.selected);
            let url = Assets.champion.splash(Math.floor(skins[i].id / 1000), skins[i].id % 1000);
            let tmp = document.createElement('img');
            tmp.src = url;
            tmp.onload = () => {
                this.refs.champBackground.css('background-image', 'url("' + url + '")');
                if (!init) return;
                this.refs.champBackground.removeClass('faded-out');
                this.refs.skinsControls.removeClass('hidden');
                this.refs.champs.parent.css('display', 'none');
                this.refs.champsControls.css('display', 'none');
            };

            this.refs.skinLeftArrow.setClass(i == 0, 'hidden');
            this.refs.skinRightArrow.setClass(i + 1 == skins.length, 'hidden');
        };
        if (this.skins) callback(this.skins);
        else Skins.owned().then(s => callback(this.skins = s));
    }

    private onSearchInput(e) {
        this.fillChampGrid();
    }

    private onLockInClick() {
        Service.lockIn();
    }

    private lastQuote: HTMLAudioElement;
    private onChampClick(id) {
        let play = this.lastState.phase == 'PICKING';
        Service.selectChampion(id).then(() => {
            if (play) {
                if (this.lastQuote) this.lastQuote.pause();
                this.lastQuote = Audio.champ_quote(id);
            }
        });
    }

    private onSpell1Click = (e, n) => this.onSpellClick(n, true);
    private onSpell2Click = (e, n) => this.onSpellClick(n, false);
    private onSpellClick(node: Swish, isSpell1: boolean) {
        this.isSpell1 = isSpell1;
        var size = this.$('.spell-selectable').bounds.width;
        var width = (size + 10) * 3;
        var height = width;
        this.refs.spellSelector.css('width', width);
        this.refs.spellSelector.css('height', height);
        var x = node.bounds.left + node.bounds.width / 2 - width / 2 - 5;
        var y = node.bounds.top + node.bounds.height / 2 - height / 2 - 5;
        y -= size + 10;
        this.refs.spellSelector.css('top', y + 'px');
        this.refs.spellSelector.css('left', x + 'px');
    }

    private onSpellSelectClick(id) {
        this.refs.spellSelector.css('top', null);
        this.refs.spellSelector.css('left', null);
        if (this.isSpell1) {
            if (id == this.spell2) this.spell2 = this.spell1;
            this.spell1 = id;
        } else {
            if (id == this.spell1) this.spell1 = this.spell2;
            this.spell2 = id;
        }
        Service.selectSpells(this.spell1, this.spell2);
    }

    private onRunesChange(e: MouseEvent) {
        var list = <HTMLSelectElement>e.currentTarget;
        var id = parseInt(list.value);
        Runes.select(this.runes[id]);
    }

    private onMasteriesChange(e: MouseEvent) {
        var list = <HTMLSelectElement>e.currentTarget;
        var id = parseInt(list.value);
        Masteries.select(this.masteries[id]);
    }

    private onEditMasteriesClick(e: MouseEvent) {
        Masteries.show(() => this.fetchBooks());
    }

    private onClose() {
        PlayLoop.quit();
        this.doDispose = false;
        this.dispatch(this.close, {});
    }

    private onTimerTick() {
        var diff = this.timerEnd - new Date().getTime();
        diff = Math.max(diff, 0);
        this.refs.phaseTitle.text = this.header + ' - ' + Math.floor(diff / 1000)
    }
}