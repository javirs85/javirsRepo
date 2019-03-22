import sys, time, os, machine

import LedPulse
led = LedPulse.LEDPulse(2, "onboard")
led.set(0)

import LDR
ldr = LDR.LDR()

import Puzzle
puzzle = Puzzle.Puzzle("LDR")


#region callback funtions 

def init():
  led = LedPulse.LEDPulse(2, "onboard")
  led.set(0)
  
def FormatIncomingNewSolution(newValuestr):
  puzzle.solution = float(newValuestr)
  
def ProcessCustomRAWValue(topic, val):
    print("Custom: ")
    print(topic)
    print(val)
    print("------")
    
def forceSolve():
  Solve()
  
def ResetGame():
  led.set(0)
  puzzle.SetStatusToUnsolved()
  
def Solve():
  led.set(80)
  puzzle.SetStatusToSolved()
  
#endregion callback 

init()

puzzle.ProcessCustomRAWValue = ProcessCustomRAWValue
puzzle.forceSolve = forceSolve
puzzle.ResetGame = ResetGame
puzzle.FormatIncomingNewSolution = FormatIncomingNewSolution



while True:  
  msBefore = time.ticks_ms()
  puzzle.con.Check()
  
  sample = ldr.ReadWithThreshold(3)
  
  if sample == None:
    pass
  else:
    
    if sample < puzzle.solution:
      print("solved!")
      Solve()
    
    puzzle.currentValue = sample
    puzzle.publishValue()
    # currentValue will be updated automatically
  
  
  msAfter = msBefore + 300
  while time.ticks_ms() < msAfter:
    time.sleep_ms(10)








