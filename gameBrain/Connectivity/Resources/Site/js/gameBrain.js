$(document).ready(function () {
    if ("WebSocket" in window) {
        var wsURL = "ws://" + location.hostname + (location.port ? ':' + location.port : '') + "/sockets/";
        var socket = new WebSocket(wsURL, ['brain']);
    }
    else {
        alert("browser does not support websockets");
    }

    socket.onopen = function () {
        //appendMsg("socket connected");
    };

    socket.onmessage = function (e) {
        appendMsg(e.data);
    };

    $("#sendButton").click(function () {
        socket.send("discoveryRequest");
    });

    function appendMsg(msg) {
        $("#messages").append("<li>" + msg + "</li>");
    }

    function render() {

    }

    //socket.send("message from the page!");

});