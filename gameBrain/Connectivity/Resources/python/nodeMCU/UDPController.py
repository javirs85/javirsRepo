
import socket
import sys

class UDPController:
  
  
    
  def __init__(self):
    self.localIp = ""
    self.multicast_group = '224.3.29.71'
    self.serverPort = 60100
    self.server_address = ('', 60101)
    self.sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    self.sock.setblocking(0)
    self.sock.bind(self.server_address)
    
    #self.group = socket.inet_aton(self.multicast_group)
    #self.mreq = struct.pack('4sL', self.group, socket.INADDR_ANY)
    #self.sock.setsockopt(socket.IPPROTO_IP, socket.IP_ADD_MEMBERSHIP, self.mreq)
    print("UDP Started at 60101")
    
  def CheckMessages(self):
    try:
      data, addr = self.sock.recvfrom(1024)
    except OSError:
      pass
    else:
      self.server_address = (addr[0],60100)
      print ('received %s bytes from %s' % (len(data), self.server_address))
      return data.decode("utf-8")
   
  def Send(self, msg):
    self.sock.sendto(bytes(msg,"utf-8"), self.server_address)
    print("sending msg to {}:{}".format(self.server_address[0], self.server_address[1]))
    
  def SendPresent(self):
    self.sock.sendto(bytes('{"msgType":1,"Name":"miniChip","Id":"0","Status":0,"PuzleKind":0,"IPSender":"'+self.localIp+'"}', "utf-8"), self.server_address)
    print("sending present to {}:{}".format(self.server_address[0], self.server_address[1]))
  
  def SendUpdate(self, newDetails):
    self.sock.sendto(bytes('{"msgType":6,"Details":"'+newDetails+'","Name":"miniChip","Id":"0","Status":0,"PuzleKind":0,"IPSender":"'+self.localIp+'"}', "utf-8"), self.server_address)
    print("sending present to {}:{}".format(self.server_address[0], self.server_address[1]))




