import Module        from './../ui/module';

import { Patcher as Service} from './../../backend/services';

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
    constructor() {
        super(template);

        this.update();
    }

    public update() {
        Service.launcher().then(state => {
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
        return new Promise<boolean>((resolve, reject) => {
            Service.launcher().then(state => resolve(state.phase != 'NONE'));
        });
    }
}