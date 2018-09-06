$(document).ready(function () {
	var socket;
	var isSocketConnected = false;
	var wsURL;
	
    if ("WebSocket" in window) {
		try {
			wsURL = "ws://" + location.hostname + (location.port ? ':' + location.port : '') + "/sockets/";
			appendMsg("Trying to connect to "+wsURL);
			socket = new WebSocket(wsURL, ['brain']);
		} catch (err)
		{
			appendMsg(err);
		}
    }
    else {
        alert("browser does not support websockets");
    }

    socket.onopen = function () {
		isSocketConnected = true;
        appendMsg("socket connected");
    };

    socket.onmessage = function (e) {
        appendMsg(e.data);
    };
	
	socket.onerror = function (evt) {		
		appendError("WebSocket error");	
	}
	socket.onclose = function (evt) {
		if(evt.code == 1006)
			appendError("Cannot connect to WebSocket server at<br/>"+wsURL);
		else
			appendMsg("Error Code: "+evt.code);
	}

    $("#sendButton").click(function () {
		if(socket.readyState == 0) appendMsg("Server is still CONNECTING");
		else if (socket.readyState == 1)
			socket.send("discoveryRequest");
		else if (socket.readyState == 2)
			appendError("The connection is in the process of closing.");
		else if (socket.readyState == 3)
			appendError("The connection is closed or couldn't be opened.");
			
		
    });

    function appendMsg(msg) {
        $("#messages").append("<li>" + msg + "</li>");
    }
	
	function appendError(msg){
		$("#messages").append("<li class='error'>" + msg + "</li>");
	}

    function render() {

    }

    //socket.send("message from the page!");

});