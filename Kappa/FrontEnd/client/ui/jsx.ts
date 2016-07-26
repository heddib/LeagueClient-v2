module React {
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
    }
    const svgs = [
        'svg',
        'circle',
        'clipPath',
        'defs',
        'ellipse',
        'feBlend',
        'feColorMatrix',
        'feComponentTransfer',
        'feComposite',
        'feConvolveMatrix',
        'feDiffuseLighting',
        'feDisplacementMap',
        'feFlood',
        'feGaussianBlur',
        'feImage',
        'feMerge',
        'feMergeNode',
        'feMorphology',
        'feOffset',
        'feSpecularLighting',
        'feTile',
        'feTurbulence',
        'filter',
        'foreignObject',
        'g',
        'image',
        'line',
        'linearGradient',
        'mask',
        'path',
        'pattern',
        'polygon',
        'polyline',
        'radialGradient',
        'rect',
        'stop',
        'symbol',
        'text',
        'tspan',
        'use',

    ]
    export abstract class Component<P, R> {
        private _refs: R;
        private _props: P & Attributes;
        private _node: Swish;

        constructor(props: P) {
            this._props = props;
            this._refs = {} as R;
            this._node = new Swish(this.render().create(this, this.refs));
        }

        protected get refs() { return this._refs; }
        protected abstract render(): JSX.Element;

        get node() { return this._node; }
        get props() { return this._props; }
    }

    export class VirtualNode<P> implements JSX.Element {
        private name: string | Function;
        private props: P;
        private children: any[];

        constructor(name: string | (new (props: P) => Component<P, any>), props: any, children: any[]) {
            this.name = name;
            this.props = props;
            this.children = children;
        }

        create(self: any, refs: any): any {
            let ref;
            if (this.props) {
                ref = this.props['ref'] || this.props['data-ref'];
                delete this.props['ref'];
            }

            if (typeof this.name == 'string') {
                let node;
                if (svgs.contains(this.name as string))
                    node = document.createElementNS('http://www.w3.org/2000/svg', this.name as string);
                else
                    node = document.createElement(this.name as string);

                for (let key in this.props) {
                    if (events[key]) {
                        var base = this.props[key].bind(self);
                        node.addEventListener(events[key], e => base(new Swish(e.target as Node), e));
                        continue;
                    }

                    if (key.startsWith(classPrefix)) {
                        var name = key.substring(classPrefix.length);
                        if (this.props[key]) node.classList.add(name);
                        continue;
                    }

                    node.setAttribute(key, this.props[key]);
                }

                for (let child of this.children)
                    addElement(node, child, self, refs);

                if (ref && refs)
                    refs[ref] = new Swish(node);

                return node;
            } else {
                let comp = new (this.name as any)(this.props) as Component<P, any>;

                if (ref && refs)
                    refs[ref] = comp;

                return comp;
            }
        }
    }

    export function createElement<P>(name: string | (new (props: P) => Component<P, any>), props: P, ...children: any[]) {
        return new VirtualNode(name, props, children);
    }

    export function template(node: VirtualNode<any>) {
        return new Swish(node.create(this, {}));
    }

    function addElement(parent: HTMLElement, child, self, refs) {
        if (child instanceof HTMLElement || child instanceof SVGElement) {
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
            for (var sub of child) addElement(parent, sub, self, refs);
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

    interface SVGAttributes extends HTMLAttributes {
        clipPath?: string;
        cx?: number | string;
        cy?: number | string;
        d?: string;
        dx?: number | string;
        dy?: number | string;
        fill?: string;
        fillOpacity?: number | string;
        fontFamily?: string;
        fontSize?: number | string;
        fx?: number | string;
        fy?: number | string;
        gradientTransform?: string;
        gradientUnits?: string;
        markerEnd?: string;
        markerMid?: string;
        markerStart?: string;
        offset?: number | string;
        opacity?: number | string;
        patternContentUnits?: string;
        patternUnits?: string;
        points?: string;
        preserveAspectRatio?: string;
        r?: number | string;
        rx?: number | string;
        ry?: number | string;
        spreadMethod?: string;
        stopColor?: string;
        stopOpacity?: number | string;
        stroke?: string;
        strokeDasharray?: string;
        strokeLinecap?: string;
        strokeMiterlimit?: string;
        strokeOpacity?: number | string;
        strokeWidth?: number | string;
        textAnchor?: string;
        transform?: string;
        version?: string;
        viewBox?: string;
        x1?: number | string;
        x2?: number | string;
        x?: number | string;
        xlinkActuate?: string;
        xlinkArcrole?: string;
        xlinkHref?: string;
        xlinkRole?: string;
        xlinkShow?: string;
        xlinkTitle?: string;
        xlinkType?: string;
        xmlBase?: string;
        xmlLang?: string;
        xmlSpace?: string;
        y1?: number | string;
        y2?: number | string;
        y?: number | string;
    }

    interface HTMLAttributes extends Attributes, EventAttributes {
        // React-specific Attributes
        defaultChecked?: boolean;
        defaultValue?: string | string[];

        // Standard HTML Attributes
        accept?: string;
        acceptCharset?: string;
        accessKey?: string;
        action?: string;
        allowFullScreen?: boolean;
        allowTransparency?: boolean;
        alt?: string;
        async?: boolean;
        autoComplete?: string;
        autoFocus?: boolean;
        autoPlay?: boolean;
        capture?: boolean;
        cellPadding?: number | string;
        cellSpacing?: number | string;
        charSet?: string;
        challenge?: string;
        checked?: boolean;
        classID?: string;
        class?: string;
        cols?: number;
        colSpan?: number;
        content?: string;
        contentEditable?: boolean;
        contextMenu?: string;
        controls?: boolean;
        coords?: string;
        crossOrigin?: string;
        data?: string;
        dateTime?: string;
        default?: boolean;
        defer?: boolean;
        dir?: string;
        disabled?: boolean;
        download?: any;
        draggable?: boolean;
        encType?: string;
        form?: string;
        formAction?: string;
        formEncType?: string;
        formMethod?: string;
        formNoValidate?: boolean;
        formTarget?: string;
        frameBorder?: number | string;
        headers?: string;
        height?: number | string;
        hidden?: boolean;
        high?: number;
        href?: string;
        hrefLang?: string;
        htmlFor?: string;
        httpEquiv?: string;
        icon?: string;
        id?: string;
        inputMode?: string;
        integrity?: string;
        is?: string;
        keyParams?: string;
        keyType?: string;
        kind?: string;
        label?: string;
        lang?: string;
        list?: string;
        loop?: boolean;
        low?: number;
        manifest?: string;
        marginHeight?: number;
        marginWidth?: number;
        max?: number | string;
        maxLength?: number;
        media?: string;
        mediaGroup?: string;
        method?: string;
        min?: number | string;
        minLength?: number;
        multiple?: boolean;
        muted?: boolean;
        name?: string;
        nonce?: string;
        noValidate?: boolean;
        open?: boolean;
        optimum?: number;
        pattern?: string;
        placeholder?: string;
        poster?: string;
        preload?: string;
        radioGroup?: string;
        readOnly?: boolean;
        rel?: string;
        required?: boolean;
        reversed?: boolean;
        role?: string;
        rows?: number;
        rowSpan?: number;
        sandbox?: string;
        scope?: string;
        scoped?: boolean;
        scrolling?: string;
        seamless?: boolean;
        selected?: boolean;
        shape?: string;
        size?: number;
        sizes?: string;
        span?: number;
        spellCheck?: boolean;
        src?: string;
        srcDoc?: string;
        srcLang?: string;
        srcSet?: string;
        start?: number;
        step?: number | string;
        style?: string;
        summary?: string;
        tabIndex?: number;
        target?: string;
        title?: string;
        type?: string;
        useMap?: string;
        value?: string | string[];
        width?: number | string;
        wmode?: string;
        wrap?: string;

        // RDFa Attributes
        about?: string;
        datatype?: string;
        inlist?: any;
        prefix?: string;
        property?: string;
        resource?: string;
        typeof?: string;
        vocab?: string;

        // Non-standard Attributes
        autoCapitalize?: string;
        autoCorrect?: string;
        autoSave?: string;
        color?: string;
        itemProp?: string;
        itemScope?: boolean;
        itemType?: string;
        itemID?: string;
        itemRef?: string;
        results?: number;
        security?: string;
        unselectable?: boolean;

        // Allows aria- and data- Attributes
        [key: string]: any;
    }

    interface Attributes {
        ref?: string;
    }

    declare type ReactEventHandler<T> = (sender: Swish | Component<any, any>, event: T) => void;

    interface EventAttributes {
        onMSContentZoom?: ReactEventHandler<UIEvent>,
        onMSGestureChange?: ReactEventHandler<MSGestureEvent>,
        onMSGestureDoubleTap?: ReactEventHandler<MSGestureEvent>,
        onMSGestureEnd?: ReactEventHandler<MSGestureEvent>,
        onMSGestureHold?: ReactEventHandler<MSGestureEvent>,
        onMSGestureStart?: ReactEventHandler<MSGestureEvent>,
        onMSGestureTap?: ReactEventHandler<MSGestureEvent>,
        onMSGotPointerCapture?: ReactEventHandler<MSPointerEvent>,
        onMSInertiaStart?: ReactEventHandler<MSGestureEvent>,
        onMSLostPointerCapture?: ReactEventHandler<MSPointerEvent>,
        onMSManipulationStateChanged?: ReactEventHandler<MSManipulationEvent>,
        onMSPointerCancel?: ReactEventHandler<MSPointerEvent>,
        onMSPointerDown?: ReactEventHandler<MSPointerEvent>,
        onMSPointerEnter?: ReactEventHandler<MSPointerEvent>,
        onMSPointerLeave?: ReactEventHandler<MSPointerEvent>,
        onMSPointerMove?: ReactEventHandler<MSPointerEvent>,
        onMSPointerOut?: ReactEventHandler<MSPointerEvent>,
        onMSPointerOver?: ReactEventHandler<MSPointerEvent>,
        onMSPointerUp?: ReactEventHandler<MSPointerEvent>,
        onAbort?: ReactEventHandler<UIEvent>,
        onActivate?: ReactEventHandler<UIEvent>,
        onAfterprint?: ReactEventHandler<Event>,
        onAriarequest?: ReactEventHandler<AriaRequestEvent>,
        onBeforeActivate?: ReactEventHandler<UIEvent>,
        onBeforeCopy?: ReactEventHandler<DragEvent>,
        onBeforeCut?: ReactEventHandler<DragEvent>,
        onBeforeDeactivate?: ReactEventHandler<UIEvent>,
        onBeforePaste?: ReactEventHandler<DragEvent>,
        onBeforePrint?: ReactEventHandler<Event>,
        onBeforeUnload?: ReactEventHandler<BeforeUnloadEvent>,
        onBlur?: ReactEventHandler<FocusEvent>,
        onCanPlay?: ReactEventHandler<Event>,
        onCanPlayThrough?: ReactEventHandler<Event>,
        onChange?: ReactEventHandler<Event>,
        onClick?: ReactEventHandler<MouseEvent>,
        onCommand?: ReactEventHandler<CommandEvent>,
        onContextMenu?: ReactEventHandler<PointerEvent>,
        onCopy?: ReactEventHandler<DragEvent>,
        onCueChange?: ReactEventHandler<Event>,
        onCut?: ReactEventHandler<DragEvent>,
        onDblClick?: ReactEventHandler<MouseEvent>,
        onDeactivate?: ReactEventHandler<UIEvent>,
        onDrag?: ReactEventHandler<DragEvent>,
        onDragEnd?: ReactEventHandler<DragEvent>,
        onDragEnter?: ReactEventHandler<DragEvent>,
        onDragLeave?: ReactEventHandler<DragEvent>,
        onDragOver?: ReactEventHandler<DragEvent>,
        onDragStart?: ReactEventHandler<DragEvent>,
        onDrop?: ReactEventHandler<DragEvent>,
        onDurationChange?: ReactEventHandler<Event>,
        onEmptied?: ReactEventHandler<Event>,
        onEnded?: ReactEventHandler<Event>,
        onError?: ReactEventHandler<ErrorEvent>,
        onFocus?: ReactEventHandler<FocusEvent>,
        onGotPointerCapture?: ReactEventHandler<PointerEvent>,
        onHashChange?: ReactEventHandler<HashChangeEvent>,
        onInput?: ReactEventHandler<Event>,
        onKeyDown?: ReactEventHandler<KeyboardEvent>,
        onKeyPress?: ReactEventHandler<KeyboardEvent>,
        onKeyUp?: ReactEventHandler<KeyboardEvent>,
        onLoad?: ReactEventHandler<Event>,
        onLoadedData?: ReactEventHandler<Event>,
        onLoadedMetadata?: ReactEventHandler<Event>,
        onLoadStart?: ReactEventHandler<Event>,
        onLostPointerPapture?: ReactEventHandler<PointerEvent>,
        onMessage?: ReactEventHandler<MessageEvent>,
        onMouseDown?: ReactEventHandler<MouseEvent>,
        onMouseEnter?: ReactEventHandler<MouseEvent>,
        onMouseLeave?: ReactEventHandler<MouseEvent>,
        onMouseNove?: ReactEventHandler<MouseEvent>,
        onMouseOut?: ReactEventHandler<MouseEvent>,
        onMouseOver?: ReactEventHandler<MouseEvent>,
        onMouseUp?: ReactEventHandler<MouseEvent>,
        onMouseWheel?: ReactEventHandler<MouseWheelEvent>,
        onOffline?: ReactEventHandler<Event>,
        onOnline?: ReactEventHandler<Event>,
        onOrientationChange?: ReactEventHandler<Event>,
        onPageHide?: ReactEventHandler<PageTransitionEvent>,
        onPageShow?: ReactEventHandler<PageTransitionEvent>,
        onPaste?: ReactEventHandler<DragEvent>,
        onPause?: ReactEventHandler<Event>,
        onPlay?: ReactEventHandler<Event>,
        onPlaying?: ReactEventHandler<Event>,
        onPointerCancel?: ReactEventHandler<PointerEvent>,
        onPointerDown?: ReactEventHandler<PointerEvent>,
        onPointerEnter?: ReactEventHandler<PointerEvent>,
        onPointerLeave?: ReactEventHandler<PointerEvent>,
        onPointerMove?: ReactEventHandler<PointerEvent>,
        onPointerOut?: ReactEventHandler<PointerEvent>,
        onPointerOver?: ReactEventHandler<PointerEvent>,
        onPointerUp?: ReactEventHandler<PointerEvent>,
        onPopstate?: ReactEventHandler<PopStateEvent>,
        onProgress?: ReactEventHandler<ProgressEvent>,
        onRatechange?: ReactEventHandler<Event>,
        onReset?: ReactEventHandler<Event>,
        onResize?: ReactEventHandler<UIEvent>,
        onScroll?: ReactEventHandler<UIEvent>,
        onSeeked?: ReactEventHandler<Event>,
        onSeeking?: ReactEventHandler<Event>,
        onSelect?: ReactEventHandler<UIEvent>,
        onSelectStart?: ReactEventHandler<Event>,
        onStalled?: ReactEventHandler<Event>,
        onStorage?: ReactEventHandler<StorageEvent>,
        onSubmit?: ReactEventHandler<Event>,
        onSuspend?: ReactEventHandler<Event>,
        onTimeUpdate?: ReactEventHandler<Event>,
        onTouchCancel?: ReactEventHandler<TouchEvent>,
        onTouchEnd?: ReactEventHandler<TouchEvent>,
        onTouchMove?: ReactEventHandler<TouchEvent>,
        onTouchStart?: ReactEventHandler<TouchEvent>,
        onUnload?: ReactEventHandler<Event>,
        onVolumeChange?: ReactEventHandler<Event>,
        onWaiting?: ReactEventHandler<Event>,
        onWheel?: ReactEventHandler<WheelEvent>,
    }

    export interface SVGProps extends SVGAttributes { }
    export interface HTMLProps<T> extends HTMLAttributes { }
}

declare namespace JSX {
    interface Element extends React.VirtualNode<any> { }
    interface ElementClass extends React.Component<any, any> { }

    interface ElementAttributesProperty { props: {}; }

    interface IntrinsicElements {
        module: React.HTMLProps<HTMLDivElement>;
        container: React.HTMLProps<HTMLDivElement>;
        'x-flexpadd': React.HTMLProps<HTMLDivElement>;
        // HTML
        a: React.HTMLProps<HTMLAnchorElement>;
        abbr: React.HTMLProps<HTMLElement>;
        address: React.HTMLProps<HTMLElement>;
        area: React.HTMLProps<HTMLAreaElement>;
        article: React.HTMLProps<HTMLElement>;
        aside: React.HTMLProps<HTMLElement>;
        audio: React.HTMLProps<HTMLAudioElement>;
        b: React.HTMLProps<HTMLElement>;
        base: React.HTMLProps<HTMLBaseElement>;
        bdi: React.HTMLProps<HTMLElement>;
        bdo: React.HTMLProps<HTMLElement>;
        big: React.HTMLProps<HTMLElement>;
        blockquote: React.HTMLProps<HTMLElement>;
        body: React.HTMLProps<HTMLBodyElement>;
        br: React.HTMLProps<HTMLBRElement>;
        button: React.HTMLProps<HTMLButtonElement>;
        canvas: React.HTMLProps<HTMLCanvasElement>;
        caption: React.HTMLProps<HTMLElement>;
        cite: React.HTMLProps<HTMLElement>;
        code: React.HTMLProps<HTMLElement>;
        col: React.HTMLProps<HTMLTableColElement>;
        colgroup: React.HTMLProps<HTMLTableColElement>;
        data: React.HTMLProps<HTMLElement>;
        datalist: React.HTMLProps<HTMLDataListElement>;
        dd: React.HTMLProps<HTMLElement>;
        del: React.HTMLProps<HTMLElement>;
        details: React.HTMLProps<HTMLElement>;
        dfn: React.HTMLProps<HTMLElement>;
        dialog: React.HTMLProps<HTMLElement>;
        div: React.HTMLProps<HTMLDivElement>;
        dl: React.HTMLProps<HTMLDListElement>;
        dt: React.HTMLProps<HTMLElement>;
        em: React.HTMLProps<HTMLElement>;
        embed: React.HTMLProps<HTMLEmbedElement>;
        fieldset: React.HTMLProps<HTMLFieldSetElement>;
        figcaption: React.HTMLProps<HTMLElement>;
        figure: React.HTMLProps<HTMLElement>;
        footer: React.HTMLProps<HTMLElement>;
        form: React.HTMLProps<HTMLFormElement>;
        h1: React.HTMLProps<HTMLHeadingElement>;
        h2: React.HTMLProps<HTMLHeadingElement>;
        h3: React.HTMLProps<HTMLHeadingElement>;
        h4: React.HTMLProps<HTMLHeadingElement>;
        h5: React.HTMLProps<HTMLHeadingElement>;
        h6: React.HTMLProps<HTMLHeadingElement>;
        head: React.HTMLProps<HTMLHeadElement>;
        header: React.HTMLProps<HTMLElement>;
        hgroup: React.HTMLProps<HTMLElement>;
        hr: React.HTMLProps<HTMLHRElement>;
        html: React.HTMLProps<HTMLHtmlElement>;
        i: React.HTMLProps<HTMLElement>;
        iframe: React.HTMLProps<HTMLIFrameElement>;
        img: React.HTMLProps<HTMLImageElement>;
        input: React.HTMLProps<HTMLInputElement>;
        ins: React.HTMLProps<HTMLModElement>;
        kbd: React.HTMLProps<HTMLElement>;
        keygen: React.HTMLProps<HTMLElement>;
        label: React.HTMLProps<HTMLLabelElement>;
        legend: React.HTMLProps<HTMLLegendElement>;
        li: React.HTMLProps<HTMLLIElement>;
        link: React.HTMLProps<HTMLLinkElement>;
        main: React.HTMLProps<HTMLElement>;
        map: React.HTMLProps<HTMLMapElement>;
        mark: React.HTMLProps<HTMLElement>;
        menu: React.HTMLProps<HTMLElement>;
        menuitem: React.HTMLProps<HTMLElement>;
        meta: React.HTMLProps<HTMLMetaElement>;
        meter: React.HTMLProps<HTMLElement>;
        nav: React.HTMLProps<HTMLElement>;
        noscript: React.HTMLProps<HTMLElement>;
        object: React.HTMLProps<HTMLObjectElement>;
        ol: React.HTMLProps<HTMLOListElement>;
        optgroup: React.HTMLProps<HTMLOptGroupElement>;
        option: React.HTMLProps<HTMLOptionElement>;
        output: React.HTMLProps<HTMLElement>;
        p: React.HTMLProps<HTMLParagraphElement>;
        param: React.HTMLProps<HTMLParamElement>;
        picture: React.HTMLProps<HTMLElement>;
        pre: React.HTMLProps<HTMLPreElement>;
        progress: React.HTMLProps<HTMLProgressElement>;
        q: React.HTMLProps<HTMLQuoteElement>;
        rp: React.HTMLProps<HTMLElement>;
        rt: React.HTMLProps<HTMLElement>;
        ruby: React.HTMLProps<HTMLElement>;
        s: React.HTMLProps<HTMLElement>;
        samp: React.HTMLProps<HTMLElement>;
        script: React.HTMLProps<HTMLElement>;
        section: React.HTMLProps<HTMLElement>;
        select: React.HTMLProps<HTMLSelectElement>;
        small: React.HTMLProps<HTMLElement>;
        source: React.HTMLProps<HTMLSourceElement>;
        span: React.HTMLProps<HTMLSpanElement>;
        strong: React.HTMLProps<HTMLElement>;
        style: React.HTMLProps<HTMLStyleElement>;
        sub: React.HTMLProps<HTMLElement>;
        summary: React.HTMLProps<HTMLElement>;
        sup: React.HTMLProps<HTMLElement>;
        table: React.HTMLProps<HTMLTableElement>;
        tbody: React.HTMLProps<HTMLTableSectionElement>;
        td: React.HTMLProps<HTMLTableDataCellElement>;
        textarea: React.HTMLProps<HTMLTextAreaElement>;
        tfoot: React.HTMLProps<HTMLTableSectionElement>;
        th: React.HTMLProps<HTMLTableHeaderCellElement>;
        thead: React.HTMLProps<HTMLTableSectionElement>;
        time: React.HTMLProps<HTMLElement>;
        title: React.HTMLProps<HTMLTitleElement>;
        tr: React.HTMLProps<HTMLTableRowElement>;
        track: React.HTMLProps<HTMLTrackElement>;
        u: React.HTMLProps<HTMLElement>;
        ul: React.HTMLProps<HTMLUListElement>;
        "var": React.HTMLProps<HTMLElement>;
        video: React.HTMLProps<HTMLVideoElement>;
        wbr: React.HTMLProps<HTMLElement>;

        // SVG
        svg: React.SVGProps;

        circle: React.SVGProps;
        clipPath: React.SVGProps;
        defs: React.SVGProps;
        ellipse: React.SVGProps;
        feBlend: React.SVGProps;
        feColorMatrix: React.SVGProps;
        feComponentTransfer: React.SVGProps;
        feComposite: React.SVGProps;
        feConvolveMatrix: React.SVGProps;
        feDiffuseLighting: React.SVGProps;
        feDisplacementMap: React.SVGProps;
        feFlood: React.SVGProps;
        feGaussianBlur: React.SVGProps;
        feImage: React.SVGProps;
        feMerge: React.SVGProps;
        feMergeNode: React.SVGProps;
        feMorphology: React.SVGProps;
        feOffset: React.SVGProps;
        feSpecularLighting: React.SVGProps;
        feTile: React.SVGProps;
        feTurbulence: React.SVGProps;
        filter: React.SVGProps;
        foreignObject: React.SVGProps;
        g: React.SVGProps;
        image: React.SVGProps;
        line: React.SVGProps;
        linearGradient: React.SVGProps;
        mask: React.SVGProps;
        path: React.SVGProps;
        pattern: React.SVGProps;
        polygon: React.SVGProps;
        polyline: React.SVGProps;
        radialGradient: React.SVGProps;
        rect: React.SVGProps;
        stop: React.SVGProps;
        symbol: React.SVGProps;
        text: React.SVGProps;
        tspan: React.SVGProps;
        use: React.SVGProps;
    }
}
