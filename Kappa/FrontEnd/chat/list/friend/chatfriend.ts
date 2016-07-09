import Module        from './../../../ui/module';
import * as Assets   from './../../../assets/assets';
import * as Summoner from './../../../summoner/summoner';
import * as Chat     from './../chat';

const html = Module.import('chat/list/friend');

export default class ChatFriend extends Module {
    public selected = this.create<{}>();
    public invited = this.create<number>();

    private data: any;
    private inviting: boolean;

    public constructor() {
        super(html);

        this.node.css('display', 'none');
        this.refs.inviteButton.on('click', () => {
            this.dispatch(this.invited, this.data.id);
        });
    }

    private onMouseUp() {
        if (!this.inviting)
            this.dispatch(this.selected, {});
    }

    public get name() { return this.data.name; }
    public get user() { return this.data.user; }

    public unread(unread: boolean) {
        if (unread) {
            this.node.addClass('unread');
        } else {
            this.node.removeClass('unread');
        }
    }

    public icon(id: number) {
        if (id) this.refs.icon.src = Assets.summoner.icon(id);
    }

    public update(data?) {
        this.data = data;

        this.tick();
    }

    public startInvite() {
        this.inviting = true;
        this.tick();
    }

    public stopInvite() {
        this.inviting = false;
        this.tick();
    }

    public tick() {
        if (!this.data || !this.data.show) {
            this.node.css('display', 'none');
        } else {
            this.node.css('display', null);
            this.node.removeClass('chat', 'dnd', 'away', 'mobile');
            this.node.addClass(this.data.show.toLowerCase());
            if (!this.data.icon) this.data.icon = 1;
            this.refs.name.text = this.data.name;
            this.refs.message.text = this.data.message || '';

            var status = (this.data.status || {}).display;
            var gametimer = '';
            if (this.data.show == 'MOBILE') {
                var millis = new Date().valueOf() - new Date(this.data.lastonline).valueOf();
                var secs = millis / 1000;
                var mins = secs / 60;
                var hrs = mins / 60;
                var days = hrs / 24;
                if (days >= 1)
                    status = 'Offline for ' + Math.floor(days) + ' day' + (days >= 2 ? 's' : '');
                else if (hrs >= 1)
                    status = 'Offline for ' + Math.floor(hrs) + ' hour' + (hrs >= 2 ? 's' : '');
                else if (mins >= 1)
                    status = 'Offline for ' + Math.floor(mins) + ' minute' + (mins >= 2 ? 's' : '');
                else
                    status = 'Offline for ' + Math.floor(secs) + ' second' + (secs >= 2 ? 's' : '');
            }

            if (this.data.game) {
                var gametimer: string = '';
                if (this.data.game.start == 0) {
                    gametimer = 'Loading';
                } else {
                    var millis = <any>new Date().valueOf() - <any>new Date(this.data.game.start).valueOf();

                    var seconds: any = Math.floor(millis / 1000) % 60;
                    let format = Util.timespan(millis, 'm:ss');

                    gametimer = (this.data.game.exact ? '' : '~') + format;
                }
                if (this.data.game.type != -1) {
                    status = 'In ' + Assets.getQueueType(this.data.game.type).display;
                }
            }

            this.node.setClass(this.inviting && !this.data.game && this.data.show != 'MOBILE', 'inviting');

            this.refs.status.text = status;
            this.refs.gametimer.text = gametimer;
        }
    }
}