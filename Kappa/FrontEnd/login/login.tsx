import http         from './../util/http';
import Module       from './../ui/module';
import * as Assets  from './../assets/assets';
import * as Meta    from './../meta/meta';
import * as Service from './service';

import Account      from './account/account';

const template = (
    <module class="login">
        <img data-ref="image" id="login-img"/>
        <video data-ref="video" id="login-video" autoplay loop></video>
        <div data-ref="accountlist" id="login-accounts"/>
        <div data-ref="form" id="login-form">
            <div id="login-username">
                <div>Username: </div>
                <input data-ref="username" data-event="keydown:onKeyDown" type="text" />
            </div>
            <div id="login-password">
                <div>Password: </div>
                <input data-ref="password" data-event="keydown:onKeyDown" type="password" />
            </div>
            <div id="login-submit">
                <button data-ref="submitbutt" data-event="click:onSubmitClick" type="button">Login</button>
            </div>
            <div data-ref="loader" id="loader"></div>
        </div>
        <div data-ref="switch" class="switch-button">
            <div class="switch-hover">
                <span data-ref="switchText"/>
            </div>
        </div>
    </module>
);

interface Refs {
    image: Swish;
    video: Swish;

    form: Swish;
    loader: Swish;
    username: Swish;
    password: Swish;
    submitbutt: Swish;

    accountlist: Swish;

    switch: Swish;
    switchText: Swish;
}

export default class Page extends Module<Refs> {
    private authHandlers: Function[] = [];
    private loading: boolean;

    private accounts: Account[] = [];

    public auth = this.create<Domain.Authentication.AccountState>();
    public load = this.create<any>();

    public constructor() {
        super(template);

        this.refs.password.focus();

        this.refs.loader.on('animationiteration', e => {
            if (!this.loading) {
                this.refs.submitbutt.disabled = false;
                this.refs.loader.removeClass('loading');
            }
        });

        this.refs.image.on('load', () => this.dispatch(this.load, {}));
        this.refs.video.on('play', () => this.dispatch(this.load, {}));
        this.refs.video.on('error', e => this.refs.image.src = Assets.login.image);
        this.refs.video.src = Assets.login.video;

        Service.saved().then(accounts => {
            if (accounts && accounts.length > 0) {
                this.refs.switch.addClass('visible');
                this.refs.switch.on('click', () => this.change());
                this.change();

                for (let i = 0; i < accounts.length; i++) {
                    let account = new Account(accounts[i]);
                    account.select.on(e => this.onAccountClick(account));
                    this.accounts.push(account);
                    this.refs.accountlist.add(account.node);
                }
            }
        });
    }

    private manual = true;
    private change() {
        this.manual = !this.manual;

        this.refs.switchText.text = this.manual ? 'Saved Accounts' : 'Login';
        this.refs.form.css('display', this.manual ? null : 'none');
        this.refs.accountlist.css('display', this.manual ? 'none' : null);
    }

    private loader(doLoad) {
        if (doLoad) {
            this.refs.submitbutt.disabled = true;
            this.refs.loader.addClass('loading');
        }
        this.loading = doLoad;
    }

    private onKeyDown(e: KeyboardEvent) {
        if (e.keyCode == 13) this.submitForm();
    }

    private onSubmitClick(e: MouseEvent) {
        this.submitForm();
    }

    private submitForm(force: boolean = false) {
        if (!force && this.loading) return;
        Service.auth(this.refs.username.value, this.refs.password.value, true)
            .then(auth => this.onAuth(auth, () => this.submitForm(true)))
            .catch(e => {
                console.error(e);
                this.loader(false);
            });

        this.loader(true);
    }

    private onAccountClick(account: Account, force = false) {
        if (!force && this.loading) return;
        this.loader(true);
        this.accounts.forEach(a => a.disable(false));
        account.disable(true);
        Service.load(account.account.user)
            .then(auth => this.onAuth(auth, () => this.onAccountClick(account, true)));
    }

    private onAuth(auth, callback: Function) {
        switch (auth.status) {
            case 'LOGIN': this.onLogin(auth); break;
            case 'FAILED': this.onFailed(auth); break;
            case 'QUEUE': this.onQueue(auth); break;
        }
    }

    private onLogin(auth) {
        Service.login().then(state => this.dispatch(this.auth, state));
    }

    private onQueue(auth) {
        this.loader(false);
        this.accounts.forEach(a => a.reset());
    }

    private onFailed(auth) {
        console.log('Failed: ' + auth.reason);
        this.loader(false);
        this.accounts.forEach(a => a.reset());
    }
}