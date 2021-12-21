const originalAddEventListener = document.addEventListener;
const originalQuerySelector = document.querySelector;
const blazorElRegex = /^\[_bl_(\w{8}-\w{4}-\w{4}-\w{4}-\w{12}|\d+)\]$/i;

var windows = {};
var eventListeners = [];
var windowManagement = null;

var initialized = false;

export function AssignWindowManagement(windowManagementRef) {
    checkInitialized();

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
    checkInitialized();

    return new Promise(resolve => {
        let win = window.open("/_content/KST.Blazor.Windows/Window.html", id, buildWindowFeatures(windowFeatures));

        windows[id] = win;

        win.addEventListener("load", () => {
            if (windowFeatures.width !== null || windowFeatures.height !== null) {
                let width = windowFeatures.width ?? win.outerWidth;
                let height = windowFeatures.height ?? win.outerHeight;

                if (win.outerWidth !== width || win.outerHeight !== height)
                    win.resizeBy(width - win.outerWidth, height - win.outerHeight);
            }

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
    checkInitialized();

    windows[id].document.title = title;
}

export function CloseWindow(id) {
    checkInitialized();

    windows[id].close();
}

export async function GetMultiScreenWindowPlacementStatus() {
    checkInitialized();

    if ("getScreens" in window || "getScreenDetails" in window) {
        try {
            const { state } = await navigator.permissions.query({ name: "window-placement" });
            if (state === "prompt")
                return "Possible";
            else if (state === "granted")
                return "Allowed";
        } catch (error) {
            console.error(error);
        }
    }

    return "NotPossible";
}

export async function SetMultiScreenWindowPlacement(enabled) {
    checkInitialized();

    if (enabled) {
        let screens;
        try {
            if ("getScreenDetails" in window) {
                screens = await window.getScreenDetails();
            } else {
                screens = await window.getScreens();
            }
        } catch (error) {
            console.error(error);
            await processSingleScreen();
            return;
        }

        if (Array.isArray(screens)) {
            await processScreens(screens);
        } else {
            await processScreens(screens.screens);
            screens.onscreenschange = async function() {
                await processScreens(screens.screens);
            };
        }
    } else {
        await processSingleScreen();
    }
}

function buildWindowFeatures(windowFeaturesObject) {
    if (!windowFeaturesObject.popup)
        return "";

    let windowFeatures = "popup=yes";

    if (windowFeaturesObject.left !== null)
        windowFeatures += `, left=${windowFeaturesObject.left}`;

    if (windowFeaturesObject.top !== null)
        windowFeatures += `, top=${windowFeaturesObject.top}`;

    if (windowFeaturesObject.width !== null)
        windowFeatures += `, width=${windowFeaturesObject.width}`;

    if (windowFeaturesObject.height !== null)
        windowFeatures += `, height=${windowFeaturesObject.height}`;

    return windowFeatures;
}

async function windowClosed(id) {
    delete windows[id];
    if (windowManagement != null) {
        await windowManagement.OnWindowClosed(id);
    }
}

async function processScreens(screens) {
    if (windowManagement != null) {
        await windowManagement.OnScreensChanged(
            screens.map(screen => ({
                'Left': screen.availLeft,
                'Top': screen.availTop,
                'Width': screen.availWidth,
                'Height': screen.availHeight,
                'IsPrimary': screen.isPrimary ?? screen.primary
            }))
        );
    }
}

async function processSingleScreen() {
    if (windowManagement != null) {
        await windowManagement.OnScreensChanged([
            {
                'Left': window.screen.availLeft ?? 0,
                'Top': window.screen.availTop ?? 0,
                'Width': window.screen.availWidth,
                'Height': window.screen.availHeight,
                'IsPrimary': true
            }
        ]);
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

function customQuerySelector(selector) {
    let result = originalQuerySelector.call(this, selector);

    if (result !== null)
        return result;

    if (blazorElRegex.test(selector)) {
        for (let id in windows) {
            if (windows.hasOwnProperty(id)) {
                let element = windows[id].document.querySelector(selector);
                if (element !== null)
                    return element;
            }
        }
    }

    return null;
}

function closeAllWindows() {
    for (let id in windows) {
        if (windows.hasOwnProperty(id)) {
            windows[id].close();
        }
    }
}

function checkInitialized() {
    if (!initialized) {
        throw new Error("Module WindowHandler.js from KST.Blazor.Windows library was not initialized yet");
    }
}

export function Init() {
    if (initialized) {
        throw new Error("Module WindowHandler.js from KST.Blazor.Windows library cannot be initialized twice");
    }
    document.addEventListener = customAddEventListener;
    document.querySelector = customQuerySelector;

    window.addEventListener("unload", closeAllWindows);

    initialized = true;
}
