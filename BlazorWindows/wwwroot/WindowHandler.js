const originalAddEventListener = document.addEventListener;

var windows = {};
var eventListeners = [];

export function OpenWindow(id, content, windowFeatures) {
    let win = window.open("about:blank", id, windowFeatures);
    windows[id] = win;

    let baseUrl = new URL(
        document.head.querySelector("base").getAttribute("href"),
        document.location.href
    );

    let baseElement = win.document.createElement("base");
    baseElement.setAttribute("href", baseUrl.href);
    win.document.head.appendChild(baseElement);

    document.head
        .querySelectorAll("link[rel='stylesheet']")
        .forEach(function(item) {
            win.document.head.appendChild(item.cloneNode());
        });

    for (let listener of eventListeners) {
        win.document.addEventListener(listener[0], listener[1], listener[2]);
    }

    win.document.body.appendChild(content);
}

function customAddEventListener(type, listener, options) {
    console.log(type, listener, options);
    eventListeners.push([type, listener, options]);

    for (let id in windows) {
        if (windows.hasOwnProperty(id)) {
            windows[id].document.addEventListener(type, listener, options);
        }
    }

    originalAddEventListener(type, listener, options);
}

function closeAllWindows(e) {
    for (let id in windows) {
        if (windows.hasOwnProperty(id)) {
            windows[id].close();
        }
    }
}

export function Init() {
    document.addEventListener = customAddEventListener;

    window.addEventListener("unload", closeAllWindows);
}
