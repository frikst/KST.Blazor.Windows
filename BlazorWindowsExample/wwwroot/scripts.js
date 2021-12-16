function test() {
    console.log("test 1");
}

function testElReference(el) {
    console.log(el);
    el.innerText = el.innerText + ".";
}