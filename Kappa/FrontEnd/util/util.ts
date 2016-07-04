class Async<T> {
    private _value: T;
    private _error: any;

    private _then: ((T) => void)[] = [];
    private _catch: ((any) => void)[] = [];

    private hasValue: boolean;
    private hasError: boolean;

    public constructor(promise: (resolve: (value: T) => void, reject: (error: any) => void) => void) {
        promise(e => this.resolve(e), e => this.reject(e));
    }

    private resolve(t: T) {
        this._value = t;
        this.hasValue = true;
        this._then.forEach(c => c(this._value));
    }

    private reject(e: any) {
        this._error = e;
        this.hasError = true;
        if (!this._catch.length) console.error(e);
        else this._catch.forEach(c => c(this._error));
    }

    public get value() { return this._value; }

    public then(callback: (value: T) => void) {
        this._then.push(callback);
        if (this.hasValue) callback(this._value);
        return this;
    }

    public catch(callback: (error: any) => void) {
        this._catch.push(callback);
        if (this.hasError) callback(this._value);
    }
}

interface Subscribable<T> {
    id: string,
    on(handler: (value: T) => void);
    off(handler: (value: T) => void);
}

interface IDisposable {
    dispose(): void;
}

class AsyncValue<T> implements Subscribable<T> {
    private _id = guid();
    private _value: T;
    private _hasValue: boolean;

    private _handlers: ((value: T) => void)[] = [];

    public get id() { return this._id; }

    public constructor(value?: T) {
        if (value !== undefined && value !== null)
            this.set(value);
    }

    public set(value: T) {
        this._hasValue = true;
        this._value = value;
        this._handlers.forEach(h => h(this._value));
    }

    public get value() { return this._value; }

    public on(handler: (value: T) => void) {
        this._handlers.push(handler);
        if (this._hasValue) handler(this._value);
    }

    public off(handler: (value: T) => void) {
        var i = this._handlers.indexOf(handler);
        this._handlers.splice(i, 1);
    }

    public single(handler: (value: T) => void) {
        let wrap = (value: T) => {
            this.off(wrap);
            handler(value);
        };
        this.on(wrap);
    }
}

class X_Event<T> implements Subscribable<T> {
    private _id = guid();
    private _handlers: ((t: T) => void)[] = [];

    public get id() { return this._id; }

    constructor(create: (id: string, dispatch: (t: T) => void) => void) {
        create(this.id, this.dispatch);
    }

    public on(callback: (t: T) => void) {
        this._handlers.push(callback);
    }

    public off(callback: (t: T) => void) {
        let i = this._handlers.indexOf(callback);
        if (i != -1) this._handlers.splice(i, 1);
    }

    private dispatch(t: T) {
        this._handlers.forEach(h => h(t));
    }
}

abstract class EventSource {
    private _dispatches: { [e: string]: Function } = {};

    protected create<T>() {
        let x: X_Event<T> = new X_Event<T>((id, dispatch) => this._dispatches[id] = dispatch);
        return x;
    }

    protected dispatch<T>(x: X_Event<T>, args: T) {
        this._dispatches[x.id].call(x, args);
    }
}

class EventModule extends EventSource {
    public constructor() { super(); }
    public create<T>() { return super.create<T>(); }
    public dispatch<T>(x: X_Event<T>, args: T) { super.dispatch(x, args); }
}

interface Array<T> {
    where(handler: (o: T, i: number, a: Array<T>) => void): Array<T>;
    first(handler?: (o: T, i: number, a: Array<T>) => boolean): T;
    firstIndex(handler: (o: T, i: number, a: Array<T>) => boolean): number;
    any(handler?: (o: T, i: number, a: Array<T>) => boolean): boolean;

    sum(handler?: (o: T, i: number, a: Array<T>) => number): number;
}
Array.prototype.where = Array.prototype.filter;
Array.prototype.first = function (handler) {
    if (!handler) return this[0];
    return this[this.firstIndex(handler)];
};
Array.prototype.firstIndex = function (handler) {
    for (let i = 0; i < this.length; i++)
        if (handler(this[i], i, this)) return i;
    return -1;
};
Array.prototype.any = function (handler) {
    return !!this.first(handler);
};
Array.prototype.sum = function (handler) {
    let sum = 0;
    for (let i = 0; i < this.length; i++)
        sum += handler(this[i], i, this);
    return sum;
};

interface IInviteProvider {
    start(): void;
    stop(): void;
}

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}

module guid {
    export const empty = '00000000-0000-0000-0000-000000000000';
}

module React {
    export function createElement(name: string, props: { [id: string]: string }, ...args: any[]) {
        let node = document.createElement(name);
        for (let id in props) node.setAttribute(id, props[id]);
        for (let child of args) {
            if (typeof child == 'string') child = document.createTextNode(child);
            node.appendChild(child);
        }
        return node;
    }
}

module Util {
    export function preload(url: string) {
        let dst = document.getElementById('preload-container');

        return new Async<{}>((resolve, reject) => {
            let image = new Image();
            image.src = url;
            image.addEventListener('load', () => resolve({}));
        });
    }
}