$$.on('load', () => {
    $$('#source-input').on('keydown', e => {
        if (e.keyCode == 13) {
            fetch();
        }
    });
    $$('#invoke').css('display', 'none');
    $$('#invoke-button').on('click', invoke);
    $$('#exit-button').on('click', () => $$('#invoke').css('display', 'none'));

    $$('#expand-button').on('click', expand);
    $$('#collapse-button').on('click', collapse);
    fetch();
});

let currentDocs;
let currentEndpoint;
let currentNode;
function fetch() {
    $$.http('/docs').get(http => {
        build(http.json);
    });
}

function build(data) {
    currentDocs = data;
    $$('#list').empty();
    $$('#title').text = data.name;
    var groups = {};
    for (let endpoint of data.endpoints) {
        let node = template('endpoint', endpoint);
        node.on('click', () => show(endpoint));
        if (!endpoint.group)
            endpoint.group = 'Other';
        var group = groups[endpoint.group];
        if (!group) {
            group = groups[endpoint.group] = makeGroup(endpoint.group);
            $$('#list').add(group);
        }
        $$(group, '.endpoint-list').add(node);
    }
}

function show(endpoint) {
    currentEndpoint = endpoint;
    $$('#invoke-list').empty();
    $$('#invoke-result').empty();
    $$('#invoke').css('display', 'block');
    $$('#invoke-name').text = endpoint.path;
    for (let arg of endpoint.args) {
        let node = template('arg', arg);
        $$('#invoke-list').add(node);
    }
}

function invoke() {
    var body = '';
    var args = $$('.arg-input');
    for (var i = 0; i < args.length; i++) {
        if (currentEndpoint.args[i].stringify)
            body += ',"' + args[i].value + '"';
        else
            body += ',' + args[i].value
    }
    body = '[' + body.substring(1) + ']';
    $$.http('http://' + currentDocs.host + currentDocs.base + currentEndpoint.path).post(body, http => {
        switch (typeof http.json.value) {
            case 'string':
            case 'number':
                $$('#invoke-result').text = http.json.value;
                break;
            default:
                currentNode = new PrettyJSON.view.Node({
                    el: jQuery('#invoke-result'),
                    data: http.json.value
                });
        }
    });
}

function expand() {
    if (currentNode) currentNode.expandAll();
}

function collapse() {
    if (currentNode) currentNode.collapseAll();
}

function makeGroup(name) {
    var node = template('group', { name: name });
    var expanded = false;
    $$(node, '.main').on('click', () => {
        if (expanded)
            $$(node, '.expander').css('height', '0');
        else
            $$(node, '.expander').css('height', $$(node, '.endpoint-list').bounds.height + 'px');
        expanded = !expanded;
    });

    return node;
}