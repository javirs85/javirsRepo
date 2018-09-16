class WebComm
{
	constructor()
	{
		this.socket;
		this.isSocketConnected = false;
		this.wsURL = "";
		var mainSocket = this;
	}
	
	
	Start() {
		if ("WebSocket" in window) {
			try {
				this.wsURL = "ws://" + location.hostname + (location.port ? ':' + location.port : '') + "/sockets/";
				appendMsg("Trying to connect to "+this.wsURL);
				this.socket = new WebSocket(this.wsURL, ['brain']);
				this.setSocketCallbacks();
			} 
			catch (err)
			{
				appendError(err);
			}
		}
		else {
			appendError("browser does not support websockets");
		}
	}
	
	processMessage(msg)
	{
		appendMsg(msg);
		var arr_from_json = JSON.parse( msg );
		if(arr_from_json.msgType == 1) //append new puzzle
			Brain.CreatePuzzle(arr_from_json.Id, arr_from_json.Name, arr_from_json.Status, arr_from_json.Details);
		else if(arr_from_json.msgType == 6) //Update existing puzzle
			Brain.UpdatePuzzle(arr_from_json);
		else if(arr_from_json.msgType == 4) //debug
			appendMsg(arr_from_json.data["debugInfo"]+"<br/>"+arr_from_json.data["callStack"]);
		else if(arr_from_json.msgType == 5) //error
			appendError(arr_from_json.data["errorInfo"]+"<br/>"+arr_from_json.data["callStack"]);
		else if(arr_from_json.msgType == 7) //overwrite
		{
			Brain.UpgradeList(arr_from_json);
		}
		else
			appendError("Unexpected message: " + arr_from_json.msgType);
	}
	
	setSocketCallbacks()
	{
		this.socket.onopen = () => {
			this.isSocketConnected = true;
			appendMsg("socket connected");
		};

		this.socket.onmessage = (e) => {
			this.processMessage(e.data);
		};
		
		this.socket.onerror = (evt) => {		
			appendError("WebSocket error");	
		}
		this.socket.onclose = (evt) => {
			if(evt.code === 1006)
				appendError("Cannot connect to WebSocket server at<br/>"+this.wsURL);
			else
				appendError("Error Code: "+evt.code);
		}
	}
	
	SendDiscoveryMessage()
	{
		var s = this.socket;
		
		if(s.readyState === 0) appendMsg("Server is still CONNECTING");
		else if (s.readyState === 1)
			s.send("discoveryRequest");
		else if (s.readyState === 2)
			appendError("The connection is in the process of closing.");
		else if (s.readyState === 3)
			appendError("The connection is closed or couldn't be opened.");
	}
	
}