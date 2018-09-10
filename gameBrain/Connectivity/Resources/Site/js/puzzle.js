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
		this.DetailsDiv = "";
		/*
		this.onResetClick = new CustomEvent('onResetClick');
		this.onOpenClick = new CustomEvent('onOpenClick');
		*/
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
		this.DetailsDiv.addClass("gray");
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
		this.DetailsDiv.removeClass("gray");
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
		this.DetailsDiv.addClass("gray");
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
		var textStatus = this.Status;
		if(textStatus == 0)
			textStatus = "Not started";
		
		this.StatusDiv = $("<div>"+textStatus+"</div>");
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
		
		this.DetailsDiv = $("<div class='Details'/>");
		var container = $("<div class='DetailsContainer'></div>");
		this.DetailsDiv.html("detailsDetailsD etailsdetailsD etailsDetailsd etailsDetailsDetail sdetailsDe tailsDet ailsdetailsDeta ilsDetails")
		container.append(this.DetailsDiv);
		this.MainDiv.append(container); 
		this.MainDiv.append(buttonBox);            
	}
	
	OpenClicked()
	{
		dispatchEvent(new CustomEvent('onOpenClick', {'detail':this}));
	}
	
	ResetClicked()
	{
		dispatchEvent(new CustomEvent('onResetClick', {'detail':this}));
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