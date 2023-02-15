import socket,sys,random,traceback
print("start with ",end=" ")
for i in sys.argv:
    print(i,end=" ")
print("")
co=[]
post=int(sys.argv[1])
def sendmsg(so,s):
    so.sendall(str(len(s)).rjust(1024).encode())
    so.sendall(s.encode())
    print("sended the str. There is "+str(len(s))+"bytes in total")
def recvmsg(so):
    tmp=so.recv(1024)
    s=so.recv(int(tmp)).decode() 
    print("received the str. There is "+str(len(s))+"bytes in total")
    return s
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
        sendmsg(co[now^1])
        tmp=recvmsg(co[now])
        sendmsg(co[now^1])
        if(tmp.split("\n")[0]=="1"):
            print("the player changed the return.")
            now^=1
    except:
        f=open("link_log.txt","a+")
        f.write(traceback.format_exc())
if __name__=="__main__":
    main()