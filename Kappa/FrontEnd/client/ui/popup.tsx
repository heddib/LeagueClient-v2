import Module        from './module';

const template = (
    <module class="popup">
        <div class="header">
            <span class="title" data-ref="title"/>
            <x-flexpadd/>
            <span class="window-button exit-button" data-ref="close"></span>
        </div>
        <container data-ref="container"/>
    </module>
);

interface Refs {
    title: Swish;
    close: Swish;
    container: Swish;
}

let container;
window.addEventListener('load', e => container = swish('#popup-container'));

abstract class Popup<T extends Module<any>> extends Module<Refs> {
    protected module: T;

    constructor(title: string, module: T) {
        super(template);

        this.refs.title.text = title;

        this.module = module;
        module.render(this.refs.container);
        this.render(container);
        container.addClass('shown');

        this.refs.close.on('click', e => this.close());
    }

    public dispose() {
        this.node.remove();

        if (!container.children.any) {
            container.removeClass('shown');
        }

        this.module.dispose();
    }

    public hide() {
        this.node.removeClass('shown');
    }

    protected close() {
        this.dispose();
    }
}

export default Popup;