import socket
import struct
import sys

multicast_group = '224.3.29.71'
serverPort = 60100
server_address = ('', 60101)

# Create the socket
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

# Bind to the server address
sock.bind(server_address)

# Tell the operating system to add the socket to the multicast group
# on all interfaces.
group = socket.inet_aton(multicast_group)
mreq = struct.pack('4sL', group, socket.INADDR_ANY)
sock.setsockopt(socket.IPPROTO_IP, socket.IP_ADD_MEMBERSHIP, mreq)

# Receive/respond loop
while True:
    print ('\nwaiting to receive message')
    data, address = sock.recvfrom(1024)
    print ('received %s bytes from %s' % (len(data), address))
    address = (address[0],60100)
    print ('sending acknowledgement to', address)
    sock.sendto(bytes('{"msgType":1,"data":{"Name":"laptop","Id":"0"},"Status":0,"PuzleKind":0,"IPSender":"192.168.137.7"}', "utf-8"), address)