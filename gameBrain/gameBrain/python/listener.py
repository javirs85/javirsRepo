import socket

listener = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
server = ("127.0.0.1", 60100)
local = ("127.0.0.1", 60101)
listener.bind(local)
data , addr = listener.recvfrom(1024)

msgKinds = ["showup", "present", "reset", "forceSolve"]

class Payload(object):
     def __init__(self, j):
         self.__dict__ = json.loads(j)
     
     def Print(self):
         return self.__dict__[]

