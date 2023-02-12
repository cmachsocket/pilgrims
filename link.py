import socket ,os
'''
a=0
while True:
    a+=1
    s=input()
    f=open("tmp","w")
    f.write(s)
    f.close()
    os.rename("tmp","n"+str(a)+".txt")
while True:
    a+=1
    f=None
    while True:
        if os.path.exists("n"+str(a)+".txt"):
            b=1
            try:
                b=0
                f=open("n"+str(a)+".txt","r")
            except:
                b=1
            break
    print(f.read())
    f.close()
    os.remove("n"+str(a)+".txt")

'''
def main():
    host=recvv()
    port=int(recvv())
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect((host,port))
    tmp=recvv()
    
def sendd():
    pass
def recvv():
    pass
if __name__=="__main__":
    main()