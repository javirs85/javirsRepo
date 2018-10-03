import json

class Message:
	def __init__(self):
		self.Id = 0
		self.msgType = ""
		self.Data = {}
	
	def Serialize(self):
		return json.dumps(self, default=lambda x: x.__dict__)