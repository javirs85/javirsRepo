from PuzzleMaster import PuzzleMaster
import time

class Puzzle (PuzzleMaster):
	def __init__(self, fileName):
		PuzzleMaster.__init__(self, fileName)
		self.KeepOn = False
	
	def Check(self):
		self.CheckMessages()
		
	def DoTesting(self):
		print("this is the particular implementation of testing")
	
	def DoForceSolve(self):
		print("ForceSolve Not implemented")
		
	def DoReset(self):
		print("Reset Not implemented")
		
	def Start(self):
		self.keepOn = True
		while not self.KeepOn:
			self.CheckMessages()
			time.sleep(1)
			
	def UpdateSensedValue(self, newVal):
		self.UpdateDetails(newVal);
		isSolved = self.CheckIfSolved(newVal);

		if isSolved:
			self.UpdateStatus("solved")
			self.OnSolved()
		else:
			print(repr(newVal)+" is not the solution")
		
	def CheckIfSolved(self, newVal):
		print("not implemented")
		
	def OnSolved(self):
		print("on solved not implemented")
		
	# def SendMessage(self, msgKind, Data):
	