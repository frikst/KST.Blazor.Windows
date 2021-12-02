const originalAddEventListener = document.addEventListener;

var windows = {};
var eventListeners = [];
var windowManagement = null;

export function AssignWindowManagement(windowManagementRef) {
    windowManagement = windowManagementRef;
}

export function OpenWindow(id, content, windowFeatures, windowTitle) {
    return new Promise(resolve => {
        let win = window.open("/_content/KST.Blazor.Windows/Window.html", id, windowFeatures);

        windows[id] = win;

        win.addEventListener("load", () => {
            win.addEventListener("unload", () => windowClosed(id));

            if (windowTitle !== null)
                win.document.title = windowTitle;

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

            win.document.body.appendChild(content);

            for (let listener of eventListeners) {
                win.document.addEventListener(listener[0], listener[1], listener[2]);
            }

            resolve();
        });
    });
}

function windowClosed(id) {
    delete windows[id];
    if (windowManagement != null) {
        windowManagement.invokeMethodAsync("OnWindowClosed", id);
    }
}

function customAddEventListener(type, listener, options) {
    eventListeners.push([type, listener, options]);

    for (let id in windows) {
        if (windows.hasOwnProperty(id)) {
            windows[id].document.addEventListener(type, listener, options);
        }
    }

    originalAddEventListener(type, listener, options);
}

function closeAllWindows() {
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
