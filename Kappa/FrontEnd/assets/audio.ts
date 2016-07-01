import http         from './../util/http';
import * as Assets  from './assets';

let cache: { [id: string]: HTMLAudioElement } = {};

let playingMusic;

function get(path: string) {
    if (!cache[path]) {
        let url = 'assets/audio' + path;
        if (!url.endsWith('.ogg')) url += '.ogg';
        cache[path] = new Audio(url);
    }

    return cache[path];
}

export function music(...keys: string[]) {
    var audio = get('/' + keys.join('/'));
    if (playingMusic) playingMusic.pause();

    audio.currentTime = 0;
    audio.play();
    playingMusic = audio;

    return audio;
}

export function champ_quote(champ: number) {
    var audio = get('/champ_quote/' + champ);

    audio.currentTime = 0;
    audio.play();

    return audio;
}

export function champ_sfx(champ: number) {
    var audio = get('/champ_sfx/' + champ);

    audio.currentTime = 0;
    audio.play();

    return audio;
}

export function effect(...keys: string[]) {
    var audio = get('/' + keys.join('/'));

    audio.currentTime = 0;
    audio.play();

    return audio;
}