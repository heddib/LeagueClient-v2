import { Swish, $ } from './swish';

export abstract class Custom<T> extends HTMLElement {
    private _refs: T;

    protected get node() { return new Swish(this); }
    protected get refs() { return this._refs; }

    private createdCallback() {
        let content = this.create();

        let callback = (n: Swish) => {
            let ref: string = n.data('ref');
            if (ref) this.refs[ref] = n;

            let events: string = n.data('event');
            if (events) {
                for (let str of events.split(' ')) {
                    let pair = str.split(':');
                    n.on(pair[0], e => this[pair[1]](e, n));
                }
            }
        }
        $(content, '*').do(callback);
        callback(this.node);

        for (var i = 0; i < content.children.length; i++) {
            this.node.add(content.children[i]);
        }
    }

    protected abstract onCreated();
    protected abstract create(): HTMLElement;
}

export function Register(name: string, type) {
    document['registerElement'](name, type);
}