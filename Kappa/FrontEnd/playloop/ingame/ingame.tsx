import { Swish }     from './../../ui/swish';
import Module        from './../../ui/module';
import ChatRoom      from './../../chat/room/chatroom';
import * as Assets   from './../../assets/assets';
import * as Service  from './service';

const template = (
    <module id="ingame">
        <div class="page waiting-page">
            <span class="label">Currently In Game</span>
            <button class="launch-button" data-ref="launch">Launch</button>
        </div>
        <div class="page post-page">
            <x-flexpadd/>
            <x-flexpadd/>
            <container data-ref="chat-area"/>
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
    }

    private onActiveState(state: Domain.Game.ActiveGameState) {
        this.refs.launch.setClass('visible', !state.launched);
    }

    private onPostState(state: Domain.Game.PostGameState) {
        console.info(state);

        if (!this.room) {
            this.room = new ChatRoom(state.chatroom);
        }
    }
}
