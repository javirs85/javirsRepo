import Puzzle

connector = Puzzle.Puzzle('MapSettings.json')

def ForcedSolvingAction():
	print("im solving the puzzle")
	connector.UpdateSensedValue('1324')
	
def CheckingAction(newVal):
	if newVal == '33':
			return True
	else:
		return False
		
def ResetAction():
	connector.UpdateSensedValue('0')
	connector.UpdateStatus("unsolved")
			
def WhenSolvedAction():
	print(" ++ Congratulations")
	
	
connector.DoForceSolve = ForcedSolvingAction
connector.CheckIfSolved = CheckingAction
connector.OnSolved = WhenSolvedAction
connector.DoReset = ResetAction

connector.UpdateSensedValue('0')
user = input()
while user != 'q':
	if user != "":
		connector.UpdateSensedValue(user)
	
	connector.Check()
	user = input()