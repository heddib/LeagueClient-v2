import { Swish }     from './../../ui/swish';
import Module        from './../../ui/module';
import * as Assets   from './../../assets/assets';
import * as Service  from './service';

const template = (
    <module id="ingame">
        <span class="label">Currently In Game</span>
    </module>
);

export default class InGamePage extends Module {
    public constructor() {
        super(template);

        Service.launch();
        this.subscribe(Service.state, this.onState);
    }

    private onState(state: Domain.ActiveGame.ActiveGameState) {
        if (!state.ingame) {
            this.dispatch(this.close, {});
            return;
        }
    }
}
