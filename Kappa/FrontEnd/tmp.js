function swish(one, two) {
    if (typeof one != 'string' && !two)
        return new Swish(one);
    var node, query;
    if (one instanceof Swish) {
        node = one[0];
        query = two;
    }
    else if (one instanceof Node && two) {
        node = one;
        query = two;
    }
    else if (one instanceof Array || one instanceof NodeList || one instanceof Node) {
        return new Swish(one);
    }
    else {
        node = document;
        query = one;
    }
    var nodes = node.querySelectorAll(query);
    return new Swish(nodes);
}
;
class Swish {
    constructor(one) {
        if (!one)
            return null;
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
    get length() {
        return this._length;
    }
    get any() {
        return this.length > 0;
    }
    $(...args) {
        var array = [this];
        for (var i = 0; i < args.length; i++)
            array[i + 1] = args[i];
        return swish.apply(window, array);
    }
    do(foreach) {
        for (var i = 0; i < this.length; i++)
            foreach(swish(this[i]));
    }
    doraw(handle) {
        for (var i = 0; i < this.length; i++)
            handle(this[i]);
    }
    map(callback) {
        var nodes = [];
        this.do(n => {
            let q = callback(n);
            if (q instanceof Swish)
                q = q[0];
            nodes.push(q);
        });
        return swish(nodes);
    }
    get where() { return this.filter; }
    filter(callback) {
        if (!callback)
            callback = n => !!n;
        var nodes = [];
        this.do(n => callback(n) ? nodes.push(n[0]) : '');
        return swish(nodes);
    }
    on(e, c) {
        this.doraw((n) => n.addEventListener(e, c));
    }
    off(e, c) {
        this.doraw((n) => n.removeEventListener(e, c));
    }
    css(key, value) {
        if (value !== undefined)
            this.doraw(n => n.style[key] = value);
        else
            return this[0].style[key];
    }
    setBackgroundImage(url) {
        this.css('background-image', `url("${url}")`);
    }
    remove(child) {
        if (child instanceof Swish)
            child = child[0];
        if (child)
            this[0].removeChild(child);
        else
            this.do((n) => n.parent.remove(n[0]));
    }
    empty() {
        this.do((n) => {
            while (n.children.any)
                n.remove(n.firstChild());
        });
    }
    setClass(value, ...name) {
        if (value)
            this.addClass.apply(this, name);
        else
            this.removeClass.apply(this, name);
    }
    addClass(...name) {
        this.doraw((n) => n.classList.add.apply(n.classList, name));
    }
    removeClass(...name) {
        this.doraw((n) => n.classList.remove.apply(n.classList, name));
    }
    scrollToTop() {
        this.doraw((n) => n.scrollTop = 0);
    }
    scrollToBottom() {
        this.doraw((n) => n.scrollTop = n.scrollHeight);
    }
    hasClass(name) {
        return this[0].classList.contains(name);
    }
    get prepend() { return this.firstChild; }
    ;
    firstChild(node) {
        if (node instanceof Swish)
            node = node[0];
        if (node)
            this[0].insertBefore(node, this.firstChild()[0]);
        else
            return this.children.first;
    }
    get append() { return this.lastChild; }
    ;
    get add() { return this.lastChild; }
    ;
    lastChild(node) {
        if (node instanceof Swish)
            node = node[0];
        if (node)
            this[0].appendChild(node);
        else
            return this.children.last;
    }
    insert(node, before) {
        if (node instanceof Swish)
            node = node[0];
        if (typeof before == 'number')
            before = this.children[before];
        this[0].insertBefore(node, before);
    }
    focus() {
        this[0].focus();
    }
    clone(deep) {
        return swish(this[0].cloneNode(deep));
    }
    replace(node) {
        if (node instanceof Swish)
            node = node[0];
        this.parent[0].replaceChild(node, this[0]);
    }
    form(name, value) {
        if (value !== undefined)
            this[0][name].value = value;
        else
            return this[0][name].value;
    }
    data(name, value) {
        if (value !== undefined)
            this[0].setAttribute('data-' + name, value);
        else
            return this[0].getAttribute('data-' + name);
    }
    single(index = 0) {
        return swish(this[index]);
    }
    get children() { return swish(this[0].childNodes).nodes; }
    get parent() { return swish(this[0].parentNode); }
    get classes() { return this[0].classList; }
    get focused() { return this.hasFocus; }
    get hasFocus() {
        var node = document.activeElement;
        while (node && node != this[0])
            node = node.parentNode;
        return node == this[0];
    }
    get first() { return swish(this[0]); }
    get last() { return swish(this[this.length - 1]); }
    get nodes() {
        return this.filter(n => n[0].nodeType == Node.ELEMENT_NODE);
    }
    get array() {
        var a = [];
        this.do(n => a.push(n));
        return a;
    }
    get disabled() { return this[0].disabled; }
    set disabled(v) { this[0].disabled = v; }
    get checked() { return this[0].checked; }
    set checked(v) { this[0].checked = v; }
    get value() { return this[0].value; }
    set value(v) { this[0].value = v; }
    get src() { return this[0].src; }
    set src(v) { this[0].src = v; }
    get id() { return this[0].id; }
    set id(v) { this[0].id = v; }
    get text() { return this[0].innerText; }
    set text(v) { this[0].innerText = v; }
    get html() { return this[0].innerHTML; }
    set html(v) { this[0].innerHTML = v; }
    get dataset() { return this[0].dataset; }
    get bounds() { return this[0].getBoundingClientRect(); }
    get styling() { return window.getComputedStyle(this[0]); }
}
var React;
(function (React) {
    const classPrefix = 'class-';
    const events = {
        onMSContentZoom: 'MSContentZoom',
        onMSGestureChange: 'MSGestureChange',
        onMSGestureDoubleTap: 'MSGestureDoubleTap',
        onMSGestureEnd: 'MSGestureEnd',
        onMSGestureHold: 'MSGestureHold',
        onMSGestureStart: 'MSGestureStart',
        onMSGestureTap: 'MSGestureTap',
        onMSGotPointerCapture: 'MSGotPointerCapture',
        onMSInertiaStart: 'MSInertiaStart',
        onMSLostPointerCapture: 'MSLostPointerCapture',
        onMSManipulationStateChanged: 'MSManipulationStateChanged',
        onMSPointerCancel: 'MSPointerCancel',
        onMSPointerDown: 'MSPointerDown',
        onMSPointerEnter: 'MSPointerEnter',
        onMSPointerLeave: 'MSPointerLeave',
        onMSPointerMove: 'MSPointerMove',
        onMSPointerOut: 'MSPointerOut',
        onMSPointerOver: 'MSPointerOver',
        onMSPointerUp: 'MSPointerUp',
        onAbort: 'abort',
        onActivate: 'activate',
        onAfterprint: 'afterprint',
        onAriarequest: 'ariarequest',
        onBeforeActivate: 'beforeactivate',
        onBeforeCopy: 'beforecopy',
        onBeforeCut: 'beforecut',
        onBeforeDeactivate: 'beforedeactivate',
        onBeforePaste: 'beforepaste',
        onBeforePrint: 'beforeprint',
        onBeforeUnload: 'beforeunload',
        onBlur: 'blur',
        onCanPlay: 'canplay',
        onCanPlayThrough: 'canplaythrough',
        onChange: 'change',
        onClick: 'click',
        onCommand: 'command',
        onContextMenu: 'contextmenu',
        onCopy: 'copy',
        onCueChange: 'cuechange',
        onCut: 'cut',
        onDblClick: 'dblclick',
        onDeactivate: 'deactivate',
        onDrag: 'drag',
        onDragEnd: 'dragend',
        onDragEnter: 'dragenter',
        onDragLeave: 'dragleave',
        onDragOver: 'dragover',
        onDragStart: 'dragstart',
        onDrop: 'drop',
        onDurationChange: 'durationchange',
        onEmptied: 'emptied',
        onEnded: 'ended',
        onError: 'error',
        onFocus: 'focus',
        onGotPointerCapture: 'gotpointercapture',
        onHashChange: 'hashchange',
        onInput: 'input',
        onKeyDown: 'keydown',
        onKeyPress: 'keypress',
        onKeyUp: 'keyup',
        onLoad: 'load',
        onLoadedData: 'loadeddata',
        onLoadedMetadata: 'loadedmetadata',
        onLoadStart: 'loadstart',
        onLostPointerPapture: 'lostpointercapture',
        onMessage: 'message',
        onMouseDown: 'mousedown',
        onMouseEnter: 'mouseenter',
        onMouseLeave: 'mouseleave',
        onMouseNove: 'mousemove',
        onMouseOut: 'mouseout',
        onMouseOver: 'mouseover',
        onMouseUp: 'mouseup',
        onMouseWheel: 'mousewheel',
        onOffline: 'offline',
        onOnline: 'online',
        onOrientationChange: 'orientationchange',
        onPageHide: 'pagehide',
        onPageShow: 'pageshow',
        onPaste: 'paste',
        onPause: 'pause',
        onPlay: 'play',
        onPlaying: 'playing',
        onPointerCancel: 'pointercancel',
        onPointerDown: 'pointerdown',
        onPointerEnter: 'pointerenter',
        onPointerLeave: 'pointerleave',
        onPointerMove: 'pointermove',
        onPointerOut: 'pointerout',
        onPointerOver: 'pointerover',
        onPointerUp: 'pointerup',
        onPopstate: 'popstate',
        onProgress: 'progress',
        onRatechange: 'ratechange',
        onReset: 'reset',
        onResize: 'resize',
        onScroll: 'scroll',
        onSeeked: 'seeked',
        onSeeking: 'seeking',
        onSelect: 'select',
        onSelectStart: 'selectstart',
        onStalled: 'stalled',
        onStorage: 'storage',
        onSubmit: 'submit',
        onSuspend: 'suspend',
        onTimeUpdate: 'timeupdate',
        onTouchCancel: 'touchcancel',
        onTouchEnd: 'touchend',
        onTouchMove: 'touchmove',
        onTouchStart: 'touchstart',
        onUnload: 'unload',
        onVolumeChange: 'volumechange',
        onWaiting: 'waiting',
        onWheel: 'wheel',
    };
    class Component {
        constructor(props) {
            this._props = props;
            this._refs = {};
            this._node = new Swish(this.render().create(this, this.refs));
        }
        get refs() { return this._refs; }
        get node() { return this._node; }
        get props() { return this._props; }
    }
    React.Component = Component;
    class VirtualNode {
        constructor(name, props, children) {
            this.name = name;
            this.props = props;
            this.children = children;
        }
        create(self, refs) {
            let ref;
            if (this.props) {
                ref = this.props['ref'] || this.props['data-ref'];
                delete this.props['ref'];
            }
            if (typeof this.name == 'string') {
                let node = document.createElement(this.name);
                for (let key in this.props) {
                    if (events[key]) {
                        var base = this.props[key].bind(self);
                        node.addEventListener(events[key], e => base(new Swish(e.target), e));
                        continue;
                    }
                    if (key.startsWith(classPrefix)) {
                        var name = key.substring(classPrefix.length);
                        if (this.props[key])
                            node.classList.add(name);
                        continue;
                    }
                    if (key == 'background-image') {
                        node.style.backgroundImage = 'url("' + this.props[key] + '")';
                        continue;
                    }
                    if (key in node.style) {
                        console.log(key);
                    }
                    node.setAttribute(key, this.props[key]);
                }
                for (let child of this.children)
                    addElement(node, child, self, refs);
                if (ref && refs)
                    refs[ref] = new Swish(node);
                return node;
            }
            else {
                let comp = new this.name(this.props);
                if (ref && refs)
                    refs[ref] = comp;
                return comp;
            }
        }
    }
    React.VirtualNode = VirtualNode;
    function createElement(name, props, ...children) {
        return new VirtualNode(name, props, children);
    }
    React.createElement = createElement;
    function template(node) {
        return new Swish(node.create(this, {}));
    }
    React.template = template;
    function addElement(parent, child, self, refs) {
        if (child instanceof HTMLElement) {
            parent.appendChild(child);
            return;
        }
        if (child instanceof VirtualNode) {
            addElement(parent, child.create(self, refs), self, refs);
            return;
        }
        if (child instanceof Component) {
            parent.appendChild(child.node[0]);
            return;
        }
        if (child instanceof Swish) {
            addElement(parent, child[0], self, refs);
            return;
        }
        if (child instanceof Array) {
            for (var sub of child)
                addElement(parent, sub, self, refs);
            return;
        }
        if (['string', 'number'].contains(typeof child)) {
            parent.appendChild(document.createTextNode(child));
            return;
        }
        if (child == undefined || child == null) {
            return;
        }
        debugger;
        console.info(child);
        console.info(child.constructor.name);
    }
})(React || (React = {}));
/** Promise that can resolve multiple times */
class MuiltPromise {
    constructor(promise) {
        this._then = [];
        this._catch = [];
        promise(e => this.resolve(e), e => this.reject(e));
    }
    resolve(t) {
        this._value = t;
        this.hasValue = true;
        this._then.forEach(c => c(this._value));
    }
    reject(e) {
        this._error = e;
        this.hasError = true;
        if (!this._catch.length)
            console.error(e);
        else
            this._catch.forEach(c => c(this._error));
    }
    get value() { return this._value; }
    then(callback) {
        this._then.push(callback);
        if (this.hasValue)
            callback(this._value);
        return this;
    }
    catch(callback) {
        this._catch.push(callback);
        if (this.hasError)
            callback(this._value);
    }
}
class AsyncValue {
    constructor(value) {
        this._id = guid();
        this._handlers = [];
        if (value !== undefined && value !== null)
            this.set(value);
    }
    get id() { return this._id; }
    set(value) {
        this._hasValue = true;
        this._value = value;
        this._handlers.forEach(h => h(this._value));
    }
    get value() { return this._value; }
    on(handler) {
        this._handlers.push(handler);
        if (this._hasValue)
            handler(this._value);
    }
    off(handler) {
        var i = this._handlers.indexOf(handler);
        if (i != -1)
            this._handlers.splice(i, 1);
    }
    single(handler) {
        let wrap = (value) => {
            this.off(wrap);
            handler(value);
        };
        this.on(wrap);
    }
}
class X_Event {
    constructor(create) {
        this._id = guid();
        this._handlers = [];
        create(this.id, this.dispatch);
    }
    get id() { return this._id; }
    on(callback) {
        this._handlers.push(callback);
    }
    off(callback) {
        let i = this._handlers.indexOf(callback);
        if (i != -1)
            this._handlers.splice(i, 1);
    }
    dispatch(t) {
        this._handlers.forEach(h => h(t));
    }
}
class EventSource {
    constructor() {
        this._dispatches = {};
    }
    create() {
        let x = new X_Event((id, dispatch) => this._dispatches[id] = dispatch);
        return x;
    }
    dispatch(x, args) {
        this._dispatches[x.id].call(x, args);
    }
}
class EventModule extends EventSource {
    constructor() {
        super();
    }
    create() { return super.create(); }
    dispatch(x, args) { super.dispatch(x, args); }
}
Array.prototype.orderby = function (handler) {
    let dst = this.slice();
    dst.sort((a, b) => {
        let vA = handler(a);
        let vB = handler(b);
        if (vA > vB)
            return 1;
        if (vA < vB)
            return -1;
        return 0;
    });
    return dst;
};
Array.prototype.select = Array.prototype.map;
Array.prototype.where = Array.prototype.filter;
Array.prototype.first = function (handler) {
    if (!handler)
        return this[0];
    return this[this.firstIndex(handler)];
};
Array.prototype.firstIndex = function (handler) {
    for (let i = 0; i < this.length; i++)
        if (handler(this[i], i, this))
            return i;
    return -1;
};
Array.prototype.any = function (handler) {
    return !!this.first(handler);
};
Array.prototype.contains = function (item) {
    return this.indexOf(item) != -1;
};
Array.prototype.sum = function (handler) {
    let sum = 0;
    for (let i = 0; i < this.length; i++)
        sum += handler(this[i], i, this);
    return sum;
};
Array.prototype.random = function (handler) {
    let filtered = handler ? this.filter(handler) : this;
    return filtered[Math.floor(Math.random() * filtered.length)];
};
function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}
var guid;
(function (guid) {
    guid.empty = '00000000-0000-0000-0000-000000000000';
})(guid || (guid = {}));
var Util;
(function (Util) {
    let preloadContainer;
    let callbacks = {};
    window.addEventListener('load', () => {
        preloadContainer = document.getElementById('preload-container');
        for (let url in callbacks)
            load(url);
    });
    function load(url) {
        let image = new Image();
        preloadContainer.appendChild(image);
        // console.time(url);
        image.src = url;
        image.addEventListener('load', () => {
            // console.timeEnd(url);
            callbacks[url]({});
        });
    }
    function clamp(val, min, max) {
        return Math.min(max, Math.max(min, val));
    }
    Util.clamp = clamp;
    function repeat(str, count) {
        let dst = str;
        for (var i = 0; i < count - 1; i++)
            dst += str;
        return dst;
    }
    Util.repeat = repeat;
    function leftpad(arg, size, padding = ' ') {
        if (padding.length > 1)
            throw 'Padding must be a single character';
        let str = arg.toString();
        if (str.length >= size)
            return str;
        else
            return repeat(padding, size - str.length) + str;
    }
    Util.leftpad = leftpad;
    function rightpad(arg, size, padding = ' ') {
        if (padding.length > 1)
            throw 'Padding must be a single character';
        let str = arg.toString();
        if (str.length >= size)
            return str;
        else
            return str + repeat(padding, size - str.length);
    }
    Util.rightpad = rightpad;
    function datetime(date, format) {
        let replace = (s, a) => format.replace(new RegExp(s + '+', 'g'), s => leftpad(Math.floor(a), s.length, '0'));
        format = replace('s', date.getSeconds());
        format = replace('m', date.getMinutes());
        format = replace('H', date.getHours());
        format = replace('h', (date.getHours() % 12) || 12);
        //replace 0 with 12
        format = replace('d', date.getDate());
        format = replace('M', date.getMonth() + 1);
        return format;
    }
    Util.datetime = datetime;
    function timespan(date, format) {
        let replace = (s, a) => format.replace(new RegExp(s + '+', 'g'), s => leftpad(Math.floor(a), s.length, '0'));
        format = replace('s', (date /= 1000) % 60);
        format = replace('m', (date /= 60) % 60);
        format = replace('H', (date /= 60) % 24);
        format = replace('d', date /= 24);
        return format;
    }
    Util.timespan = timespan;
    function preload(url) {
        return new Promise((resolve, reject) => {
            if (callbacks[url])
                resolve({});
            else {
                callbacks[url] = resolve;
                if (preloadContainer)
                    load(url);
            }
        });
    }
    Util.preload = preload;
})(Util || (Util = {}));
// export class ChatFriend extends Module<Refs> {
//     public selected = this.create<any>();
//     public invited = this.create<number>();
//     private data: any;
//     private inviting: boolean;
//     public constructor() {
//         super(html);
//         this.node.css('display', 'none');
//         this.refs.inviteButton.on('click', () => {
//             this.dispatch(this.invited, this.data.id);
//         });
//     }
//     private onMouseUp() {
//         if (!this.inviting)
//             this.dispatch(this.selected, {});
//     }
//     public get name() { return this.data.name; }
//     public get user() { return this.data.user; }
//     public unread(unread: boolean) {
//         if (unread) {
//             this.node.addClass('unread');
//         } else {
//             this.node.removeClass('unread');
//         }
//     }
//     public icon(id: number) {
//         if (id) this.refs.icon.src = Assets.summoner.icon(id);
//     }
//     public update(data?) {
//         this.data = data;
//         this.tick();
//     }
//     public setInviting(inviting: boolean) {
//         this.inviting = inviting;
//         this.tick();
//     }
//     public tick() {
//         if (!this.data || !this.data.show) {
//             this.node.css('display', 'none');
//         } else {
//             this.node.css('display', null);
//             this.node.removeClass('chat', 'dnd', 'away', 'mobile');
//             this.node.addClass(this.data.show.toLowerCase());
//             if (!this.data.icon) this.data.icon = 1;
//             this.refs.name.text = this.data.name;
//             this.refs.message.text = this.data.message || '';
//             var status = (this.data.status || {}).display;
//             var gametimer = '';
//             if (this.data.show == 'MOBILE') {
//                 var millis = new Date().valueOf() - new Date(this.data.lastonline).valueOf();
//                 var secs = millis / 1000;
//                 var mins = secs / 60;
//                 var hrs = mins / 60;
//                 var days = hrs / 24;
//                 if (days >= 1)
//                     status = 'Offline for ' + Math.floor(days) + ' day' + (days >= 2 ? 's' : '');
//                 else if (hrs >= 1)
//                     status = 'Offline for ' + Math.floor(hrs) + ' hour' + (hrs >= 2 ? 's' : '');
//                 else if (mins >= 1)
//                     status = 'Offline for ' + Math.floor(mins) + ' minute' + (mins >= 2 ? 's' : '');
//                 else
//                     status = 'Offline for ' + Math.floor(secs) + ' second' + (secs >= 2 ? 's' : '');
//             }
//             if (this.data.game) {
//                 var gametimer: string = '';
//                 if (this.data.game.start == 0) {
//                     gametimer = 'Loading';
//                 } else {
//                     var millis = new Date().valueOf() - new Date(this.data.game.start).valueOf();
//                     var seconds: any = Math.floor(millis / 1000) % 60;
//                     let format = Util.timespan(millis, 'm:ss');
//                     gametimer = (this.data.game.exact ? '' : '~') + format;
//                 }
//                 if (this.data.game.type != -1) {
//                     status = 'In ' + Assets.getQueueType(this.data.game.type).display;
//                 }
//             }
//             this.node.setClass(this.inviting && !this.data.game && this.data.show != 'MOBILE', 'inviting');
//             this.refs.status.text = status;
//             this.refs.gametimer.text = gametimer;
//         }
//     }
// } 
/*
function loadPages() {
    let list = popup.$('#mastery-page-list');
    list.empty();
    for (let i = 0; i < currentBook.bookPages.length; i++) {
        let page = currentBook.bookPages[i];
        let node = UI.template(root, 'mastery-page', { name: page.name });
        node.on('click', e => loadPage(page));
        list.add(node);
        if (page.current) {
            node.addClass('active');
        }
    }
}

function loadPage(page?: SpellBookPageDTO) {
    if (page) {
        select(page);
        currentPage = page;
        talents = {};
        for (var i = 0; i < currentPage.talentEntries.length; i++) {
            var entry = currentPage.talentEntries[i];
            talents[entry.talentId] = entry;
        }
    }

    loadPages();

    popup.$('.mastery-icon').do(n => {
        $(n, '.current-points').text = '0';
        n.removeClass('active');
    })

    for (var id in Assets.ddragon.masteries.data) {
        var info = Assets.ddragon.masteries.data[id];
        var node = popup.$('#mastery-icon-' + id);
        var talent = talents[id];
        var rank = 0;
        if (!talent || !talent.rank) {
            node.removeClass('active');
            delete talents[id];
        } else {
            rank = talent.rank;
            node.addClass('active');
        }
        $(node, '.current-points').text = rank;
        $(node, '.description').html = info.description[Math.max(rank - 1, 0)];
    }
}

function onMasteryChange(info, tree, row: number, delta: number) {
    let entry = talents[info.id];
    let changed = (entry ? entry.rank : 0) + delta;

    if (changed > info.ranks || changed < 0)
        return;

    //Steal from other icons in row//
    let currentRow = getRowPoints(tree[row]);
    currentRow.sum += delta;

    //Steal from other icons in row//
    if (currentRow.sum > currentRow.max) {
        let other = tree[row].filter(n => n && n.masteryId != info.id && talents[n.masteryId]);
        talents[other[0].masteryId].rank--;
    }

    if (delta < 0) {
        //Check for masteries in higher rows//
        for (let y = row + 1; y < tree.length; y++) {
            for (let x = 0; x < tree[y].length; x++) {
                if (tree[y][x] && talents[tree[y][x].masteryId])
                    return;
            }
        }
    } else {
        //Check for masteries in lower rows//
        for (let y = 0; y < row; y++) {
            let above = getRowPoints(tree[y]);
            if (above.sum < above.max) return;
        }

        //Check for mastery limit//
        let count = 0;
        for (let key in talents)
            count += talents[key].rank;
        if (count >= 30) return;
    }

    talents[info.id] = <TalentEntry>{ rank: changed, talentId: info.id };

    loadPage();
}

function getRowPoints(row: any[]) {
    var rowSum = 0, rowMax = 0;
    for (var x = 0; x < row.length; x++) {
        if (row[x]) {
            var id = row[x].masteryId;
            var entry = talents[id];
            if (entry) {
                rowSum += entry.rank;
            }
            rowMax = Assets.ddragon.masteries.data[id].ranks;
        }
    }
    return { sum: rowSum, max: rowMax };
}

function createTree(src, dst: Swish) {
    for (let y = 0; y < src.length; y++) {
        let row = UI.template(root, 'mastery-row', {});
        for (let x = 0; x < src[y].length; x++) {
            if (src[y][x]) {
                let info = Assets.ddragon.masteries.data[src[y][x].masteryId];
                let icon = UI.template(root, 'mastery-icon', {
                    id: info.id,
                    iconurl: Assets.image('mastery', info.id),
                    ranks: info.ranks,
                    title: info.name,
                });
                if (info.ranks == 1)
                    icon.addClass('single');
                row.add(icon);
                icon.on('wheel', e => onMasteryChange(info, src, y, -e.deltaY / Math.abs(e.deltaY)));
            } else {
                var blank = UI.template(root, 'mastery-icon', { iconurl: '' });
                blank.addClass('blank');
                row.add(blank);
            }
        }
        dst.add(row);
    }
}

function onSaveMasteriesClick(e: MouseEvent) {
    currentPage.talentEntries = [];
    for (var id in talents) {
        currentPage.talentEntries.push(talents[id]);
    }
    Service.save(currentPage);
    loadPage();
}

function onRevertMasteriesClick(e: MouseEvent) {
    loadPage(currentPage);
}

function onResetMasteriesClick(e: MouseEvent) {
    talents = {};
    loadPage();
}

function onCloseMasteriesClick(e: MouseEvent) {
    popup.removeClass('shown');
    loadPage(currentPage);
    while (callbacks[0])
        callbacks.shift()();
}*/ 
// export class ChatFriend extends Module<Refs> {
//     public selected = this.create<any>();
//     public invited = this.create<number>();
//     private data: any;
//     private inviting: boolean;
//     public constructor() {
//         super(html);
//         this.node.css('display', 'none');
//         this.refs.inviteButton.on('click', () => {
//             this.dispatch(this.invited, this.data.id);
//         });
//     }
//     private onMouseUp() {
//         if (!this.inviting)
//             this.dispatch(this.selected, {});
//     }
//     public get name() { return this.data.name; }
//     public get user() { return this.data.user; }
//     public unread(unread: boolean) {
//         if (unread) {
//             this.node.addClass('unread');
//         } else {
//             this.node.removeClass('unread');
//         }
//     }
//     public icon(id: number) {
//         if (id) this.refs.icon.src = Assets.summoner.icon(id);
//     }
//     public update(data?) {
//         this.data = data;
//         this.tick();
//     }
//     public setInviting(inviting: boolean) {
//         this.inviting = inviting;
//         this.tick();
//     }
//     public tick() {
//         if (!this.data || !this.data.show) {
//             this.node.css('display', 'none');
//         } else {
//             this.node.css('display', null);
//             this.node.removeClass('chat', 'dnd', 'away', 'mobile');
//             this.node.addClass(this.data.show.toLowerCase());
//             if (!this.data.icon) this.data.icon = 1;
//             this.refs.name.text = this.data.name;
//             this.refs.message.text = this.data.message || '';
//             var status = (this.data.status || {}).display;
//             var gametimer = '';
//             if (this.data.show == 'MOBILE') {
//                 var millis = new Date().valueOf() - new Date(this.data.lastonline).valueOf();
//                 var secs = millis / 1000;
//                 var mins = secs / 60;
//                 var hrs = mins / 60;
//                 var days = hrs / 24;
//                 if (days >= 1)
//                     status = 'Offline for ' + Math.floor(days) + ' day' + (days >= 2 ? 's' : '');
//                 else if (hrs >= 1)
//                     status = 'Offline for ' + Math.floor(hrs) + ' hour' + (hrs >= 2 ? 's' : '');
//                 else if (mins >= 1)
//                     status = 'Offline for ' + Math.floor(mins) + ' minute' + (mins >= 2 ? 's' : '');
//                 else
//                     status = 'Offline for ' + Math.floor(secs) + ' second' + (secs >= 2 ? 's' : '');
//             }
//             if (this.data.game) {
//                 var gametimer: string = '';
//                 if (this.data.game.start == 0) {
//                     gametimer = 'Loading';
//                 } else {
//                     var millis = new Date().valueOf() - new Date(this.data.game.start).valueOf();
//                     var seconds: any = Math.floor(millis / 1000) % 60;
//                     let format = Util.timespan(millis, 'm:ss');
//                     gametimer = (this.data.game.exact ? '' : '~') + format;
//                 }
//                 if (this.data.game.type != -1) {
//                     status = 'In ' + Assets.getQueueType(this.data.game.type).display;
//                 }
//             }
//             this.node.setClass(this.inviting && !this.data.game && this.data.show != 'MOBILE', 'inviting');
//             this.refs.status.text = status;
//             this.refs.gametimer.text = gametimer;
//         }
//     }
// } 
