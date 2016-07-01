import http         from './../util/http';
import * as Service from './service';
import * as Discord from 'discord-domain';

let guilds: { [id: string]: Discord.Guild.Guild } = {};
const events = new EventModule();

export const ready = events.create<Discord.Gateway.ReadyData>();

export function login(email: string, pass: string) {
    Service.Auth.login(email, pass).then(onAuthed);
}

function onAuthed() {
    Service.Gateway.connect();
}

Service.ready.on(data => {
    events.dispatch(ready, data);

    console.log(`Logged into discord as ${data.user.username}`);
    console.log(data.user_settings);
    
    debugger;

    for (let guild of data.guilds) {
        guilds[guild.id] = guild;
    }
});

Service.presenceUpdate.on(data => {
    let guild = guilds[data.guild_id];
    let member = guild.members.first(m => m.user.id == data.user.id);
});
