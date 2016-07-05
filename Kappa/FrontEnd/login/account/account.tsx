import { Swish }    from './../../ui/swish';
import Module       from './../../ui/module';
import * as Assets  from './../../assets/assets';

const template = (
    <module class="saved-account">
        <div class="icon" data-ref="icon" data-event="mouseup:onMouseUp">
            <div class="border"/>
        </div>
        <div class="name">
            <span data-ref="name"/>
        </div>
    </module>
);

export default class Account extends Module {
    private disabled: boolean;
    private _account: any;

    public get account() { return this._account; }
    public select = this.create<any>();

    public constructor(account) {
        super(template);

        this.refs.icon.setBackgroundImage(Assets.summoner.icon(account.icon));
        this.refs.name.text = account.name;
        this._account = account;
    }

    public reset() {
        this.disabled = false;
        this.node.removeClass('loading');
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
}
