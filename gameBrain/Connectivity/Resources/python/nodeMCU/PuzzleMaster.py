import json
import Message
import socket

class PuzzleMaster:
	def __init__(self, ID):
		self.Id = ID
		self.Name = "Default name"
		self.Status = "unsolved"
		self.PuzleKind = "sensor"
		self.Details = "Default values from the constructor"
		self.myIP = "0.0.0.0"
		self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
		self.ConnectToDefault()
		Data = {}
		Data["Name"] = "Amparo"
		self.SendMessage("update", Data)
		
		
	def ConnectToIP(self, IP):
		self.sock.connect((IP, 50100))
		self.SendPresent()
		
	def ConnectToDefault(self):
		self.ConnectToIP("192.168.1.33")
	
	def SendMessage(self, msgKind, Data):
		m = Message.Message()
		m.Id = self.Id
		m.msgType = msgKind
		m.Data = Data		
		str = m.Serialize()		
		self.Send(str)
		
	def SendPresent(self):
		Data = {}
		Data["myID"] = self.Id
		Data["myName"] = self.Name
		Data["myStatus"] = self.Status
		Data["myKind"] = self.PuzleKind
		Data["myDetails"] = self.Details
		self.SendMessage("present", Data)
		
	def SendUpdate(self):
		Data = {}
		Data["myID"] = self.Id
		Data["myName"] = self.Name
		Data["myStatus"] = self.Status
		Data["myKind"] = self.PuzleKind
		Data["myDetails"] = self.Details
		self.SendMessage("update", Data)
	
	def Send(self, str):
		print('Sending new message: '+str)
		self.sock.send(bytes(str, "utf-8"))
	
	def SendTestMessage(self):
		m = Message.Message()
		m.msgType = "debug"
		m.Data = {}
		m.Data["prop1"] = "val1"
		m.Data["prop2"] = "val2"
		m.Status = self.Status
		m.PuzleKind = self.PuzleKind
		m.Details = self.Details		
		str = m.Serialize()		
		self.Send(str)
		
	def Disconnect(self):
		self.sock.close()
		
	def CheckMessages(self):
		try:
			self.sock.setblocking(False)
			data, address = self.sock.recvfrom(1024)
		except BlockingIOError:
			print("no message")
		else:
			print(data)
			str = data.decode("utf-8")
			chunks = self.SplitStringIntoJSONS(str)
			
			for chunk in chunks :
				msg = json.loads(chunk)
				command = msg["Data"]["msg"]
				if command == "testing":
					self.DoTesting()
				elif command == "update":
					self.SendUpdate()
				elif command == "forecesolve":
					self.DoForceSolve()
				elif command == "reset":
					self.DoReset()
				else:
					print("** PuzzleMaster : Unexpected message: "+command)
			

	def SplitStringIntoJSONS(self, str):
		result = []
		items = str.split("}{")
		if len(items) > 1:
			for idx, item in enumerate(items):
				if idx == 0:
					item += "}"
				elif idx == (len(items)-1):
					item = "{"+item
				else:
					item = "{"+item+"}"
				result.append(item)
			return result
		elif len(items) == 1:
			return items
		else:
			return None

		
	def DoTesting(self):
		print("Not implemented")
	
	def SendUpdate(self):
		print("Not implemented")
	
	def DoForceSolve(self):
		print("Not implemented")
		