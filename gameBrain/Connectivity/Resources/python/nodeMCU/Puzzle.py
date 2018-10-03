from PuzzleMaster import PuzzleMaster

class Puzzle (PuzzleMaster):
	def __init__(self, ID):
		PuzzleMaster.__init__(self, ID)
	
	def Check():
		self.CheckMessages()
		
	def DoTesting(self):
		print("this is the particular implementation of testing")
	
	def SendUpdate(self):
		print("Not implemented")
	
	def DoForceSolve(self):
		print("Not implemented")
		
	# def SendMessage(self, msgKind, Data):
		
		