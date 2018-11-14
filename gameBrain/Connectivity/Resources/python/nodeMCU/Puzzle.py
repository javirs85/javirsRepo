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
		
	# def SendMessage(self, msgKind, Data):
	