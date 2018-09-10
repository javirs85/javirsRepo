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
		
		//this.CreatePuzzle(0, "mapa imanes", "sin tocar");
		//this.CreatePuzzle(1, "Sonar", "Fase 0");
	}
	
	CreatePuzzle(ID, name, currentStatus)
	{
		if(puzzles.some( e => e.ID === "main"+ID))
			appendError("tried to add puzzle ["+ID+", "+name+"]. But the ID already exists.");
		else
		{
			var puzzle = new Puzzle(ID, name, currentStatus);
			puzzles.push(puzzle);
			AddNewPuzzletoUI(puzzle);
			
		}	
	}
}