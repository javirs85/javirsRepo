$(document).ready(function () {
    if ("WebSocket" in window) {
        var wsURL = "ws://" + location.hostname + (location.port ? ':' + location.port : '') + "/sockets/";
        var socket = new WebSocket(wsURL, ['brain']);
    }
    else {
        alert("browser does not support websockets");
    }

    socket.onopen = function () {
        alert("socket connected");
    };

    socket.onmessage = function (e) {
        alert(e.data);
    };

    $("#sendButton").click(function () {
        socket.send("clicked!");
    });

    //socket.send("message from the page!");

});