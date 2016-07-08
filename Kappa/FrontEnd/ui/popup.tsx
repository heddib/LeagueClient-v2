import { Swish, $  } from './swish';
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

abstract class Popup extends Module {
    protected module: Module;

    constructor(title: string, module: Module) {
        super(template);

        this.refs.title.text = title;

        this.module = module;
        module.render(this.refs.container);
        this.render($('#popup-container'));
        $('#popup-container').addClass('shown');

        this.refs.close.on('click', e => this.close());
    }

    public dispose() {
        this.node.remove();

        if (!$('#popup-container').children.any) {
            $('#popup-container').removeClass('shown');
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