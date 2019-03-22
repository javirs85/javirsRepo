

from machine import Pin, PWM  
  
class LEDPulse:
  def __init__(self, p, name):
        self.name = name
        self.port = p
        self.debug = False
        self.pin = Pin(p,Pin.OUT)
        self.pulser = PWM(self.pin)
        self.pulser.freq(50)
  def setRaw(self, value):
        print ("raw %s %d" % (self.name, value))
        self.pulser.duty(value)

  def set(self, angle):
    self.setPercentage(angle)

  def setPercentage(self, angle):
    if angle > 100:
      angle = 100
   
    if angle < 0:
      angle = 0
       
    #600= top 1022 = min
    maxLight = 600
    minLight = 1022
    maxAngle = 100
    minAngle = 0
     
    deltaLight = maxLight - minLight
    deltaAngle = maxAngle - minAngle
      
    slope = deltaLight / deltaAngle
      
      # light = angle*slope + c
      # c = light - (angle*slope)
      
    c = maxLight -(maxAngle * slope)
      
    currentLight = angle*slope + c
      
    currentLight = int(currentLight)
    if self.debug == True:
      print ("%s %d %d" % (self.name, angle, currentLight))
    self.pulser.duty(currentLight)






