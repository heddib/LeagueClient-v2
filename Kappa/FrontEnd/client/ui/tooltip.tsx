import Module        from './module';

const template = (
    <module class="tooltip">
        <container data-ref="container"/>
    </module>
);

function create(node: Swish, module: Content<any>) {
    let tip = Module.create(template);
    module.render(tip.refs.container);

    node.on('mouseenter', () => {
        tip.render(new Swish(document.body));
        module.onshow();
    });

    node.on('mouseleave', () => {
        tip.node.css('top', null);
        tip.node.css('left', null);
        
        tip.node.remove();
        module.onhide();
    });

    return tip;
}

export function top(node: Swish, module: Content<any>) {
    let tip = create(node, module);

    node.on('mouseenter', () => {
        let centerX = node.bounds.left + node.bounds.width / 2;
        let bottomY = node.bounds.top - 10;

        tip.node.css('top', (bottomY - tip.node.bounds.height) + 'px');
        tip.node.css('left', (centerX) + 'px');
    });
}

export abstract class Content<T> extends Module<T> {
    constructor(template: JSX.Element) {
        super(template);
    }

    public onshow() { }
    public onhide() { }
}