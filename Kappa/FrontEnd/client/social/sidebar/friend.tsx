import * as Chat     from './../../../frontend/chat';
import * as Assets   from './../../../frontend/assets';
import * as Summoner from './../../../frontend/summoner';

interface Props {
    friend: any;
    onSelected: () => void;
    onInvited: (id: number) => void;
}

interface Refs {
    inviteButton: Swish;
    icon: Swish;
    name: Swish;
    message: Swish;
    status: Swish;
    gametimer: Swish;
}

export default class ChatFriend2 extends React.Component<Props, Refs> {
    private inviting: boolean;
    private data: any;

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

    public update(data) {
        this.data = data;

        this.tick();
    }

    public setInviting(inviting: boolean) {
        this.inviting = inviting;
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
                    var millis = new Date().valueOf() - new Date(this.data.game.start).valueOf();

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

    private onClick(s: Swish, e: MouseEvent) {
        this.props.onSelected();
    }

    private onInvite(s: Swish, e: MouseEvent) {
        this.props.onInvited(this.data.id);
    }

    render() {
        this.data = this.props.friend;

        return (
            <module class="sidebar-friend" onClick={ this.onClick }>
                <div class="icon">
                    <img ref="icon"/>
                </div>
                <div class="text">
                    <span ref="name" class="name">{ this.data.name }</span>
                    <span ref="message" class="message"></span>
                    <x-flexpadd></x-flexpadd>
                    <div class="status">
                        <span ref="status" class="status-text"></span>
                        <x-flexpadd></x-flexpadd>
                        <span ref="gametimer" class="gametimer-text"></span>
                    </div>
                </div>
                <svg class="invite-button" onClick={ this.onInvite }>
                    <line x1="16" y1="0" x2="16" y2="32" stroke="#EEEEEE" stroke-width="4" />
                    <line x1="0" y1="16" x2="32" y2="16" stroke="#EEEEEE" stroke-width="4" />
                </svg>
            </module>
        );
    }
}

// export class ChatFriend extends Module<Refs> {
//     public selected = this.create<any>();
//     public invited = this.create<number>();

//     private data: any;
//     private inviting: boolean;

//     public constructor() {
//         super(html);

//         this.node.css('display', 'none');
//         this.refs.inviteButton.on('click', () => {
//             this.dispatch(this.invited, this.data.id);
//         });
//     }

//     private onMouseUp() {
//         if (!this.inviting)
//             this.dispatch(this.selected, {});
//     }

//     public get name() { return this.data.name; }
//     public get user() { return this.data.user; }

//     public unread(unread: boolean) {
//         if (unread) {
//             this.node.addClass('unread');
//         } else {
//             this.node.removeClass('unread');
//         }
//     }

//     public icon(id: number) {
//         if (id) this.refs.icon.src = Assets.summoner.icon(id);
//     }

//     public update(data?) {
//         this.data = data;

//         this.tick();
//     }

//     public setInviting(inviting: boolean) {
//         this.inviting = inviting;
//         this.tick();
//     }

//     public tick() {
//         if (!this.data || !this.data.show) {
//             this.node.css('display', 'none');
//         } else {
//             this.node.css('display', null);
//             this.node.removeClass('chat', 'dnd', 'away', 'mobile');
//             this.node.addClass(this.data.show.toLowerCase());
//             if (!this.data.icon) this.data.icon = 1;
//             this.refs.name.text = this.data.name;
//             this.refs.message.text = this.data.message || '';

//             var status = (this.data.status || {}).display;
//             var gametimer = '';
//             if (this.data.show == 'MOBILE') {
//                 var millis = new Date().valueOf() - new Date(this.data.lastonline).valueOf();
//                 var secs = millis / 1000;
//                 var mins = secs / 60;
//                 var hrs = mins / 60;
//                 var days = hrs / 24;
//                 if (days >= 1)
//                     status = 'Offline for ' + Math.floor(days) + ' day' + (days >= 2 ? 's' : '');
//                 else if (hrs >= 1)
//                     status = 'Offline for ' + Math.floor(hrs) + ' hour' + (hrs >= 2 ? 's' : '');
//                 else if (mins >= 1)
//                     status = 'Offline for ' + Math.floor(mins) + ' minute' + (mins >= 2 ? 's' : '');
//                 else
//                     status = 'Offline for ' + Math.floor(secs) + ' second' + (secs >= 2 ? 's' : '');
//             }

//             if (this.data.game) {
//                 var gametimer: string = '';
//                 if (this.data.game.start == 0) {
//                     gametimer = 'Loading';
//                 } else {
//                     var millis = new Date().valueOf() - new Date(this.data.game.start).valueOf();

//                     var seconds: any = Math.floor(millis / 1000) % 60;
//                     let format = Util.timespan(millis, 'm:ss');

//                     gametimer = (this.data.game.exact ? '' : '~') + format;
//                 }
//                 if (this.data.game.type != -1) {
//                     status = 'In ' + Assets.getQueueType(this.data.game.type).display;
//                 }
//             }

//             this.node.setClass(this.inviting && !this.data.game && this.data.show != 'MOBILE', 'inviting');

//             this.refs.status.text = status;
//             this.refs.gametimer.text = gametimer;
//         }
//     }
// }