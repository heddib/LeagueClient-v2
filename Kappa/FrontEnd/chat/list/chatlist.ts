import Module        from './../../ui/module';
import * as Assets   from './../../assets/assets';
import * as Audio    from './../../assets/audio';
import * as Summoner from './../../summoner/summoner';
import * as Invite   from './../../invite/invite';
import * as Chat     from './../chat';

import ChatFriend    from './friend/chatfriend';

const html = Module.import('chat/list');

export default class ChatList extends Module implements IInviteProvider {
    private friends: { [user: string]: ChatFriend } = {};
    private chatHistories: { [user: string]: any[] } = {};
    private intervalId: number;
    private selected: string;

    public constructor() {
        super(html);
        let chatInput = this.$('#active-chat .chat-input > input');
        this.intervalId = setInterval(() => this.tick(), 1000);

        this.$('#active-chat').on('mouseup', (e: MouseEvent) => chatInput.focus());

        chatInput.on('keydown', (e: KeyboardEvent) => {
            if (chatInput.value && e.keyCode == 13) {
                Chat.send(this.selected, chatInput.value);
                chatInput.value = '';
            }
        });

        this.$('#active-chat .exit-button').on('mouseup', (e: MouseEvent) => {
            this.selected = null;
            this.onFriendsUpdate(Chat.list());
        });

        this.subscribe(Chat.update, list => this.onFriendsUpdate(list));
        this.subscribe(Chat.message, msg => this.onMessage(msg.user, msg));
        if (Chat.list()) this.onFriendsUpdate(Chat.list());

        Summoner.me.on(me => {
            this.refs.myName.text = me.name;
            this.refs.myIcon.src = Assets.summoner.icon(me.icon);
        });
    }

    public start() {
        for (var user in this.friends) {
            this.friends[user].startInvite();
        }
    }

    public stop() {
        for (var user in this.friends) {
            this.friends[user].stopInvite();
        }
    }

    public dispose() {
        super.dispose();
        clearInterval(this.intervalId);
    }

    private onFriendsUpdate(list: any[]) {
        for (let i = 0; i < list.length; i++) {
            let friend = this.friends[list[i].user];
            if (!friend) {
                friend = this.friends[list[i].user] = new ChatFriend();
                friend.selected.on(e => {
                    this.selected = friend.user;
                    this.drawSelected();
                });
                friend.invited.on(e => Invite.send(e));
            }
            friend.update(list[i]);
            this.refs.list.insert(friend.node, i);

            if (!this.chatHistories[list[i].user])
                this.chatHistories[list[i].user] = [];
        }

        let ids = [];
        for (let user of list)
            ids.push(user.user.substring(3));
        Summoner.icon(ids).then(map => {
            for (let id in map) {
                if (this.friends['sum' + id])
                    this.friends['sum' + id].icon(map[id]);
            }
        });

        this.drawSelected();
        this.tick();
    }

    private drawSelected() {
        if (this.refs.activeChat.hasClass('expanded') != this.selected) {
            if (this.selected) {
                this.friends[this.selected].unread(false);
                this.refs.activeChat.addClass('expanded');

                this.refs.selectedName.text = this.friends[this.selected].name;

                this.refs.messageList.empty();
                this.chatHistories[this.selected].forEach(msg => this.appendMessage(msg, false));
                this.refs.messageList.scrollToBottom();

                this.refs.chatInput.focus();
            } else {
                this.refs.activeChat.removeClass('expanded');
            }
        }
    }

    private onMessage(user, msg: Domain.ChatMessage) {
        this.chatHistories[user].push(msg);

        if (msg.received && !msg.archived) {
            Audio.effect('chat', 'message');
        }

        if (this.selected == user) {
            this.appendMessage(msg, true);
        } else if (!msg.archived) {
            this.friends[user].unread(true);
        }
    }

    private appendMessage(msg, scroll) {
        var date = new Date(msg.date);
        let pre = '';
        if (new Date(msg.date).setHours(0, 0, 0, 0) != new Date().setHours(0, 0, 0, 0))
            pre = `${date.getDate()}/${date.getMonth() + 1} `;

        var node = this.template('message', {
            style: msg.received ? 'received' : 'sent',
            date: `${pre}${date.getHours()}:${date.getMinutes()}`,
            body: msg.body
        });
        this.refs.messageList.add(node);
        if (scroll) this.refs.messageList.scrollToBottom();
    }

    private tick() {
        for (let user in this.friends) {
            this.friends[user].tick();
        }
    }
}