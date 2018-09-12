
    function appendMsg(msg) {
        $("#messages").append("<li>" + msg + "</li>");
    }

	function appendError(msg){
		$("#messages").append("<li class='error'>" + msg + "</li>");
	}


	function AddNewPuzzletoUI(puzzle)
	{
		puzzle.generateDiv();
		$("#Puzzles").append(puzzle.MainDiv);
	}

	Brain = new gameBrain();
	WebServer = new WebComm();
	puzzles = [];

$(document).ready(function () {
	Brain.Start();

	$('.drawer').drawer();

    $("#sendButton").click(function () {

		if(typeof WebServer !== 'undefined')
			WebServer.SendDiscoveryMessage();		
		else
			appendError("Server not connected");

    });

	$("#test1").click(function () {
		var p = puzzles.find(x=>x.ID == "main0");
		p.WentSolved();
	});
	$("#test2").click(function () {
		var p = puzzles.find(x=>x.ID == "main1");
		p.WentOnline("up and running");
	});
});
