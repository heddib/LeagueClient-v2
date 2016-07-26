import * as Chat     from './../../frontend/chat';
import * as Audio    from './../../frontend/audio';
import * as Summoner from './../../frontend/summoner';
import ChatFriend    from './friend';

import Sidebar       from './sidebar/list';

interface Props {

}

interface Refs {
    list: Swish;

    closeChat: Swish;
    chatInput: Swish;
    activeChat: Swish;
    messageList: Swish;
    selectedName: Swish;
}

export default class SocialPage extends React.Component<Props, Refs> {
    private chatHistories: { [user: string]: any[] } = {};
    private friends: { [user: string]: ChatFriend } = {};
    private selected: string;

    public sidebar: Sidebar;

    constructor(props: Props) {
        super(props);

        this.sidebar = new Sidebar({});

        this.refs.closeChat.on('click', e => {
            this.selected = null;
            this.onFriendsUpdate(Chat.list());
        })

        Chat.update.on(list => this.onFriendsUpdate(list));
        Chat.message.on(msg => this.onMessage(msg.user, msg));
        if (Chat.list()) this.onFriendsUpdate(Chat.list());

        setInterval(() => this.tick(), 1000);
    }

    private onFriendsUpdate(list) {
        for (let i = 0; i < list.length; i++) {
            let friend = this.friends[list[i].user];
            if (!friend) {
                friend = this.friends[list[i].user] = new ChatFriend({
                    friend: list[i],
                    onSelected: () => {
                        this.selected = friend.user;
                        this.drawSelected();
                    }
                });
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

    private onMessage(user, msg: Domain.Chat.ChatMessage) {
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
        let format = 'H:mm';
        if (new Date(msg.date).setHours(0, 0, 0, 0) != new Date().setHours(0, 0, 0, 0))
            format = 'M/d ' + format;

        this.refs.messageList.add(React.template(
            <div class={ 'message ' + (msg.received ? 'received' : 'sent') }>
                <div class="date">
                    <span>{ Util.datetime(date, format) }</span>
                </div>
                <x-flexpadd></x-flexpadd>
                <div class="body">
                    <span>{ msg.body }</span>
                </div>
            </div>
        ));

        if (scroll) this.refs.messageList.scrollToBottom();
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

    private tick() {
        for (let user in this.friends) {
            this.friends[user].tick();
        }
    }

    render() {
        return (
            <module class="social-page">
                <div class="left">
                    <div class="list" ref="list"/>
                </div>
                <div class="right">
                    <div ref="activeChat" class="active-chat">
                        <div class="title">
                            <span class="window-button exit-button" ref="closeChat"></span>
                            <span ref="selectedName"></span>
                        </div>
                        <div ref="messageList" class="message-list"></div>
                        <div class="chat-input">
                            <input ref="chatInput" type="text"/>
                        </div>
                    </div>
                </div>
            </module>
        );
    }
}