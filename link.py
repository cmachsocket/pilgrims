import socket ,os,sys,traceback
print("start with ",end=" ")
for i in sys.argv:
    print(i,end=" ")
print(os.path.split(os.path.realpath(__file__))[0])
host=sys.argv[1]
port=int(sys.argv[2])
ms=0
fir=0
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
    global host,port,fir
    ls=[]
    try:
        for froot ,dir ,files in os.walk(os.path.split(os.path.realpath(__file__))[0]):
            print(files)
            for filen in files:
                if(filen[0]=='n'):
                    os.remove(os.path.join(os.path.split(os.path.realpath(__file__))[0],filen))
                    print("cleaned rest file,"+filen)
            break
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
    except:
        pass       
    while True:
        while fir==0:
            try:
                tmp=recvmsg(soc)
                sendd(tmp)
                tmp=recvmsg(soc)
                sendd(tmp)
                if int(tmp[1])==1:
                    print("changed the turn")
                    fir=1
            except:
                pass
        while fir==1:
            try:
                tmp=recvv()
                sendmsg(soc,tmp)
                tmp=recvv()
                sendmsg(soc,tmp)
                if int(tmp[1])==1:
                    print("changed the turn")
                    fir=0
            except:
                pass
    
def sendd(s):
    global ms
    ms+=1
    f=open("n"+str(ms)+".txt","w",encoding="utf-8")
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
                    f=open("n"+str(ms)+".txt","r",encoding="utf-8")
                except:
                    b=1
            break
    tmps=f.read()
    f.close()
    os.remove("n"+str(ms)+".txt")
    return tmps
if __name__=="__main__":
    main()