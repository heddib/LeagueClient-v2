import { Swish }     from './../ui/swish';
import Module        from './../ui/module';
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

interface Refs {
    loader: Swish;
}

export default class GamePatcherPage extends Module<Refs> {
    constructor() {
        super(template);

        this.update();
    }

    public update() {
        Service.game().then(state => {
            if (state.phase == 'NONE') {
                this.dispatch(this.closed, {});
                return;
            }

            var ratio = state.current / state.total;
            this.refs.loader.css('transform', `translateX(-${(1 - ratio) * 100}%)`)

            setTimeout(() => this.update(), 1000);
        });
    }

    public static required() {
        return new Async<boolean>((resolve, reject) => {
            Service.game().then(state => resolve(state.phase != 'NONE'));
        });
    }
}