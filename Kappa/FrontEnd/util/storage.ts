function key(category: string, name: string) {
    return category + '.' + name;
}

export default class Storage {
    private _category: string;

    public constructor(category: string) {
        this._category = category;
    }

    public get(name: string) {
        var raw = localStorage.getItem(key(this._category, name))
        return JSON.parse(raw);
    }

    public set(name: string, value: any) {
        let raw = JSON.stringify(value);
        localStorage.setItem(key(this._category, name), raw);
    }
}