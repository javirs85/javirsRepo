
import usocket
import time

class TCPController:
  
    def init(self, wlan):
      self.wlan = wlan        
      self.port=50100
      self.serverIp=self.wlan.ifconfig()[0]
      
      ai = usocket.getaddrinfo("0.0.0.0", self.port)
      addr = ai[0][-1]
      
      self.s = usocket.socket()
      self.s.setsockopt(usocket.SOL_SOCKET, usocket.SO_REUSEADDR, 1)
      self.s.setblocking(False)
      self.s.bind(addr)
      self.s.listen(1)
      
      self.gameBrain_s = None
      self.gameBrain_addr = None
      
      print ('tcp ready to listen at:{}:{}'.format(addr[0], addr[1]))
      
    
    def checkIfConnectionsAvailables(self):
      try:
        res = self.s.accept()
        print("accepted TCP conection!")
      except OSError as err:
        return False
        #print(err)
      else:
        self.gameBrain_s = res[0]
        self.gameBrain_s.setblocking(False)
        self.gameBrain_addr = res[1]
        print("Client address:", self.gameBrain_addr)
        print("Client socket:", self.gameBrain_s)
        return(True)
        #client_s.send(CONTENT % counter)

    
    def cloaseConnection(self):
      self.gameBrain_s.close()
    
    def readMsg(self):
      try:
        req = self.gameBrain_s.recv(4096)
        print("Request:")
        print(req)
      except:
        pass
        #if(self.gameBrain_s):
        #  print("clossing TCP socket")
        #  self.gameBrain_s.close()
          
    
    
    def Send(self, message):
      if self.gameBrain_s is not None:
        self.gameBrain_s.write(message)
      else:
        print("Server not yet connected via TCP")


