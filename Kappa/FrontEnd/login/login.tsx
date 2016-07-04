import http         from './../util/http';
import { Swish }    from './../ui/swish';
import Module       from './../ui/module';
import * as Assets  from './../assets/assets';
import * as Meta    from './../meta/meta';
import * as Service from './service';

import Account      from './account/account';

const template = (
    <module id="login">
        <img data-ref="image" id="login-img"/>
        <video data-ref="video" id="login-video" autoplay loop></video>
        <div data-ref="accountlist" id="login-accounts"></div>
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
    </module>
);

export default class Page extends Module {
    private authHandlers: Function[] = [];
    private loading: boolean;

    private accounts: Account[] = [];

    public auth = this.create<any>();
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
                this.refs.form.css('display', 'none');
                for (let i = 0; i < accounts.length; i++) {
                    let account = new Account(accounts[i]);
                    account.select.on(e => this.onAccountClick(account));
                    this.accounts.push(account);
                    this.refs.accountlist.add(account.node);
                }
            }
        });
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
    }
}