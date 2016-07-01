import { Swish }    from './../../ui/swish';
import Module       from './../../ui/module';
import * as Assets  from './../../assets/assets';

const html = Module.import('login/account');

export default class Account extends Module {
    private disabled: boolean;
    private _account: any;

    public get account() { return this._account; }
    public select = this.create<{}>();

    public constructor(account) {
        super(html);

        this.refs.icon.src = Assets.image('profile', account.icon);
        // this.refs.name.text = account.name;
        this._account = account;
    }

    public reset() {
        this.disabled = false;
    }

    public disable(load: boolean) {
        this.disabled = true;
        if (load) {
            this.node.addClass('loading');
        }
    }

    private onMouseUp(e: MouseEvent) {
        if (this.disabled) return;
        this.dispatch(this.select, this._account);
    }

    private onAnimationLoop() {
        if (!this.disabled) {
            this.node.removeClass('loading');
        }
    }
}
