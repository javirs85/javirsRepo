import sys
import network
from umqtt.robust import MQTTClient

class Connectivity:
	def __init__(self):
		self.sta = network.WLAN(network.STA_IF)
		self.ConnectWiFi()
		self.mqtt()
		
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
		self.c.connect()
		self.c.subscribe("master/#")
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
		if topic == b'master':
			toprint = "from master: "
			if msg == b'SHOWUP':
				toprint = " -Replying to a showup message"
				self.c.publish("devices/tester", "MCU - PRESENT")
			else:
				toprint = toprint + msg.decode("utf-8")

		elif topic == b'master/horizontal':
			val = int(msg.decode('UTF-8'))
			self.moveHorizontal(val)
			toprint = "."
		  
		elif topic == b'master/vertical':
			val = int(msg.decode('UTF-8'))
			self.moveVertical(val)
			toprint = "."

		else:
			toprint = "unexpected "

		print(toprint)
    
	def moveVertical(self, val):
		print("using Generic")
    
	def moveHorizontal(self, val):
		print("using generic")
    
	