UDP = UDPController.UDPController()

UDP.ConnecToWifi()
print("wifi connected")
UDP.init()

print("starting TCP")
import TCPController
TCP = TCPController.TCPController()
TCP.init(UDP.wlan)

button = Pin(14, Pin.IN, Pin.PULL_UP)
led = Pin(0,Pin.OUT)
led.value(0)
currentValue = button.value()

TCPConnected = False

while True:
  msg = UDP.CheckMessages()
  if msg is None:
    pass
    #print ("no udp messages to be read")
  elif msg.lower() == "showup":
    print("Showup from server")
    UDP.SendPresent()    
    TCP.Send("this is a test")
  else:
    print("received unexpected msg from server:")
    print(msg)
    
  if TCPConnected:
    TCP.readMsg()
  else:   
    if TCP.checkIfConnectionsAvailables():
      TCPConnected = True
    
  
  #newValue = button.value()
  #if newValue != currentValue:
  #  print(currentValue)
  #  currentValue = newValue
  #  if currentValue == 1:
  #    UDP.SendUpdate("Button not pressed")
  #    led.value(0)
  #  else:
  #    UDP.SendUpdate("Button PRESSED!!")
  #    led.value(1)
    
  time.sleep(0.2)
  if currentValue:
    currentValue = 0
  else:
    currentValue = 1
  
  led.value(currentValue)
  






