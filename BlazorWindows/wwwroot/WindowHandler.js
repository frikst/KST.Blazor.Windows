const originalAddEventListener = document.addEventListener;

var windows = {};
var eventListeners = [];
var windowManagement = null;

export function AssignWindowManagement(windowManagementRef) {
    windowManagement = {
        'OnWindowClosed': async function(id) {
            await windowManagementRef.invokeMethodAsync("OnWindowClosed", id);
        },
        'OnScreensChanged': async function(screens) {
            await windowManagementRef.invokeMethodAsync("OnScreensChanged", screens);
        }
    };
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

export function ChangeWindowTitle(id, title) {
    windows[id].document.title = title;
}

export async function SetMultiScreenWindowPlacement(enabled) {
    if (!enabled) {
        processSingleScreen();
    } else if ("getScreens" in window) {
        var denied;
        try {
            const { state } = await navigator.permissions.query({ name: "window-placement" });
            denied = state === "denied";
        } catch (error) {
            console.error(error);
            denied = true;
        }

        if (denied) {
            processSingleScreen();
        } else {
            const screens = await window.getScreens();
            processScreens(screens.screens);
            screens.onscreenschange = function() {
                processScreens(screens.screens);
            };
        }
    } else {
        processSingleScreen();
    }
}

async function windowClosed(id) {
    delete windows[id];
    if (windowManagement != null) {
        await windowManagement.OnWindowClosed(id);
    }
}

function processScreens(screens) {
    windowManagement.OnScreensChanged(
        screens.map(screen => ({
            'Left': screen.availLeft,
            'Top': screen.availTop,
            'Width': screen.availWidth,
            'Height': screen.availHeight,
            'IsPrimary': screen.isPrimary
        }))
    );
}

function processSingleScreen() {
    windowManagement.OnScreensChanged([
        {
            'Left': 'availLeft' in window.screen ? window.screen.availLeft : 0,
            'Top': 'availTop' in window.screen ? window.screen.availTop : 0,
            'Width': window.screen.availWidth,
            'Height': window.screen.availHeight,
            'IsPrimary': true
        }
    ]);
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
