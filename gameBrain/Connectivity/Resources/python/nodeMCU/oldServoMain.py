import mosq, utime
from servo import Servo
from machine import Pin
import sys

shoulder = Servo(14, "shoulder")
elbow = Servo(12, "elbow")
pinStatus = False
led = Pin(2, Pin.OUT)

shoulder.setPercentage(10)
elbow.setPercentage(10)

def f(t,m):
    print(t+": "+m)
    
    if(m==b"ping"):
        com.sendStr("pong")
    elif(m == b"pong"):
        pass
    else:
        try:
            p = int(m.decode("utf-8"))
            if p>1999:
                p = p-2000
                shoulder.setRaw(p)
                com.sendStr("shoulder %d" % (p))
            else:
                p = p-1000
                elbow.setRaw(p)
                com.sendStr("elbow %d" % (p))
        except Exception as e:
            print("error processing: "+m.decode("utf-8")+ " : " + str(e))
            sys.print_exception(e)


com = mosq.communication()
com.connectWifi()
com.connectMQTT("192.168.0.14")
com.listen()
com.server.set_callback(f)

while True:
    com.server.wait_msg()
    if pinStatus == True:
        led.off()
    else:
        led.on()  

    pinStatus = not pinStatus


while True:
    com.server.check_msg()
    utime.sleep(0.1)
    if pinStatus == True:
        led.off()
    else:
        led.on()
        
    pinStatus = not pinStatus













