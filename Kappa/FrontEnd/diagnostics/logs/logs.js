const toStringify = [
    'String', 'Guid'
]

$$.on('load', () => {
    $$('#search-input').on('input', () => {
        $$('#log').empty();
        logHistory.forEach(i => {
            if (match(i)) render(i)
        });
    });
    $$('#details').css('display', 'none');
    $$('#exit-button').on('click', () => $$('#details').css('display', 'none'));

    $$('#expand-button').on('click', expand);
    $$('#collapse-button').on('click', collapse);
    connect();
});

function match(item) {
    return item.summary.toLowerCase().indexOf($$('#search-input').value.toLowerCase()) >= 0;
}

/** @type {WebSocket} */
let socket;
let logHistory = [];
let currentNode;
function connect() {
    if (socket) socket.close();
    $$('#log').empty();

    socket = new WebSocket('ws://localhost:25566/log', 'protocolTwo');
    socket.addEventListener('message', e => append(JSON.parse(e.data)));
    $$.http('http://localhost:25566/logs').get(http => {
        var list = http.json.logs;
        for (let i = 0; i < list.length; i++) {
            append(list[i]);
        }
    });
}

function append(item) {
    logHistory.push(item);
    let bottom = $$('#log').parent[0].scrollTop + $$('#log').parent.bounds.height == $$('#log').bounds.height;
    if (match(item)) render(item);
    if (bottom) $$('#log').parent.scrollToBottom();
}

function render(item) {
    let node = template('logitem', item);
    node.addClass(item.category.toLowerCase());
    node.on('click', () => details(item));
    $$('#log').add(node);
}

function details(item) {
    $$('#details').css('display', null);
    $$('#details-name').text = item.summary;

    let toDisplay;
    switch (item.content.type) {
        case 'async':
        case 'async_proxy':
            $$('#details-args').css('display', 'none');
            toDisplay = item.content.body;
            break;

        case 'invoke':
            toDisplay = item.content.return;
            $$('#details-args').css('display', null);
            $$('#details-args').empty();
            for (var arg of item.content.args) {
                let node = $$(document.createElement('div'));
                display(arg, node);
                $$('#details-args').add(node);
            }
            break;
        case 'invoke_proxy':
            toDisplay = item.content.return;
            $$('#details-args').css('display', null);
            $$('#details-args').empty();
            let node = $$(document.createElement('div'));
            display(item.content.args, node);
            $$('#details-args').add(node);
            break;
    }
    currentNode = display(toDisplay, $$('#details-content'));
}

function display(value, node) {
    switch (typeof value) {
        case 'string':
        case 'number':
            node.text = value;
            break;
        default:
            return new PrettyJSON.view.Node({
                el: node[0],
                data: value
            });
    }
}

function expand() {
    if (currentNode) currentNode.expandAll();
}

function collapse() {
    if (currentNode) currentNode.collapseAll();
}