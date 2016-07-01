var $$ = (function () {
    const mirror = {
        'disabled': 'disabled',
        'checked': 'checked',
        'value': 'value',
        'src': 'src',
        'id': 'id',
        'bounds': 'getBoundingClientRect',
        'text': 'innerText',
        'html': 'innerHTML',
        'dataset': 'dataset'
    };

    const alias = {
        'prepend': 'firstChild',
        'append': 'lastChild',
        'add': 'lastChild',
        'where': 'filter'
    };

    function wrap(name) {
        return {
            get: function () {
                if (typeof this[0][name] == 'function') return $(this[0][name]());
                else return $(this[0][name]);
            }
        };
    }

    function create(name) {
        return {
            get: function () {
                if (typeof this[0][name] == 'function') return this[0][name]();
                else return this[0][name];
            },
            set: function (value) {
                this.doraw(function (n) {
                    n[name] = value;
                });
            }
        };
    }

    function prop(one, two) {
        switch (typeof one) {
            case "function":
                return {
                    get: one,
                    set: two
                };
            default:
                return {
                    value: one
                };
        }
    }

    function $(one, two) {
        if (!one) return null;
        if (this instanceof $) {
            if (one instanceof NodeList || one instanceof Array) {
                for (var i = 0; i < one.length; i++)
                    this[i] = one[i];
                Object.defineProperty(this, 'length', prop(one.length));
            } else if (one instanceof Node) {
                this[0] = one;
                Object.defineProperty(this, 'length', prop(1));
            }
        } else {
            var node, query;
            if (one instanceof $) {
                node = one[0];
                query = two;
            } else if (one instanceof Node && two) {
                node = one;
                query = two;
            } else if (one instanceof Array || one instanceof NodeList || one instanceof Node) {
                return new $(one);
            } else {
                node = document;
                query = one;
            }
            var nodes = node.querySelectorAll(query);
            return new $(nodes);
        }
    }

    //Set Operators//

    $.prototype.map = function (callback) {
        var nodes = [];
        this.do(function (n) {
            n = callback(n);
            if (nodes[i] instanceof $)
                n = n[0];
            nodes.push(n);
        });
        return $(nodes);
    }
    $.prototype.filter = function (callback) {
        if (!callback) callback = function (n) {
            return n;
        };
        var nodes = [];
        this.do(function (n) {
            if (callback(n)) nodes.push(n[0]);
        });
        return $(nodes);
    }
    $.prototype.do = function (handle) {
        for (var i = 0; i < this.length; i++)
            handle($(this[i]));
    }
    $.prototype.doraw = function (handle) {
        for (var i = 0; i < this.length; i++)
            handle(this[i]);
    }

    $.prototype.on = function (e, c) {
        this.do(function (n) {
            n[0].addEventListener(e, c.bind(n));
        });
    }
    $.prototype.off = function (e, c) {
        this.doraw(function (n) {
            n.removeEventListener(e, c);
        });
    }

    $.prototype.css = function (key, value) {
        if (value !== undefined) this.doraw(function (n) {
            n.style[key] = value;
        });
        else return this[0].style[key];
    }

    $.prototype.remove = function (child) {
        if (child instanceof $) child = child[0];
        if (child) this[0].removeChild(child);
        else this.do(function (n) {
            n.parent.remove(n[0]);
        });
    }
    $.prototype.empty = function () {
        this.do(function (n) {
            while (n.firstChild()) n.firstChild().remove();
        });
    }

    $.prototype.addClass = function (name) {
        this.doraw(function (n) {
            n.classList.add(name);
        });
    };
    $.prototype.removeClass = function (name) {
        this.doraw(function (n) {
            n.classList.remove(name);
        });
    };


    $.prototype.scrollToTop = function () {
        this.doraw(function (n) {
            n.scrollTop = 0;
        });
    }
    $.prototype.scrollToBottom = function () {
        this.doraw(function (n) {
            n.scrollTop = n.scrollHeight;
        });
    }

    //Single Operators//

    $.prototype.hasClass = function (name) {
        return this[0].classList.contains(name);
    };

    $.prototype.firstChild = function (node) {
        if (node instanceof $) node = node[0];
        if (node) this[0].insertBefore(node, this.firstChild()[0]);
        else return $(this[0].firstChild);
    }
    $.prototype.lastChild = function (node) {
        if (node instanceof $) node = node[0];
        if (node) this[0].appendChild(node);
        else return $(this[0].lastChild);
    }
    $.prototype.focus = function () {
        this[0].focus();
    }

    $.prototype.clone = function (deep) {
        return $(this[0].cloneNode(deep));
    }

    $.prototype.replace = function (node) {
        if (node instanceof $) node = node[0];
        this.parent[0].replaceChild(node, this[0]);
    }

    $.prototype.form = function (name, value) {
        if (value !== undefined) {
            var old = this[0][name].value;
            this[0][name].value = value;
            return old;
        }
        else return $(this[0][name]);
    }

    Object.defineProperties($.prototype, {
        children: wrap('childNodes'),
        parent: wrap('parentNode'),
        classes: create('classList'),

        hasFocus: prop(function () {
            var node = document.activeElement;
            while (node && node != this[0])
                node = node.parentNode;
            return node == this[0];
        }),

        first: prop(function () {
            return $(this[0]);
        }),
        single: prop(function () {
            return $(this[0]);
        }),
        last: prop(function () {
            return $(this[this.length - 1]);
        }),

        nodes: prop(function () {
            return this.filter(function (n) {
                return n[0].nodeType == 1;
            });
        }),
        array: prop(function () {
            var a = [];
            this.do(function (n) {
                a.push(n);
            });
            return a;
        }),
    });

    for (var key in mirror)
        Object.defineProperty($.prototype, key, create(mirror[key]));

    for (var key in alias)
        $.prototype[key] = $.prototype[alias[key]];

    $.on = function (e, c) {
        window.addEventListener(e, c);
    }


    function HTTP(url) {
        this._url = url;
        this._base = new XMLHttpRequest();
    }

    function httpOnLoad(resolve, self) {
        self._base.addEventListener('load', function (e) {
            resolve(self);
        });
    }

    HTTP.prototype.get = function (callback) {
        this._base.open('GET', this._url);
        return new Promise(function (resolve, reject) {
            this._base.send();
            httpOnLoad(callback || resolve, this);
        }.bind(this));
    };

    HTTP.prototype.post = function (content, callback) {
        this._base.open('POST', this._url);
        return new Promise(function (resolve, reject) {
            switch (typeof content) {
                case 'string':
                case 'blob':
                    this._base.send(content);
                    break;
                default:
                    this._base.send(JSON.stringify(content));
            }
            httpOnLoad(callback || resolve, this);
        }.bind(this));
    };

    // HTTP.prototype.send = function (data) {
    //   if (!data) this._base.send();
    //   else switch (typeof data) {
    //     case 'string':
    //     case 'blob':
    //       this._base.send(data);
    //       break;
    //     default:
    //       this.send(JSON.stringify(data));
    //   }
    //   return this;
    // };

    Object.defineProperties(HTTP.prototype, {
        'status': {
            get: function () {
                return this._base.status;
            }
        },
        'text': {
            get: function () {
                return this._base.responseText;
            }
        },
        'json': {
            get: function () {
                if (this._json) return this._json;
                return this._json = JSON.parse(this.text);
            }
        }
    });

    $.http = function (url) {
        return new HTTP(url);
    }

    function IMPORT(base) {
        this._base = base;
    }

    IMPORT.prototype.on = function (e, c) {
        this._base.addEventListener(e, c.bind(this));
    }

    Object.defineProperty(IMPORT.prototype, 'content', {
        get: function () {
            return $(this._base.import);
        }
    });

    Object.defineProperty(IMPORT.prototype, 'link', {
        get: function () {
            return $(this._base);
        }
    });

    $.import = function (url) {
        var link = document.createElement('link');
        link.rel = 'import';
        link.href = url;
        document.head.appendChild(link);
        return new IMPORT(link);
    }

    return $;
})();


function template(id, context) {
    var html;
    var all = $$('template');
    for (var i = 0; i < all.length; i++) {
        if (all[i].dataset.template == id) {
            html = all[i].innerHTML.trim();
            break;
        }
    }
    for (var key in context) {
        html = html.replace(new RegExp('{{ *' + key + ' *}}', 'g'), context[key]);
    }
    var div = document.createElement('div');
    div.innerHTML = html;
    return $$(div.firstChild);
}