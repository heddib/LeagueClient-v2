import { Swish, $ }   from './../../ui/swish';
import Module         from './../../ui/module';

import * as Discord   from 'discord-domain';
import * as Service   from './../discord';

const html = Module.import('discord/home');

export default class DiscordHome extends Module {
    constructor() {
        super(html);

        this.subscribe(Service.ready, this.onReady);

        this.refs.homePage.css('display', 'none');
    }

    private onReady(data: Discord.Gateway.ReadyData) {
        this.refs.loginPage.css('display', 'none');
        this.refs.homePage.css('display', null);
        for (let friend of data.presences) {
            let icon = friend.user.avatar ? `https://cdn.discordapp.com/avatars/${friend.user.id}/${friend.user.avatar}.jpg` : 'https://discordapp.com/assets/6debd47ed13483642cf09e832ed0bc1b.png';
            let node = this.template('friend', {
                username: friend.user.username,
                status: friend.status,
                avatar: icon,
                displayStatus: friend.status.substring(0, 1).toUpperCase() + friend.status.substring(1)
            });
            this.refs.friendsList.add(node);
        }
    }

    private onLoginInput(e: KeyboardEvent) {
        if (e.keyCode == 13) this.onLoginClick();
    }

    private onLoginClick() {
        Service.login(this.refs.loginForm.form('email'), this.refs.loginForm.form('password'));
    }
}