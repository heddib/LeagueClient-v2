import Module        from './../ui/module';

import * as Audio    from './../../frontend/audio';
import * as Assets   from './../../frontend/assets';
import * as Invite   from './../../frontend/invite';
import * as Summoner from './../../frontend/summoner';
import * as PlayLoop from './../../frontend/playloop';

Invite.received.on(() => {
    Audio.effect('home', 'invite');
});

interface Props {
    invite: Domain.Game.Invitation;
    onCustom: (invite: Domain.Game.Invitation) => void;
    onLobby: (invite: Domain.Game.Invitation) => void;
}

interface Refs {
    icon: Swish;
}

export default class Control extends React.Component<Props, Refs> {
    private onClick(accept: boolean) {
        if (accept) {
            Invite.accept(this.props.invite).then(() => {
                if (this.props.invite.game.queue == 0)
                    this.props.onCustom(this.props.invite);
                else
                    this.props.onLobby(this.props.invite);
            });
        } else {
            Invite.decline(this.props.invite);
        }
    }

    constructor(props: Props) {
        super(props);

        Summoner.icon([props.invite.from.id]).then(ids => {
            var id = ids[props.invite.from.id];
            if (id) this.refs.icon.src = Assets.summoner.icon(id);
        });
    }

    protected render() {
        let queue = PlayLoop.queueNames[this.props.invite.game.queue]
            || PlayLoop.featuredNames[this.props.invite.game.queue]
            || Assets.getQueueType(this.props.invite.game.queue).display;

        return (
            <module class="invitation">
                <img class="icon" ref="icon"/>
                <div class="text">
                    <span>{ this.props.invite.from.name }</span>
                    <span>{ queue }</span>
                </div>
                <x-flexpadd/>
                <div class="buttons">
                    <span onClick={ () => this.onClick(true) } class="accept-button"></span>
                    <span onClick={ () => this.onClick(false) } class="exit-button"></span>
                </div>
            </module>
        );
    }
}