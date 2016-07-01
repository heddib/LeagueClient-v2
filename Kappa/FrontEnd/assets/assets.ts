import http         from './../util/http';
import * as Defer   from './../defer';
import * as Service from './service';

class Queue {
    get id() { return this._id; }
    get key() { return this._key; }
    get display() { return this._display; }

    private _id: any;
    private _key: any;
    private _display: any;

    constructor(key, id, display) {
        this._key = key;
        this._id = id;
        this._display = display;
    }
}

const ddragon_cdn = 'http://ddragon.leagueoflegends.com/cdn';

let info;
let queuesById: { [id: number]: Queue } = {};
let queuesByKey: { [key: string]: Queue } = {};

let handlers: Function[] = [];

Service.info().then(i => {
    info = i;

    login.video = Service.loginVideo();
    login.image = Service.loginImage()

    var dataurl = `${ddragon_cdn}/${info.version}/data/${info.locale}/`;
    http(dataurl + 'championFull.json').get(e => ddragon.champs = e.json);
    http(dataurl + 'profileicon.json').get(e => ddragon.icons = e.json);
    http(dataurl + 'summoner.json').get(e => ddragon.spells = e.json);
    http(dataurl + 'language.json').get(e => ddragon.languages = e.json);
    http(dataurl + 'rune.json').get(e => ddragon.runes = e.json);
    http(dataurl + 'mastery.json').get(e => ddragon.masteries = e.json);
    http(dataurl + 'item.json').get(e => ddragon.items = e.json);
    http(dataurl + 'map.json').get(e => ddragon.maps = e.json);

    handlers.forEach(a => a());
});

/* Init Queues */ {
    let all_queues = [
        new Queue('CUSTOM', 0, 'Custom'),
        new Queue('NORMAL_3x3', 8, 'Normal 3v3'),
        new Queue('NORMAL_5x5_BLIND', 2, 'Normal Blind Pick'),
        new Queue('NORMAL_5x5_DRAFT', 14, 'Normal Draft Pick (old)'),
        new Queue('RANKED_SOLO_5x5', 4, 'Ranked Soloqueue (old)'),
        new Queue('RANKED_TEAM_3x3', 41, 'Ranked Team 3v3'),
        new Queue('RANKED_TEAM_5x5', 42, 'Ranked Team 5v5'),
        new Queue('ODIN_5x5_BLIND', 16, 'Dominion Blind Pick'),
        new Queue('ODIN_5x5_DRAFT', 17, 'Dominion Draft Pick'),
        new Queue('BOT_ODIN_5x5', 25, 'Dominion Co-op vs AI'),
        new Queue('BOT_5x5_INTRO', 31, 'Intro Co-op vs AI'),
        new Queue('BOT_5x5_BEGINNER', 32, 'Beginner Co-op vs AI'),
        new Queue('BOT_5x5_INTERMEDIATE', 33, 'Intermediate Co-op vs AI'),
        new Queue('BOT_TT_3x3', 52, '3v3 Co-op vs AI'),
        new Queue('GROUP_FINDER_5x5', 61, 'Teambuilder'),
        new Queue('ARAM_5x5', 65, 'ARAM'),
        new Queue('ONEFORALL_5x5', 70, 'One for All'),
        new Queue('FIRSTBLOOD_1x1', 72, 'Snowdown Showdown'),
        new Queue('FIRSTBLOOD_2x2', 73, 'Snowdown Showdown'),
        new Queue('SR_6x6', 75, 'Summoner\'s Rift Hexakill'),
        new Queue('URF_5x5', 76, 'URF'),
        new Queue('BOT_URF_5x5', 83, 'URF Co-op vs AI'),
        new Queue('NIGHTMARE_BOT_5x5_RANK1', 91, 'Doom Bots Rank 1'),
        new Queue('NIGHTMARE_BOT_5x5_RANK2', 92, 'Doom Bots Rank 2'),
        new Queue('NIGHTMARE_BOT_5x5_RANK5', 93, 'Doom Bots Rank 5'),
        new Queue('ASCENSION_5x5', 96, 'Ascension'),
        new Queue('HEXAKILL', 98, 'Twisted Treeline Hexakill'),
        new Queue('BILGEWATER_ARAM_5x5', 100, 'Butcher\'s Bridge ARAM'),
        new Queue('KING_PORO_5x5', 300, 'King Poro'),
        new Queue('COUNTER_PICK', 310, 'Nemesis Draft'),
        new Queue('BILGEWATER_5x5', 313, 'Black Market Brawlers'),
        new Queue('SOMETHING????', 317, 'Definitely not Dominion'),
        new Queue('TEAM_BUILDER_DRAFT_UNRANKED_5x5', 400, 'Normal Draft Pick'),
        new Queue('TEAM_BUILDER_DRAFT_RANKED_5x5', 410, 'Ranked Soloqueue'),
    ];

    for (var i = 0; i < all_queues.length; i++) {
        queuesByKey[all_queues[i].key] = all_queues[i];
        queuesById[all_queues[i].id] = all_queues[i];
    }
}

interface DataDragon {
    champs: ChampionListDto;
    icons: ProfileIconListDto;
    spells: SpellListDto;
    runes: ItemListDto;
    masteries: MasteryListDto;
    items: ItemListDto;
    maps: MapListDto;
    languages: any;
}

export const ddragon: DataDragon = <any>{};

export const login = {
    video: null,
    image: null
};

export const splash = {
    centered: function (champ: number, skin: number) {
        champ = Math.floor(champ);
        skin = Math.floor(skin);
        if (skin < 1000) skin += champ * 1000;
        return `/kappa/assets/game-data/${info.version}/champion/splash/${skin}.jpg`;
    }
};

export function onload(callback: Function) {
    if (info) callback();
    else handlers.push(callback);
}

export function image(type: string, name: string | number, arg?) {
    const fallback = '';
    if (!name) return fallback;
    var path;
    var noVersion = false;
    switch (type) {
        case 'item':
            path = 'img/item/' + name + '.png';
            break;
        case 'champ':
        case 'champ_icon':
            if (typeof name == 'number')
                name = ddragon.champs.keys[name];
            if (!name) return fallback;
            path = 'img/champion/' + name + '.png';
            break;
        case 'champ_splash':
            if (typeof name == 'number')
                name = ddragon.champs.keys[name];
            if (!name) return fallback;
            path = 'img/champion/splash/' + name + '_' + arg + '.jpg';
            noVersion = true;
            break;
        case 'champ_profile':
            if (typeof name == 'number')
                name = ddragon.champs.keys[name];
            if (!name) return fallback;
            path = 'img/champion/loading/' + name + '_' + arg + '.jpg';
            noVersion = true;
            break;
        case 'map':
            path = 'img/map/map' + name + '.png';
            break;
        case 'mastery':
            path = 'img/mastery/' + name + '.png';
            break;
        case 'profile':
            path = 'img/profileicon/' + name + '.png';
            break;
        case 'rune':
            path = 'img/rune/' + name + '.png';
            break;
        case 'spell':
            if (typeof name == 'number') {
                for (var key in ddragon.spells.data)
                    if (ddragon.spells.data[key].key == name)
                        name = ddragon.spells.data[key].id;
            }
            if (!name) return fallback;
            path = 'img/spell/' + name + '.png';
            break;
    }
    if (noVersion)
        return `${ddragon_cdn}/${path}`;
    else
        return `${ddragon_cdn}/${info.version}/${path}`;
}

export function getQueueType(key: string | number) {
    if (typeof key == 'number')
        return queuesById[key];
    else
        return queuesByKey[key];
}