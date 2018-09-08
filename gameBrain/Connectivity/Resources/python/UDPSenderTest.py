import socket
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.sendto(bytes('{"msgType":1,"data":{"Name":"laptop","Id":"2"},"Status":0,"PuzleKind":0,"IPSender":"192.168.137.7"}', "utf-8"),("192.168.137.1",60100))