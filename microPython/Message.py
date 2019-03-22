
import json


class MessageConstructor:
  def __init__(self, ClientID):
    self.SenderID = ClientID
    
  def GeneratePressentMessageStr(self):
    m = Message(self.SenderID)
    m.Order = 1
    return m.Serialize()
  
  def GenerateThisIsMySolutionMessage(self, newSolutionSTR):
    m = Message(self.SenderID)
    m.Order = 7
    m.Params["mySolution"] = newSolutionSTR
    return m.Serialize()
    
  def GenerateStatusUpdateMessage(self, newStatus):
    m = Message(self.SenderID)
    m.Order = 8
    m.Params["myStatus"] = newStatus
    return m.Serialize()
   
  def GenerateSolvedMesage(self):
    m = Message(self.SenderID)
    m.Order = 5
    return m.Serialize()
    
  def FromString(self, str):
    obj = json.loads(str)
    m = Message(self.SenderID)
    m.Order = obj["Order"]
    m.SenderID = obj["SenderID"]
    m.Params = obj["Params"]
    return m

class Message:
  def __init__(self, ClientID):
    self.SenderID = ClientID
    self.Order = 0
    self.Params = {}
    
	
  def Serialize(self):
    return json.dumps(self.__dict__)
    
  def PressentMsg(self):
    self.Order = 1



