declare var ClientWindow: {
    minimize: () => void,
    focus: () => void,
    close: () => void,
    show: () => void,
    resize: (w, h) => void
};

export function close() {
    ClientWindow.close();
}

export function minimize() {
    ClientWindow.minimize();
}

export function focus() {
    ClientWindow.focus();
}

export function show() {
    ClientWindow.show();
}

export function resize(w: number, h: number) {
    ClientWindow.resize(w, h);
}

export default ClientWindow;