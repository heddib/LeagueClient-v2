import Module        from './../../ui/module';
import ChatRoom      from './../../chat/room/chatroom';
import * as Assets   from './../../../frontend/assets';
import { PlayLoop }  from './../../../backend/services';

const template = (
    <module id="ingame">
        <div class="page waiting-page" data-ref="waiting">
            <span class="label">Currently In Game</span>
            <button class="launch-button" data-ref="launch">Launch</button>
        </div>
        <div class="page post-page" data-ref="post">
            <x-flexpadd/>
            <div class="center">
                <div class="ip">
                    <span data-ref="ip"/>
                    <span> IP</span>
                </div>
                <div class="grade" data-ref="gradeBack">
                    <span data-ref="grade"/>
                </div>
                <button class="leave" data-ref="leave">Leave</button>
            </div>
            <x-flexpadd/>
            <container class="chat-area" data-ref="chatArea"/>
        </div>
    </module>
);

interface Refs {
    waiting: Swish;
    launch: Swish;

    post: Swish;
    ip: Swish;
    grade: Swish;
    gradeBack: Swish;
    chatArea: Swish;
    leave: Swish;
}

export default class InGamePage extends Module<Refs> {
    private room: ChatRoom;

    public constructor() {
        super(template);

        PlayLoop.InGame.launch();

        this.subscribe(PlayLoop.InGame.state, this.onActiveState);
        this.subscribe(PlayLoop.InGame.finished, this.onFinished);
        this.subscribe(PlayLoop.PostGame.state, this.onPostState);
        this.refs.post.css('display', 'none')
        this.refs.launch.on('click', () => PlayLoop.InGame.launch());
        this.refs.leave.on('click', () => this.dispatch(this.closed, {}));
    }

    private onFinished(error: boolean) {
        this.refs.waiting.css('display', 'none')
        this.refs.post.css('display', null)
        if (error) {
            this.dispatch(this.closed, {});
        }
    }

    private onActiveState(state: Domain.Game.ActiveGameState) {
        this.refs.launch.setClass(!state.launched, 'visible');
    }

    private onPostState(state: Domain.Game.PostGameState) {
        console.info(state);
        this.refs.ip.text = state.ipEarned;

        if (state.championMastery) {
            this.refs.gradeBack.setBackgroundImage(Assets.champion.splash(state.championMastery.champion, 0));
            this.refs.grade.text = state.championMastery.grade;
        }

        if (!this.room && state.chatroom != guid.empty) {
            this.room = new ChatRoom(state.chatroom);
            this.room.render(this.refs.chatArea);
        }
    }
}
