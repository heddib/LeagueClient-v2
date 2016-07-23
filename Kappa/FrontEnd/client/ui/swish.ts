type NodeArg = Node | Swish;

function swish(one: NodeArg | NodeList | Array<Element> | React.Component<any, any> | string, two?: string): Swish {
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

class Swish {
    private _length: number;

    public get length() {
        return this._length;
    }
    public get any() {
        return this.length > 0;
    }

    constructor(one: NodeArg | NodeList | Array<Element> | React.Component<any, any>) {
        if (!one) return null;
        if (one instanceof React.Component) {
            this[0] = one.node[0];
            this._length = 1;
        }
        else if (one instanceof NodeList || one instanceof Array) {
            for (var i = 0; i < one.length; i++)
                this[i] = one[i];
            this._length = one.length;
        }
        else if (one instanceof Node) {
            this[0] = one;
            this._length = 1;
        }
    }

    public $(...args: any[]): Swish {
        var array = [this];
        for (var i = 0; i < args.length; i++)
            array[i + 1] = args[i];
        return swish.apply(window, array);
    }

    public do(foreach: (n: Swish) => void) {
        for (var i = 0; i < this.length; i++)
            foreach(swish(this[i]));
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
        return swish(nodes);
    }

    public get where() { return this.filter; }
    public filter(callback?: (n: Swish) => boolean) {
        if (!callback) callback = n => !!n;
        var nodes: Element[] = [];
        this.do(n => callback(n) ? nodes.push(n[0]) : '');
        return swish(nodes);
    }

    public on(e: string, c: (e: any) => any);
    public on(e: "MSGestureChange", c: (e: MSGestureEvent) => any);
    public on(e: "MSGestureDoubleTap", c: (e: MSGestureEvent) => any);
    public on(e: "MSGestureEnd", c: (e: MSGestureEvent) => any);
    public on(e: "MSGestureHold", c: (e: MSGestureEvent) => any);
    public on(e: "MSGestureStart", c: (e: MSGestureEvent) => any);
    public on(e: "MSGestureTap", c: (e: MSGestureEvent) => any);
    public on(e: "MSInertiaStart", c: (e: MSGestureEvent) => any);
    public on(e: "MSPointerCancel", c: (e: MSPointerEvent) => any);
    public on(e: "MSPointerDown", c: (e: MSPointerEvent) => any);
    public on(e: "MSPointerEnter", c: (e: MSPointerEvent) => any);
    public on(e: "MSPointerLeave", c: (e: MSPointerEvent) => any);
    public on(e: "MSPointerMove", c: (e: MSPointerEvent) => any);
    public on(e: "MSPointerOut", c: (e: MSPointerEvent) => any);
    public on(e: "MSPointerOver", c: (e: MSPointerEvent) => any);
    public on(e: "MSPointerUp", c: (e: MSPointerEvent) => any);
    public on(e: "abort", c: (e: UIEvent) => any);
    public on(e: "afterprint", c: (e: Event) => any);
    public on(e: "beforeprint", c: (e: Event) => any);
    public on(e: "beforeunload", c: (e: BeforeUnloadEvent) => any);
    public on(e: "blur", c: (e: FocusEvent) => any);
    public on(e: "canplay", c: (e: Event) => any);
    public on(e: "canplaythrough", c: (e: Event) => any);
    public on(e: "change", c: (e: Event) => any);
    public on(e: "click", c: (e: MouseEvent) => any);
    public on(e: "compassneedscalibration", c: (e: Event) => any);
    public on(e: "contextmenu", c: (e: PointerEvent) => any);
    public on(e: "dblclick", c: (e: MouseEvent) => any);
    public on(e: "devicemotion", c: (e: DeviceMotionEvent) => any);
    public on(e: "deviceorientation", c: (e: DeviceOrientationEvent) => any);
    public on(e: "drag", c: (e: DragEvent) => any);
    public on(e: "dragend", c: (e: DragEvent) => any);
    public on(e: "dragenter", c: (e: DragEvent) => any);
    public on(e: "dragleave", c: (e: DragEvent) => any);
    public on(e: "dragover", c: (e: DragEvent) => any);
    public on(e: "dragstart", c: (e: DragEvent) => any);
    public on(e: "drop", c: (e: DragEvent) => any);
    public on(e: "durationchange", c: (e: Event) => any);
    public on(e: "emptied", c: (e: Event) => any);
    public on(e: "ended", c: (e: Event) => any);
    public on(e: "focus", c: (e: FocusEvent) => any);
    public on(e: "hashchange", c: (e: HashChangeEvent) => any);
    public on(e: "input", c: (e: Event) => any);
    public on(e: "keydown", c: (e: KeyboardEvent) => any);
    public on(e: "keypress", c: (e: KeyboardEvent) => any);
    public on(e: "keyup", c: (e: KeyboardEvent) => any);
    public on(e: "load", c: (e: Event) => any);
    public on(e: "loadeddata", c: (e: Event) => any);
    public on(e: "loadedmetadata", c: (e: Event) => any);
    public on(e: "loadstart", c: (e: Event) => any);
    public on(e: "message", c: (e: MessageEvent) => any);
    public on(e: "mousedown", c: (e: MouseEvent) => any);
    public on(e: "mouseenter", c: (e: MouseEvent) => any);
    public on(e: "mouseleave", c: (e: MouseEvent) => any);
    public on(e: "mousemove", c: (e: MouseEvent) => any);
    public on(e: "mouseout", c: (e: MouseEvent) => any);
    public on(e: "mouseover", c: (e: MouseEvent) => any);
    public on(e: "mouseup", c: (e: MouseEvent) => any);
    public on(e: "mousewheel", c: (e: MouseWheelEvent) => any);
    public on(e: "offline", c: (e: Event) => any);
    public on(e: "online", c: (e: Event) => any);
    public on(e: "orientationchange", c: (e: Event) => any);
    public on(e: "pagehide", c: (e: PageTransitionEvent) => any);
    public on(e: "pageshow", c: (e: PageTransitionEvent) => any);
    public on(e: "pause", c: (e: Event) => any);
    public on(e: "play", c: (e: Event) => any);
    public on(e: "playing", c: (e: Event) => any);
    public on(e: "pointercancel", c: (e: PointerEvent) => any);
    public on(e: "pointerdown", c: (e: PointerEvent) => any);
    public on(e: "pointerenter", c: (e: PointerEvent) => any);
    public on(e: "pointerleave", c: (e: PointerEvent) => any);
    public on(e: "pointermove", c: (e: PointerEvent) => any);
    public on(e: "pointerout", c: (e: PointerEvent) => any);
    public on(e: "pointerover", c: (e: PointerEvent) => any);
    public on(e: "pointerup", c: (e: PointerEvent) => any);
    public on(e: "popstate", c: (e: PopStateEvent) => any);
    public on(e: "progress", c: (e: ProgressEvent) => any);
    public on(e: "ratechange", c: (e: Event) => any);
    public on(e: "readystatechange", c: (e: ProgressEvent) => any);
    public on(e: "reset", c: (e: Event) => any);
    public on(e: "resize", c: (e: UIEvent) => any);
    public on(e: "scroll", c: (e: UIEvent) => any);
    public on(e: "seeked", c: (e: Event) => any);
    public on(e: "seeking", c: (e: Event) => any);
    public on(e: "select", c: (e: UIEvent) => any);
    public on(e: "stalled", c: (e: Event) => any);
    public on(e: "storage", c: (e: StorageEvent) => any);
    public on(e: "submit", c: (e: Event) => any);
    public on(e: "suspend", c: (e: Event) => any);
    public on(e: "timeupdate", c: (e: Event) => any);
    public on(e: "unload", c: (e: Event) => any);
    public on(e: "volumechange", c: (e: Event) => any);
    public on(e: "waiting", c: (e: Event) => any);
    public on(e: "wheel", c: (e: WheelEvent) => any);
    public on(e: string, c: EventListener) {
        this.doraw((n: Element) => n.addEventListener(e, c as any));
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
        return swish(this[0].cloneNode(deep));
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
        return swish(this[index]);
    }

    public get children() { return swish(this[0].childNodes).nodes; }
    public get parent() { return swish(this[0].parentNode); }

    public get classes() { return this[0].classList; }

    public get focused() { return this.hasFocus; }
    public get hasFocus() {
        var node = document.activeElement;
        while (node && node != this[0])
            node = <Element>node.parentNode;
        return node == this[0];
    }

    public get first() { return swish(this[0]); }
    public get last() { return swish(this[this.length - 1]); }

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