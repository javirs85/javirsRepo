from machine import Pin
from time import sleep
import os

print("this is main")

import network
import machine
import time  
sta_if = network.WLAN(network.STA_IF)
sta_if.active(True)
sta_if.connect('AJ_gameBrainSSID_B827EB137750', 'gameBrain')
print('connecting')

while not sta_if.isconnected():
  print(sta_if.isconnected())
  time.sleep(1)
  
  
print('WLAN connection succeeded!')
print(sta_if.ifconfig())

import UDPController
UDP = UDPController.UDPController()
UDP.init(sta_if.ifconfig()[0])


button = Pin(14, Pin.IN, Pin.PULL_UP)
led = Pin(0,Pin.OUT)
led.value(0)
currentValue = button.value()

while True:
  msg = UDP.CheckMessages()
  if msg == "ShowUp":
    print("Showup from server")
    UDP.SendPresent()
  elif msg is None:
    pass
  else:
    print("received unexpected msg from server:")
    print(msg)
    
  newValue = button.value()
  if newValue != currentValue:
    print(currentValue)
    currentValue = newValue
    if currentValue == 1:
      UDP.SendUpdate("Button not pressed")
      led.value(0)
    else:
      UDP.SendUpdate("Button PRESSED!!")
      led.value(1)
    
  time.sleep(0.1)
  




