import sys
import network
import Message
from umqtt.robust import MQTTClient

class Connectivity:
  def __init__(self, puzzle):
    self.puzzle = puzzle
    self.sta = network.WLAN(network.STA_IF)
    self.publishingChannel = "puzzles/"+self.puzzle.ID+"/values"
    self.ConnectWiFi()
    self.mqtt()   
    self.started = False
    
  def ConnectWiFi(self):
    self.sta.active(True)
    #self.sta.connect('g.tec', '#g.tec#17!')
    self.sta.connect('GBLan', 'join1234')

    print("connecting to WiFi at GBLan")

    while self.sta.isconnected() == False:
      pass
      
    self.serverIP = self.sta.ifconfig()[3]
    
    print("Successfully connected to " + self.serverIP)
    
    
  def mqtt(self):

    print("connecting to mqtt ROBUST at {}".format(self.serverIP))  
    self.c = MQTTClient("umqtt_client", self.serverIP)
    self.c.set_callback(self.processMEssage)
    self.messager = Message.MessageConstructor(self.puzzle.ID)
    self.c.connect()
    self.c.subscribe("master/#")    
    str = self.messager.GeneratePressentMessageStr()
    self.c.publish("devices/tester", str)
    print("connected to mqtt broker")
    
  def Emit(self):
    emi = MQTTClient('emiter', self.serverIP)
    emi.connect()
    emi.publish("devices/tester", "from MCU !")
    emi.disconnect()
    
  def Check(self):
    self.c.check_msg()
    
    
  def deb(self, topic, msg):
    print((topic, msg))

    
  def processMEssage(self, topic, msg):
    print("received")
    print(topic)
    print (msg)
    
    if topic == b'master':

      toprint = "from master to ALL: "
      if self.messager is not None:
        obj = self.messager.FromString(msg)
        
        if obj.Order == 0:
          toprint = " -Replying to a showup message"
          serialMsg = self.messager.GeneratePressentMessageStr()
          self.c.publish("devices/"+self.puzzle.ID, serialMsg)
          target = str(self.puzzle.solution)
          serialMsg = self.messager.GenerateThisIsMySolutionMessage(target)
          self.c.publish("devices/"+self.puzzle.ID, serialMsg)
        else:
          print ("Unexpected message from master to all:")
          print (topic)
          print (msg)  
            
    elif topic == b'master/'+self.puzzle.ID:
      #try:
        toprint = "from master to "+self.puzzle.ID+": "
        if self.messager is not None:
          obj = self.messager.FromString(msg)

          # 0 showup,
          # 1 present,
          # 2 forceSolve, 
          # 3 Reset,
          # 4 UpdateRequested,
          # 5 ImSolved , <-- out message
          # 6 UpdateYOURSolution
          
            
        if obj.Order == 2: #forceSolve
          self.puzzle.forceSolve()
        
        elif obj.Order == 3: #Reset
          self.puzzle.ResetGame()
        
        elif obj.Order == 4: #UpdateRequested
          self.publishValue()
        
        elif obj.Order == 6: #UpdateYOURSolution          
          self.puzzle.SetCurrentValueAsNewSolution()
          
        elif obj.Order == 9: #setThisNewSolution
          print(obj)
          self.puzzle.SetThisSolution(obj.Params["newSolution"])
          
        else:
          toprint = toprint + msg.decode("utf-8")
        
        print(toprint)
      #except:
      #  print("ups")
      #  print(topic)
      #  print (msg)

    else:
      print("CUSTOM ***")
      print(topic)
      print (msg)
      print("**********")
      self.ProcessCustomRAWValue(topic, msg)
  

  def Publish(self, msg):
    self.c.publish("devices/"+self.puzzle.ID, msg)
  
  def PublishValue(self, channel, val):
    self.c.publish(channel, val)
        
  def UpdateRequested(self, str):
    print("default UpdateSolution used")
    
  def ProcessCustomRAWValue(self, topic, val):
    print("using Generic Raw")
    
    
  





