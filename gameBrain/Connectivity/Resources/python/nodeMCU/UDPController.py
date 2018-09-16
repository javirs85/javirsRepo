
import socket
import sys

class UDPController:
  
  
    
  def __init__(self):
    self.localIp = ""
    self.multicast_group = '224.3.29.71'
    self.serverPort = 60100
    self.server_address = ('192.168.137.1', 60100)
    self.listener = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    self.listener.setblocking(0)
    self.sender = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    
    
    #self.group = socket.inet_aton(self.multicast_group)
    #self.mreq = struct.pack('4sL', self.group, socket.INADDR_ANY)
    #self.sock.setsockopt(socket.IPPROTO_IP, socket.IP_ADD_MEMBERSHIP, self.mreq)
    print("UDP Started at 60101")
    
  def init(self, ip):
    self.localIp = ip
    self.listener.bind(('',60101))
    self.SendPresent()
    
  def CheckMessages(self):
    try:
      data, addr = self.listener.recvfrom(1024)
    except OSError:
      pass
    else:
      self.server_address = (addr[0],60100)
      print ('received %s bytes from %s' % (len(data), self.server_address))
      return data.decode("utf-8")
   
  def Send(self, msg):
    self.sender.sendto(bytes(msg,"utf-8"), self.server_address)
    print("sent")
    
  def SendPresent(self):
    msg ='{"msgType":1,"Name":"miniChip","Id":"0","Status":0,"PuzleKind":0,"IPSender":"'+self.localIp+'"}'
    self.Send(msg)
    print("sending present")
  
  def SendUpdate(self, newDetails):
    self.Send('{"msgType":6,"Details":"'+newDetails+'","Name":"miniChip","Id":"0","Status":0,"PuzleKind":0,"IPSender":"'+self.localIp+'"}')
    print("sending update")



