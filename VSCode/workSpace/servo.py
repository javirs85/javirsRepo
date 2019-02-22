from machine import Pin, PWM

class Servo:
    def __init__(self, p, name):
        self.name = name
        self.port = p
        self.pin = Pin(p,Pin.OUT)
        self.pulser = PWM(self.pin)
        self.pulser.freq(50)

    def setRaw(self, value):
        print ("raw %s %d" % (self.name, value))
        self.pulser.duty(value)


    def setPercentage(self, angle):
        #47 - 130
        val = (0.81*angle)+49
        val = int(val)
        print ("%s %d %d" % (self.name, angle, val))
        self.pulser.duty(val)

