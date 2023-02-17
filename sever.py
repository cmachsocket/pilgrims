import socket,sys,random,traceback
print("start with ",end=" ")
for i in sys.argv:
    print(i,end=" ")
print("")
co=[]
post=int(sys.argv[1])
def sendmsg(so,s):
    tmp=s.encode()
    so.sendall((len(tmp)).to_bytes(1024,'big'))
    so.sendall(tmp)
    print("sent the str. There is "+str(len(tmp))+"bytes in total")
def recvmsg(so):
    lenth=0
    tmpl=b''
    while(lenth<1024):
        s=so.recv(1024-lenth)
        lenth+=len(s)
        print("  trying to get length. There is "+str(lenth)+"bytes in total,and it was expected to receive 1024 bytes.")
        tmpl+=s
    tmp=int.from_bytes(tmpl,'big')
    lenth=0
    ans=""
    while(lenth<tmp):
        s=so.recv(tmp-lenth)
        lenth+=len(s)
        print("    received the str. There is "+str(lenth)+"bytes in total,and it was expected to receive "+str(tmp)+"bytes.")
        ans+=s.decode()
    print("the function was completed.")
    return ans
def main():
   global co,post
   soc = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
   soc.bind(("0.0.0.0", post))
   print("Waiting for connect. (0/2)")
   soc.listen(2)
   co1, addr = soc.accept()
   co.append(co1)
   print("Waiting for connect. (1/2)")
   co2, addr = soc.accept()
   co.append(co2)
   print("the socket has gotten a connect.(2/2)")
   rd=random.randint(0,1)
   sendmsg(co[0],str(rd))
   sendmsg(co[1],str((rd^1)))
   print("the orders are sent.")
   tmp=recvmsg(co[0])
   sendmsg(co[1],tmp)
   tmp=recvmsg(co[1])
   sendmsg(co[0],tmp)
   print("the names are sent.")
   now=rd^1
   while True:
    try:
        tmp=recvmsg(co[now])
        sendmsg(co[now^1],tmp)
        tmp=recvmsg(co[now])
        sendmsg(co[now^1],tmp)
        if(int(tmp[1])==1):
            print("the player changed the turn.")
            now^=1
    except:
        pass
if __name__=="__main__":
    main()