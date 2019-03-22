
import Connectivity, time
import Message

class Puzzle:
  def __init__(self, ID):
    self.ID = ID
    self.messager = Message.MessageConstructor(self.ID)
    self.StopRequest = False
    self.solution = 600
    self.con = Connectivity.Connectivity(self)  
    self.status = "Online"
    self.currentValue = ""
    self.publishingChannel = "puzzles/"+self.ID+"/values"
    
  def forceSolve(self):
    print("overriding forceSolve needed")
    
  def ResetGame(self):
    print("overriding ResetGame needed")
    
  def FormatIncomingNewSolution(self, newStringValue):
    print("overriding FormatIncomingNewSolution needed")
    
  def SetCurrentValueAsNewSolution(self):
    self.solution = self.currentValue
    serialMsg = self.messager.GenerateThisIsMySolutionMessage(self.solution)
    self.con.Publish(serialMsg)
    
  def publishValue(self):
    s = str(self.currentValue)
    self.con.PublishValue(self.publishingChannel, s)
    
  def SetThisSolution(self, newValue): 
    self.FormatIncomingNewSolution(newValue)
    serialMsg = self.messager.GenerateThisIsMySolutionMessage(self.solution)
    self.con.Publish(serialMsg)
    

  def SetStatusToSolved(self):
    self.status = "Solved"
    serialMsg = self.messager.GenerateStatusUpdateMessage(self.status)
    self.con.Publish(serialMsg)
    
  def SetStatusToUnsolved(self):
    self.status = "Online"
    serialMsg = self.messager.GenerateStatusUpdateMessage(self.status)
    self.con.Publish(serialMsg)
  




