type NodeArg = Node | Swish;

export class Swish {
    private _length: number;

    public get length() {
        return this._length;
    }
    public get any() {
        return this.length > 0;
    }

    constructor(one: NodeArg | NodeList | Array<Element>) {
        if (!one) return null;
        if (one instanceof NodeList || one instanceof Array) {
            for (var i = 0; i < one.length; i++)
                this[i] = one[i];
            this._length = one.length;
        } else if (one instanceof Node) {
            this[0] = one;
            this._length = 1;
        }
    }

    public $(...args: any[]): Swish {
        var array = [this];
        for (var i = 0; i < args.length; i++)
            array[i + 1] = args[i];
        return $.apply(window, array);
    }

    public do(foreach: (n: Swish) => void) {
        for (var i = 0; i < this.length; i++)
            foreach($(this[i]));
    }

    public doraw(handle: (n: Element) => void) {
        for (var i = 0; i < this.length; i++)
            handle(this[i]);
    }

    public map(callback: (n: Swish) => NodeArg) {
        var nodes: Element[] = [];
        this.do(n => {
            let q = callback(n);
            if (q instanceof Swish) q = q[0];
            nodes.push(<any>q);
        });
        return $(nodes);
    }

    public get where() { return this.filter; }
    public filter(callback?: (n: Swish) => boolean) {
        if (!callback) callback = n => !!n;
        var nodes: Element[] = [];
        this.do(n => callback(n) ? nodes.push(n[0]) : '');
        return $(nodes);
    }

    public on(e: string, c: EventListener) {
        this.doraw((n: Element) => n.addEventListener(e, c));
    }

    public off(e: string, c: EventListener) {
        this.doraw((n: Element) => n.removeEventListener(e, c));
    }

    public css(key: string, value?: any) {
        if (value !== undefined)
            this.doraw(n => (<HTMLElement>n).style[key] = value);
        else
            return this[0].style[key];
    }

    public setBackgroundImage(url: string) {
        this.css('background-image', `url("${url}")`);
    }

    public remove(child?: NodeArg) {
        if (child instanceof Swish) child = child[0];
        if (child) this[0].removeChild(child);
        else this.do((n: Swish) => n.parent.remove(n[0]));
    }

    public empty() {
        this.do((n: Swish) => {
            while (n.children.any) n.remove(n.firstChild());
        });
    }

    public setClass(value: boolean, ...name: string[]) {
        if (value) this.addClass.apply(this, name);
        else this.removeClass.apply(this, name);
    }

    public addClass(...name: string[]) {
        this.doraw((n: Element) => n.classList.add.apply(n.classList, name));
    }

    public removeClass(...name: string[]) {
        this.doraw((n: Element) => n.classList.remove.apply(n.classList, name));
    }

    public scrollToTop() {
        this.doraw((n: Element) => n.scrollTop = 0);
    }
    public scrollToBottom() {
        this.doraw((n: Element) => n.scrollTop = n.scrollHeight);
    }

    public hasClass(name: string) {
        return this[0].classList.contains(name);
    }

    public get prepend() { return this.firstChild };
    public firstChild(node?: NodeArg) {
        if (node instanceof Swish) node = node[0];
        if (node) this[0].insertBefore(node, this.firstChild()[0]);
        else return this.children.first;
    }

    public get append() { return this.lastChild };
    public get add() { return this.lastChild };
    public lastChild(node?: NodeArg) {
        if (node instanceof Swish) node = node[0];
        if (node) this[0].appendChild(node);
        else return this.children.last;
    }

    public insert(node: NodeArg, before: number | NodeArg) {
        if (node instanceof Swish) node = node[0];
        if (typeof before == 'number') before = this.children[<number>before];
        this[0].insertBefore(node, before);
    }

    public focus() {
        this[0].focus();
    }
    public clone(deep: boolean) {
        return $(this[0].cloneNode(deep));
    }

    public replace(node: NodeArg) {
        if (node instanceof Swish) node = node[0];
        this.parent[0].replaceChild(node, this[0]);
    }

    public form(name: string, value?) {
        if (value !== undefined) this[0][name].value = value;
        else return this[0][name].value;
    }
    public data(name: string, value?) {
        if (value !== undefined) this[0].setAttribute('data-' + name, value);
        else return this[0].getAttribute('data-' + name);
    }

    public single(index: number = 0) {
        return $(this[index]);
    }

    public get children() { return $(this[0].childNodes).nodes; }
    public get parent() { return $(this[0].parentNode); }

    public get classes() { return this[0].classList; }

    public get focused() { return this.hasFocus; }
    public get hasFocus() {
        var node = document.activeElement;
        while (node && node != this[0])
            node = <Element>node.parentNode;
        return node == this[0];
    }

    public get first() { return $(this[0]); }
    public get last() { return $(this[this.length - 1]); }

    public get nodes() {
        return this.filter(n => n[0].nodeType == Node.ELEMENT_NODE);
    }
    public get array() {
        var a: Swish[] = [];
        this.do(n => a.push(n));
        return a;
    }

    public get disabled() { return this[0].disabled; }
    public set disabled(v) { this[0].disabled = v; }

    public get checked() { return this[0].checked; }
    public set checked(v) { this[0].checked = v; }

    public get value() { return this[0].value; }
    public set value(v) { this[0].value = v; }

    public get src() { return this[0].src; }
    public set src(v) { this[0].src = v; }

    public get id() { return this[0].id; }
    public set id(v) { this[0].id = v; }

    public get text() { return this[0].innerText; }
    public set text(v) { this[0].innerText = v; }

    public get html() { return this[0].innerHTML; }
    public set html(v) { this[0].innerHTML = v; }

    public get dataset() { return this[0].dataset; }
    public get bounds(): ClientRect { return this[0].getBoundingClientRect(); }
    public get styling() { return window.getComputedStyle(this[0]); }
}

export function $(one: NodeArg | NodeList | Array<Element> | string, two?: string): Swish {
    if (typeof one != 'string' && !two) return new Swish(<any>one);
    var node, query;
    if (one instanceof Swish) {
        node = one[0];
        query = two;
    } else if (one instanceof Node && two) {
        node = one;
        query = two;
    } else if (one instanceof Array || one instanceof NodeList || one instanceof Node) {
        return new Swish(one);
    } else {
        node = document;
        query = one;
    }
    var nodes = node.querySelectorAll(query);
    return new Swish(nodes);
};
