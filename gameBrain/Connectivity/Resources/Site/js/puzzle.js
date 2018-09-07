class Puzzle
{	
	constructor(ID,Name, Status)
	{
		this.ID = "main"+ID;
		this.Name = Name;
		this.Status = Status;
		this.MainDiv = "";
		this.StatusDiv = "";
		this.ResetButton = "";
		this.OpenButton = "";
	}
	
	UpdateStatus(newStatus)
	{
		this.Status = newStatus;
		this.StatusDiv.html(newStatus);
	}
	
	WentOffline(){
		this.MainDiv.fadeTo("slow", 0.5);
		this.Status = "Offline";
		this.StatusDiv.html(this.Status);
		this.ResetButton.removeClass("active");
		this.ResetButton.addClass("disabled");
		this.OpenButton.removeClass("active");
		this.OpenButton.addClass("disabled");
	}
	
	WentOnline(newStatus)
	{
		this.MainDiv.fadeTo("slow", 1);
		this.Status = newStatus;
		this.StatusDiv.html(this.Status);
		this.StatusDiv.removeClass("green");
		this.ResetButton.addClass("active");
		this.ResetButton.removeClass("disabled");
		this.OpenButton.addClass("active");
		this.OpenButton.removeClass("disabled");
	}
	
	WentSolved()
	{
		this.MainDiv.fadeTo("slow", 0.5);
		this.Status = "SOLVED";
		this.StatusDiv.addClass("green");
		this.StatusDiv.html(this.Status);
		this.ResetButton.removeClass("active");
		this.ResetButton.addClass("disabled");
		this.OpenButton.removeClass("active");
		this.OpenButton.addClass("disabled");
	}
	
	generateDiv()
	{
		this.MainDiv = $('<div id="'+this.ID+'"/>').addClass("puzzle");
		var LabelsBox = $('<div class="labelsBox"/>');
		LabelsBox.append("<div class='Name'>"+this.Name.toUpperCase()+"</div>");
		this.StatusDiv = $("<div>"+this.Status+"</div>");
		this.StatusDiv.addClass("Status");		
		LabelsBox.append(this.StatusDiv);
		this.MainDiv.append(LabelsBox);
		var buttonBox = $("<div class='buttonBox'/>");
		this.ResetButton = $("<button class='button disabled'>RESET</button>");
		this.ResetButton.click(function(){
			var puzzle = Puzzle.fromButtonToPuzzle(this);
			if(puzzle.Status != "Offline" && !$(this).hasClass("disabled"))
				puzzle.ResetClicked();
		});
		this.OpenButton = $("<button class='button active'>OPEN</button>");
		this.OpenButton.click(function(){
			var puzzle = Puzzle.fromButtonToPuzzle(this);
			if(puzzle.Status != "Offline" && !$(this).hasClass("disabled"))
				puzzle.OpenClicked();
		});
		buttonBox.append(this.ResetButton);
		buttonBox.append(this.OpenButton);
		this.MainDiv.append(buttonBox);            
	}
	
	OpenClicked()
	{
		appendMsg("user requested force-opening "+this.Name);
	}
	
	ResetClicked()
	{
		appendMsg("user requested reseting "+this.Name);
	}
	
	static fromIDToPuzzle(id)
	{
		return puzzles.find(x=> x.ID == id);
	}
	
	static fromButtonToPuzzle(item)
	{
		var id = $(item).parent().parent().attr('id');
		return Puzzle.fromIDToPuzzle(id);
	}

}