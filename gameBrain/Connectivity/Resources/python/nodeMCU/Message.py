import json

class Message:
	def __init__(self):
		self.Id = 0
		self.Order = ""
		self.Params = {}
		self.Token = "Game1"
	
	def Serialize(self):
		return json.dumps(self, default=lambda x: x.__dict__)