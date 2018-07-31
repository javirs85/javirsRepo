$(document).ready(function () {
    if ("WebSocket" in window) {
        var wsURL = "ws://" + location.hostname + (location.port ? ':' + location.port : '') + "/sockets/";
        var socket = new WebSocket(wsURL, ['brain']);
    }
    else {
        alert("browser does not support websockets");
    }

    socket.onopen = function () {
        AddMessage("connected to socket server");
    };

    socket.onmessage = function (e) {
        AddMessage("server said: "+e.data);
    };

    $("#sendButton").click(function () {
        socket.send("clicked!");
    });

    //socket.send("message from the page!");

});

function AddMessage(msg) {
    $("#messages").append("<li>" + msg + "</li>");
}