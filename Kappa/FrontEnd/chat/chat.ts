import Module          from './../ui/module';
import * as Service    from './service';

let friends = {};

Service.status.on(friend => {
    friends[friend.user] = friend;
    onUpdate();
});

Service.message.on(msg => {
    events.dispatch(message, msg);
});

let _list;

var events = new EventModule();

export const update = events.create<any[]>();
export const message = events.create<any>();

export function send(user: string, message: string) {
    return Service.send(user, message);
}

export function list() {
    return _list;
}

function onUpdate() {
    var list = [];
    for (let user in friends) {
        list.push(friends[user]);
    }
    list.sort((a, b) => {
        if (!a.show) return 1;
        if (!b.show) return -1;

        if (a.show != b.show)
            return compareShow(a.show, b.show);

        if (a.status && b.status && a.status.id != b.status.id)
            return compareStatus(a.status.id, b.status.id);

        if (a.game && b.game && a.game.start != b.game.start)
            return (a.game.start || Number.MAX_VALUE) - (b.game.start || Number.MAX_VALUE);

        if (a.lastonline && b.lastonline)
            return new Date(a.lastonline).getTime() - new Date(b.lastonline).getTime();

        return a.name.localeCompare(b.name);
    });
    _list = list;
    events.dispatch(update, list);
}

function compareShow(a: string, b: string) {
    const order = ['CHAT', 'DND', 'AWAY', 'MOBILE'];
    return order.indexOf(a) - order.indexOf(b);
}

function compareStatus(a: string, b: string) {
    const order = [
        'outOfGame',
        'inTeamBuilder', 'hostingPracticeGame', 'hostingRankedGame',
        'hostingCoopVsAIGame', 'hostingNormalGame', 'teamSelect',
        'spectating', 'inQueue', 'inGame', 'tutorial', 'championSelect'
    ];
    return order.indexOf(a) - order.indexOf(b);
}