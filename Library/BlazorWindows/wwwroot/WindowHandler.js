const originalAddEventListener = document.addEventListener;
const originalQuerySelector = document.querySelector;
const blazorElRegex = /^\[_bl_(\w{8}-\w{4}-\w{4}-\w{4}-\w{12}|\d+)\]$/i;

var windows = {};
var eventListeners = [];
var windowManagement = null;
var screenDetails = null;

var initialized = false;

export function AssignWindowManagement(windowManagementRef) {
    checkInitialized();

    windowManagement = {
        'OnWindowClosed': async function(id) {
            await windowManagementRef.invokeMethodAsync("OnWindowClosed", id);
        },
        'OnScreensChanged': async function(screens) {
            await windowManagementRef.invokeMethodAsync("OnScreensChanged", screens);
        },
        'OnWindowPositionsChanged': async function(windowPositions) {
            await windowManagementRef.invokeMethodAsync("OnWindowPositionsChanged", windowPositions);
        }
    };
}

export async function OpenWindow(id, content, windowFeatures, windowTitle) {
    checkInitialized();

    let win = window.open("/_content/KST.Blazor.Windows/Window.html", id, buildWindowFeatures(windowFeatures));

    windows[id] = {
        'window': win,
        'position': null
    };

    await waitForEvent(win, "load");

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

    await refreshWindowPositions();

    win.addEventListener("resize", refreshWindowPositions);
}

export function ChangeWindowTitle(id, title) {
    checkInitialized();

    windows[id].window.document.title = title;
}

export function CloseWindow(id) {
    checkInitialized();

    windows[id].window.close();
}

export async function GetMultiScreenWindowPlacementStatus() {
    checkInitialized();

    if (!("getScreenDetails" in window))
        return "NotPossible";

    if (!("permissions" in navigator))
        return "Possible";

    try {
        const { state } = await navigator.permissions.query({ name: "window-management" });

        if (state === "granted")
            return "Allowed";

        if (state === "prompt")
            return "Possible";
    } catch (error) {
        // getScreenDetails() is the authoritative feature check. A browser may
        // expose it without supporting this Permissions API descriptor query.
        console.warn("Unable to query the window-management permission.", error);
        return "Possible";
    }

    return "NotPossible";
}

export async function SetMultiScreenWindowPlacement(enabled) {
    checkInitialized();
    stopScreenMonitoring();

    if (!enabled || !("getScreenDetails" in window)) {
        await processSingleScreen();
        await refreshWindowPositions();
        return;
    }

    try {
        screenDetails = await window.getScreenDetails();
        await processScreens(screenDetails.screens);
        screenDetails.addEventListener("screenschange", screensChanged);
        await refreshWindowPositions();
    } catch (error) {
        console.error(error);
        stopScreenMonitoring();
        await processSingleScreen();
        await refreshWindowPositions();
    }
}

async function screensChanged() {
    if (screenDetails === null)
        return;

    await processScreens(screenDetails.screens);
    await refreshWindowPositions();
}

function stopScreenMonitoring() {
    if (screenDetails !== null) {
        screenDetails.removeEventListener("screenschange", screensChanged);
        screenDetails = null;
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
                'IsPrimary': screen.isPrimary
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
            windows[id].window.document.addEventListener(type, listener, options);
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
            windows[id].window.close();
        }
    }
}

function waitForEvent(eventTarget, event) {
    return new Promise(resolve => eventTarget.addEventListener(event, resolve));
}

async function refreshWindowPositions() {
    let changes = []

    for (let id in windows) {
        if (windows.hasOwnProperty(id)) {
            let win = windows[id];
            let currentScreen = getCurrentScreen(win.window);
            let newPosition = {
                'left': win.window.screenLeft,
                'top': win.window.screenTop,
                'width': win.window.outerWidth,
                'height': win.window.outerHeight,
                'innerWidth': win.window.innerWidth,
                'innerHeight': win.window.innerHeight,
                'screen': `${currentScreen.availLeft ?? 0},${currentScreen.availTop ?? 0}`
            };

            if (!shallowEqual(win.position, newPosition)) {
                win.position = newPosition;

                changes.push({
                    'windowId': win.window.name,
                    ...newPosition
                });
            }
        }
    }

    if (changes.length > 0) {
        await windowManagement.OnWindowPositionsChanged(changes);
    }
}

function getCurrentScreen(win) {
    if (screenDetails === null || screenDetails.screens.length === 0)
        return win.screen;

    const windowLeft = win.screenLeft;
    const windowTop = win.screenTop;
    const windowRight = windowLeft + win.outerWidth;
    const windowBottom = windowTop + win.outerHeight;

    let currentScreen = screenDetails.screens[0];
    let largestIntersection = -1;

    for (const screen of screenDetails.screens) {
        const intersectionWidth = Math.max(
            0,
            Math.min(windowRight, screen.left + screen.width) - Math.max(windowLeft, screen.left)
        );
        const intersectionHeight = Math.max(
            0,
            Math.min(windowBottom, screen.top + screen.height) - Math.max(windowTop, screen.top)
        );
        const intersection = intersectionWidth * intersectionHeight;

        if (intersection > largestIntersection) {
            currentScreen = screen;
            largestIntersection = intersection;
        }
    }

    return currentScreen;
}

function checkInitialized() {
    if (!initialized) {
        throw new Error("Module WindowHandler.js from KST.Blazor.Windows library was not initialized yet");
    }
}

function shallowEqual(object1, object2) {
    if (object1 === null || object2 === null)
        return object1 === object2;

    const keys1 = Object.keys(object1);
    const keys2 = Object.keys(object2);

    if (keys1.length !== keys2.length)
        return false;
    
    return keys1.every(key => object2.hasOwnProperty(key) && object1[key] === object2[key])
}

export function Init() {
    if (initialized)
        return;

    document.addEventListener = customAddEventListener;
    document.querySelector = customQuerySelector;

    window.addEventListener("unload", closeAllWindows);

    window.setInterval(refreshWindowPositions, 100);

    initialized = true;
}
