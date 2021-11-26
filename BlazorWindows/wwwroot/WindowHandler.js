var windows = { };

export function OpenWindow(id, content, windowFeatures) {
    let win = window.open("about:blank", id, windowFeatures);
    windows[id] = win;
    win.document.body.appendChild(content);

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

}
