import { Swish }     from './../ui/swish';
import Module        from './../ui/module';
import * as PlayLoop from './../playloop/playloop';
import * as Custom   from './../playloop/custom/service';
import * as Lobby    from './../playloop/lobby/service';
import * as Service from './service';

const template = (
    <module class="game-patcher">
        <div class="title">
            <span>Patching...</span>
        </div>
        <div data-ref="loaderBack" class="loader-back">
            <div data-ref="loader"></div>
        </div>
        <div class="title"></div>
    </module>
);

export default class GamePatcherPage extends Module {
    public complete = this.create<any>();

    constructor() {
        super(template);

        this.update();
    }

    public update() {
        Service.game().then(state => {
            if (state.phase == 'NONE') {
                this.dispatch(this.complete, {});
                return;
            }

            var percent = state.current / state.total * 100;
            this.refs.loader.css('width', percent + "%");

            setTimeout(() => this.update(), 1000);
        });
    }

    public static required() {
        return new Async<boolean>((resolve, reject) => {
            Service.game().then(state => resolve(state.phase != 'NONE'));
        });
    }
}