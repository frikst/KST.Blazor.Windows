import { Init } from "./WindowHandler.js";

var initialized = false;

export function beforeWebStart(options) {
    if (initialized) {
        return;
    }
    initialized = true;

    Init();
}

export function beforeStart(options, extensions) {
    if (initialized) {
        return;
    }
    initialized = true;

    Init();
}
