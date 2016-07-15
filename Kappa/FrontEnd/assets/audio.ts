import http          from './../util/http';
import Settings       from './../meta/settings';
import * as Assets   from './assets';

const VolumeType = {
    MUSIC: 'MUSIC',
    CHAMP_QUOTE: 'CHAMP_QUOTE',
    CHAMP_SFX: 'CHAMP_SFX',
    DEFAULT: 'DEFAULT'
};

declare type Volumes = { [id: string]: number };

const settings = new Settings<Volumes>('audio');

let cache: { [id: string]: HTMLAudioElement } = {};
let playing: { [id: string]: HTMLAudioElement } = {};
let volumes: Volumes;
settings.load().then(v => volumes = v || {});

function get(path: string) {
    if (!cache[path]) {
        let url = 'assets/audio' + path;
        if (!url.endsWith('.ogg')) url += '.ogg';
        cache[path] = new Audio(url);
    }

    return cache[path];
}

function play(type: string, key: string, allowOverlap = false) {
    if (!allowOverlap && playing[type]) playing[type].pause();
    if (!volumes[type]) volumes[type] = 1;

    let audio = get(key);
    audio.currentTime = 0;
    audio.volume = volumes[type];
    audio.play();
    playing[type] = audio;

    return audio;
}

export namespace Volume {
    function volume(type: string, value: number) {
        value = Util.clamp(value, 0, 1);
        volumes[type] = value;
        if (playing[type]) playing[type].volume = value;
        settings.save(volumes);
    }

    export function music(value: number) {
        volume(VolumeType.MUSIC, value);
    }

    export function champ_quote(value: number) {
        volume(VolumeType.CHAMP_QUOTE, value);
    }

    export function champ_sfx(value: number) {
        volume(VolumeType.CHAMP_SFX, value);
    }

    export function effect(value: number) {
        volume(VolumeType.DEFAULT, value);
    }
}

export function music(...keys: string[]) {
    return play(VolumeType.MUSIC, '/' + keys.join('/'));
}

export function champ_quote(champ: number) {
    return play(VolumeType.CHAMP_QUOTE, '/champ_quote/' + champ);
}

export function champ_sfx(champ: number) {
    return play(VolumeType.CHAMP_SFX, '/champ_sfx/' + champ);
}

export function effect(...keys: string[]) {
    return play(VolumeType.DEFAULT, '/' + keys.join('/'), true);
}