function notify(title) {
    window.external.notify(title);
}

function addMenu() {
    var body = document.getElementsByTagName("body")[0],
        menu = document.createElement('div');

    addButton("Refresh");
    addButton("Add Local Files");

    body.insertBefore(menu, body.firstChild);

    function addButton(title) {
        var b = document.createElement('button');
        b.textContent = title;
        b.addEventListener("click", function () {
            notify(title);
        });
        menu.appendChild(b);
    }
}

addMenu();