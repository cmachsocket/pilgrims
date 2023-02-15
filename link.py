import socket ,os,sys,traceback
print("start with ",end=" ")
for i in sys.argv:
    print(i,end=" ")
print("")
host=sys.argv[1]
port=int(sys.argv[2])
ms=0
fir=0
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
    global host,port,fir
    soc = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    soc.connect((host,port))
    print("connected.")
    tmp=recvmsg(soc)
    fir=int(tmp)
    print("the order has received.")
    sendd(tmp)
    tmp=recvv()
    sendmsg(soc,tmp)
    tmp=recvmsg(soc)
    sendd(tmp)
    while True:
        while fir==0:
            try:
                tmp=recvmsg(soc)
                sendd(tmp)
                tmp=recvmsg(soc)
                sendd(tmp)
                if tmp.split('\n')[0]=="1":
                    fir==1
            except:
                f=open("link_log.txt","a+")
                f.write(traceback.format_exc())
        while fir==1:
            try:
                tmp=recvv()
                sendmsg(soc,tmp)
                tmp=recvv()
                sendmsg(soc,tmp)
                if tmp.split('\n')[0]=="1":
                    fir==0
            except:
                f=open("link_log.txt","a+")
                f.write(traceback.format_exc())
    
def sendd(s):
    global ms
    ms+=1
    f=open("n"+str(ms)+".txt","w")
    f.write(s)
    f.close()
def recvv():
    global ms
    ms+=1
    while True:
        if os.path.exists("n"+str(ms)+".txt"):
            b=1
            while b==1:
                try:
                    b=0
                    f=open("n"+str(ms)+".txt","r")
                except:
                    b=1
            break
    tmps=f.read()
    f.close()
    os.remove("n"+str(ms)+".txt")
    return tmps
if __name__=="__main__":
    main()