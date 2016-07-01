import { $ } from './../ui/swish';

export default function http(url: string) {
    return new HttpRequest(url);
}

class HttpRequest {
    private _url: string;
    private _base: XMLHttpRequest;
    private _json: any;

    constructor(url: string) {
        this._url = url;
        this._base = new XMLHttpRequest();
    }

    public get(callback: (http: HttpRequest) => void) {
        this._base.addEventListener('load', () => callback(this));
        this._base.open('GET', this._url);
        this._base.send();
    }

    public post(content: any, callback: (http: HttpRequest) => void) {
        this._base.addEventListener('load', () => callback(this));
        this._base.open('POST', this._url);
        switch (typeof content) {
            case 'string':
            case 'blob':
                this._base.send(content);
                break;
            default:
                this._base.send(JSON.stringify(content));
        }
    }

    public get status() { return this._base.status; }
    public get text() { return this._base.responseText; }
    public get xml() { return $(this._base.responseXML); }
    public get json() {
        if (!this._json) this._json = JSON.parse(this.text);
        return this._json;
    }
}