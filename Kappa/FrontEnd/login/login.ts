import http         from './../util/http';
import { Swish }    from './../ui/swish';
import Module       from './../ui/module';
import * as Assets  from './../assets/assets';
import * as Meta    from './../meta/meta';
import * as Service from './service';

import Account      from './account/account';

const html = Module.import('login');

export default class Page extends Module {
    private authHandlers: Function[] = [];
    private loading: boolean;

    private accounts: Account[] = [];

    public auth = this.create<{}>();
    public load = this.create<{}>();

    public constructor() {
        super(html);

        this.refs.password.focus();

        this.refs.loader.on('animationiteration', e => {
            if (!this.loading) {
                this.refs.submitbutt.disabled = false;
                this.refs.loader.removeClass('loading');
            }
        });

        Assets.onload(() => {
            this.refs.image.on('load', () => this.dispatch(this.load, {}));
            this.refs.video.on('play', () => this.dispatch(this.load, {}));
            this.refs.video.on('error', e => this.refs.image.src = Assets.login.image);
            this.refs.video.src = Assets.login.video;
        });

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
        if (auth.status == 'LOGIN') {
            Service.login()
                .then(state => this.dispatch(this.auth, state))
                .catch(e => setTimeout(() => this.onAuth(auth, callback), 500));
        } else {
            console.log('Login: ' + auth.status);
            if (auth.status == 'BUSY' || auth.status == 'QUEUE') {
                setTimeout(() => callback(), 500);
            } else {
                console.log(auth);
            }
        }
    }
}