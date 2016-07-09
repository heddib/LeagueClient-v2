import { Swish }     from './../../ui/swish';
import Module        from './../../ui/module';
import ChatRoom      from './../../chat/room/chatroom';
import * as Assets   from './../../assets/assets';
import * as Service  from './service';

const template = (
    <module id="ingame">
        <div class="page waiting-page" data-ref="waiting">
            <span class="label">Currently In Game</span>
            <button class="launch-button" data-ref="launch">Launch</button>
        </div>
        <div class="page post-page" data-ref="post">
            <x-flexpadd/>
            <x-flexpadd/>
            <container class="chat-area" data-ref="chatArea"/>
        </div>
    </module>
);

export default class InGamePage extends Module {
    private room: ChatRoom;

    public constructor() {
        super(template);

        Service.launch();

        this.subscribe(Service.activeState, this.onActiveState);
        this.subscribe(Service.postState, this.onPostState);
        this.subscribe(Service.finished, this.onFinished);
        this.refs.post.css('display', 'none')
    }

    private onFinished(error: boolean) {
        this.refs.waiting.css('display', 'none')
        this.refs.post.css('display', null)
    }

    private onActiveState(state: Domain.Game.ActiveGameState) {
        this.refs.launch.setClass(!state.launched, 'visible');
    }

    private onPostState(state: Domain.Game.PostGameState) {
        console.info(state);

        if (!this.room && state.chatroom != guid.empty) {
            this.room = new ChatRoom(state.chatroom);
            this.room.render(this.refs.chatArea);
        }
    }
}
