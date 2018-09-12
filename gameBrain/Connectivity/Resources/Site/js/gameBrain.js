class gameBrain
{	
	constructor()
	{
		addEventListener('onOpenClick', function(elem){			
			appendMsg("GB: user requested force-opening "+elem.detail.Name);
			}, false);
			
		addEventListener('onResetClick', function(elem){
			appendMsg("GB: user requested reseting "+elem.detail.Name);
			}, false);
	}
	
	Start()
	{		
		WebServer.Start();
		
		this.CreatePuzzle(0, "mapa imanes", "sin tocar");
		//this.CreatePuzzle(1, "Sonar", "Fase 0");
	}
	
	CreatePuzzle(ID, name, currentStatus, Details)
	{
		if(puzzles.some( e => e.ID === ID))
			appendError("tried to add puzzle ["+ID+", "+name+"]. But the ID already exists.");
		else
		{
			var puzzle = new Puzzle(ID, name, currentStatus, Details);
			puzzles.push(puzzle);
			AddNewPuzzletoUI(puzzle);
			
		}	
	}
	
	UpdatePuzzle(message)
	{
		var puzzle = puzzles.find(x => x.ID == message.Id);
		if(puzzle.Status != message.Status)
			puzzle.UpdateStatus(message.Status);
		if(puzzle.Details != message.Details)
			puzzle.UpdateDetails(message.Details);
		//
		//TODO find the puzzle by ID and update it ussing arr_from_json.Id, arr_from_json.Name, arr_from_json.Status, arr_from_json.Details
	}
	
}