import json
import Message
import socket

class PuzzleMaster:
	def __init__(self):
		try:
			self.ReadSettingsFromFile()
		except:
			self.Id = 1
			self.Name = "default Name"
			self.Status = "unsolved"
			self.Details = {}
			self.PuzzleKind = "sensor"
			self.SaveCurrentStatusAsDefault()
			
		self.myIP = "0.0.0.0"
		self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
		self.ConnectToDefault()		
		
	def __init__(self, file):
		try:
			self.ReadSettingsFromFile(file)
		except:
			self.Id = 1
			self.Name = _Name
			self.Status = "unsolved"
			self.Details = {}
			self.PuzzleKind = _Kind
			self.SaveCurrentStatusAsDefault()
			
		self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
		self.ConnectToDefault()		
		
	def ConnectToIP(self, IP):
		self.sock.connect((IP, 53200))
		self.SendPresent()
		
	def ConnectToDefault(self):
		self.ConnectToIP("192.168.1.34")
		
	
	def SendMessage(self, msgKind, Params):
		m = Message.Message()
		m.Id = self.Id
		m.Order = msgKind
		m.Params = Params
		str = m.Serialize()		
		self.Send(str)
		
	def SendPresent(self):
		Params = {}
		Params["myID"] = self.Id
		Params["myName"] = self.Name
		self.SendMessage("Present", Params)
		
		
	def UpdateStatus(self, newStatus):
		self.Status = newStatus
		Data = {}
		Data["prop"] = "status";
		Data["newVal"] = self.Status
		self.SendMessage("update", Data)
		
	def UpdateDetails(self, newDetails):
		Data = {}
		Data["prop"] = "sensed";
		Data["newVal"] = newDetails
		self.SendMessage("update", Data)
		
	
	def Send(self, str):
		#print('Sending new message: '+str)
		print ('->')
		self.sock.send(bytes(str, "utf-8"))
	
		
	def Disconnect(self):
		self.sock.close()
		
	def CheckMessages(self):
		try:
			self.sock.setblocking(False)
			data, address = self.sock.recvfrom(1024)
		except BlockingIOError:
			print("-")
		else:
			print("<-")
			str = data.decode("utf-8")
			chunks = self.SplitStringIntoJSONS(str)
			
			for chunk in chunks :
				msg = json.loads(chunk)
				#4 = forceOpen
				if msg['Order'] == 6:
					self.DoForceSolve()
				#3 = reset
				elif msg['Order'] == 7:
					self.DoReset()
				else:
					print("** PuzzleMaster : Unexpected message: " + repr(msg['Order']))
			

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

			
	#{"r1": "10:10", "s1": "s1", "r2": "20:20", "s2": "s2", "s3": "s3", "r3": "30:30", "r4": "40:40", "s4": "s4", "r5": "50:50", "s5": "s5"}		
			
	def SaveCurrentStatusAsDefault(self):
		Settings = {}
		Settings["Name"] = self.Name
		Settings["Id"] = self.Id
		Settings["Status"] = self.Status
		Settings["Details"] = self.Details
		Settings["Kind"] = self.PuzzleKind
		
		with open('settings.json', 'w') as outfile:
			json.dump(Settings, outfile)
			
	def ReadSettingsFromFile(self, fileName):
		with open(fileName) as infile:
			Settings = json.load(infile)
		self.Name = Settings["Name"]
		self.Id = Settings["Id"]
		self.Status = Settings["Status"]
		self.Details = Settings["Details"]
		self.PuzzleKind = Settings["Kind"]
		
		
	def DoTesting(self):
		print("Not implemented")
	
	def DoForceSolve(self):
		print("Not implemented")
		