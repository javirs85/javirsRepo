import sys
import network
from umqtt.robust import MQTTClient

class Connect:
  def __init__(self):
    self.sta = network.WLAN(network.STA_IF)
    self.ConnectWiFi()
    self.c = MQTTClient("umqtt_client", "192.168.110.51")
    self.ConnectMQTT()
    
  def ConnectMQTT(self):
    print("connecting to mqtt ROBUST")  
    self.c.set_callback(self.deb)
    self.c.connect()
    self.c.subscribe("test/msg")
    
  def Emit(self):
    emi = MQTTClient('emiter', "192.168.110.51")
    emi.connect()
    emi.publish("test/msg", "from nodeMCU !")
    emi.disconnect()
    
  def deb(self, topic, msg):
    print((topic, msg))
    
  def ConnectWiFi(self):
    self.sta.active(True)
    self.sta.connect('g.tec', '#g.tec#17!')
    
    print("connecting to WiFi at gtec")
    
    while self.sta.isconnected() == False:
      pass
    
    print("Connection successful")
    print(self.sta.ifconfig())

