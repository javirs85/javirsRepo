
import machine

class LDR:
  def __init__(self):
    self.adc = machine.ADC(0)
    self.currentValue = 0
    
  def readRaw(self):
    return self.adc.read()
    
  def ReadWithThreshold(self, threshold):
    v = self.readRaw()
    dif = self.currentValue - v
    if dif < 0:
      dif = -dif
      
    if dif > 30:
      self.currentValue = v
      return v
    else:
      return None
      

