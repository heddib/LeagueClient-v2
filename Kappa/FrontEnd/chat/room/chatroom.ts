import Module        from './../../ui/module';
import * as Chat     from './../chat';
import * as Service  from './service';

const html = Module.import('chat/room');

interface Refs {
    history: Swish;
}

export default class ChatRoom extends Module<Refs> {
    private room: string;
    private list: Domain.Chat.MucFriend[] = [];
    private messages: Domain.Chat.MucMessage[] = [];

    public constructor(id: string) {
        super(html);

        this.room = id;

        this.subscribe(Service.join, friend => {
            if (friend.room == this.room) this.onJoin(friend);
        });
        this.subscribe(Service.leave, friend => {
            if (friend.room == this.room) this.onLeave(friend);
        });
        this.subscribe(Service.message, msg => {
            if (msg.room == this.room) this.onMessage(msg);
        });

        this.$('.chat-input > input').on('keyup', (e: KeyboardEvent) => this.onChatInput(e));
    }

    public dispose() {
        super.dispose();
    }

    private onJoin(user: Domain.Chat.MucFriend) {
        var node = this.template('lobby-message', {
            body: user.name + ' joined'
        });
        this.append(node);
    }

    private onLeave(user: Domain.Chat.MucFriend) {
        var node = this.template('lobby-message', {
            body: user.name + ' left'
        });
        this.append(node);
    }

    private onMessage(msg: Domain.Chat.MucMessage) {
        var node = this.template('message', msg);
        this.append(node);
    }

    private append(node: Swish) {
        let obj = this.refs.history[0];
        let bottom = obj.scrollTop === (obj.scrollHeight - obj.offsetHeight);
        this.refs.history.add(node);
        if (bottom)
            this.refs.history.scrollToBottom();
    }

    private onChatInput(e: KeyboardEvent) {
        let input = this.$('.chat-input > input');

        if (input.value && e.keyCode == 13) {
            Service.send(this.room, input.value);
            input.value = '';
        }
    }
}