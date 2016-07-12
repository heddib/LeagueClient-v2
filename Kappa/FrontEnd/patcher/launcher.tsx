import { Swish }     from './../ui/swish';
import Module        from './../ui/module';
import * as Service from './service';

const template = (
    <module class="launcher-patcher">
        <div class="icon"/>
        <div data-ref="loaderBack" class="loader-back">
            <div data-ref="loader"></div>
        </div>
    </module>
);

interface Refs {
    loader: Swish;
}

export default class LauncherPatcherPage extends Module<Refs> {
    public complete = this.create<any>();

    constructor() {
        super(template);

        this.update();
    }

    public update() {
        Service.launcher().then(state => {
            if (state.phase == 'NONE') {
                this.dispatch(this.complete, {});
                return;
            }

            var ratio = state.current / state.total;
            this.refs.loader.css('transform', `translateX(-${(1 - ratio) * 100}%)`)

            setTimeout(() => this.update(), 1000);
        });
    }

    public static required() {
        return new Async<boolean>((resolve, reject) => {
            Service.launcher().then(state => resolve(state.phase != 'NONE'));
        });
    }
}