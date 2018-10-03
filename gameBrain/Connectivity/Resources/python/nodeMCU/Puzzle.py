import json
import Message
import socket

class Puzzle:
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
		