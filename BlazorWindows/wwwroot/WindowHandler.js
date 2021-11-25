var windows = { };

export function OpenWindow(id, content, windowFeatures) {
    var win = window.open("about:blank", id, windowFeatures);
    windows[id] = win;
    win.document.body.appendChild(content);
}
