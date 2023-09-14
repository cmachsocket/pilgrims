using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Logging;
using pil;
using System;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Diagnostics;
using System.Text;

namespace pilgrims
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Button[,,] bnts = new Button[common.MAXPLAY, common.MAXP, common.MAXN];
        public TextBox[] xu = new TextBox[common.MAXPLAY];
        public TextBox[] cost = new TextBox[common.MAXPLAY];
        public Button[] w = new Button[common.MAXPLAY];
        public Button[] na = new Button[common.MAXPLAY];
        public TextBox zhu;
        public string port;
        public string host;
        public Socket c;
        public RoutedEventArgs ee;
        public int ms;
        public Qipan qi;
        
        public void sendd(string msg)//发送给python进行传输
        {
            ms++;
            StreamWriter sr = new StreamWriter("n"+ms.ToString()+".txt",false,Encoding.UTF8);
            sr.Write(msg);
            sr.Close();        
        }
        public string recvv()//从python那边接收消息
        {
            ms++;
            StreamReader sr = null;
            while (true)//等待消息
            {
                if (File.Exists("n" + ms.ToString() + ".txt"))
                {
                    //有消息,但不知是否能访问(因为对方可能在写入
                    int b = 1;
                    while (b == 1)
                    {
                        try//能够访问到消息
                        {
                            b = 0;//退出
                            sr = new StreamReader("n" + ms.ToString() + ".txt",Encoding.UTF8);
                        }
                        catch { b = 1; }//不能访问
                    }

                    break;
                }
            }
            Thread.Sleep(50);
            //处理
            string msg=sr.ReadToEnd();
            sr.Close();
            File.Delete("n" + ms.ToString() + ".txt");
            return msg;
           
        }
        public static void cmduse(string str)
        {
            Thread sb = new Thread(new ParameterizedThreadStart(sys));
            sb.Start(str);
        }

        public static void sys(object s)
        {
            System.Diagnostics.Process.Start("python\\\\python.exe", s.ToString());

        }
        public MainWindow()
        {
            qi = new Qipan();
            int flag;
            if(MessageBox.Show("是否作为服务器?", "piligrims", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                host = Interaction.InputBox("输入IP地址", "pilgrims", "");
                flag = 0;
            }
            else
            {
                host = "127.0.0.1";
                flag = 1;
            }
            port = Interaction.InputBox("输入连接端口", "pilgrims", "");
            //调试代码区
            //host = "127.0.0.1";
            //port = 13579;
            //tmpname1 = "114514";
            //
            if(flag == 1)
            {
                cmduse("sever.py " + port);
            }
            cmduse("link.py "+host+" "+port);
            qi.t_fir=int.Parse(recvv());
            qi.p[1] = Interaction.InputBox("输入名称", "pilgrims", "");
            sendd(qi.p[1]);
            qi.p[0] = recvv();
            Title = "pilgrims" + common.BAN + " made by cmach_socket";
            InitializeComponent();
            common.initpai(ref qi);
            StreamReader sr = new StreamReader("player.txt",Encoding.UTF8);
            qi.ppai[1] = sr.ReadLine();
            sr.Close();
            StreamReader fr = new StreamReader(qi.ppai[1], Encoding.UTF8);
            for (int i = 1; i < common.MAXIP; i++)
            {
                qi.paip[1, i] = fr.ReadLine();
            }
            fr.Close();

        }
        public void gameover(int x)//x为胜者
        {
            string prmess = "游戏结束!" + qi.p[x] + "胜利!";
            MessageBox.Show(prmess, "pilgrims");
            string last = DateTime.Now.ToString();
            StreamWriter sw = new StreamWriter("result.txt",true,Encoding.UTF8);
            last += " " + qi.p[x] + "战胜了" + qi.p[x ^ 1];
            sw.WriteLine(last);
            sw.Close();
            while (qi.prtext != "") ;
            Environment.Exit(0);
        }
        public void sendmsg(int x)//发送消息
        {
            int[] sendint = new int[common.MAXI];//要发送的棋盘一维化
            string exchange = qi.fatext;//copy要发送的信息
            string extrastr = ""; //额外的字符串信息
            int ji = 0;
            qi.fatext = "";
            int l = 4;
            sendint[1] = qi.ndian[1]; sendint[2] = qi.ndian[0]; sendint[3] = qi.pxue[1]; sendint[4] = qi.pxue[0];
            //棋盘前四位分别是双方点数和血量
            if (x == 0)
            {
                sendint[0] = 1;//第0位是是否结束 1结束 0继续回合
            }
            extrastr += "$" + qi.dead;//死亡的士兵
            for (int i = 0; i <= 1; i++)
            {
                sendint[++l] = qi.pwuqi[i].naiju;
                sendint[++l] = qi.pwuqi[i].dianshu;
                sendint[++l] = qi.pwuqi[i].show;
                extrastr += "$" + qi.pwuqi[i].name;
                extrastr += "$" + qi.pwuqi[i].dll;
                extrastr += "$" + qi.pwuqi[i].cla;
                extrastr += "$" + qi.pwuqi[i].uses;
                sendint[++l] = qi.kqian[i];
                
                for(int j = 0; j <=common.MAXP-1; j++)
                {
                    sendint[++l] = qi.k[i, j]; 
                }
            }//双方武器状况
            for (int k = 0; k <= 1; k++)
            {
                for (int i = 1; i <= common.MAXP - 1; i++)
                {
                    for (int j = 1; j < common.MAXN; j++)
                    {
                        sendint[++l] = qi.bing[k, i, j].gongji;
                        sendint[++l] = qi.bing[k, i, j].fanci;
                        sendint[++l] = qi.bing[k, i, j].xue;
                        sendint[++l] = qi.bing[k, i, j].gjcishu;
                        sendint[++l] = qi.bing[k, i, j].shecheng;
                        sendint[++l] = qi.bing[k, i, j].dun;
                        sendint[++l] = qi.bing[k, i, j].lingjcs;
                        sendint[++l] = qi.bing[k, i, j].dianshu;
                        sendint[++l] = qi.bing[k, i, j].xixue;
                        sendint[++l] = qi.bing[k, i, j].boolpojia;
                        sendint[++l] = qi.bing[k, i, j].paishu;
                        sendint[++l] = qi.bing[k, i, j].qianfeng;
                        sendint[++l] = qi.bing[k, i, j].bihu;
                        sendint[++l] = qi.bing[k, i, j].dianji;
                        sendint[++l] = qi.bing[k, i, j].a;
                        sendint[++l] = qi.bing[k, i, j].b;
                        sendint[++l] = qi.bing[k, i, j].c;
                        sendint[++l] = qi.bing[k, i, j].maxxue;
                        sendint[++l] = qi.bing[k, i, j].show;
                        extrastr += "$" + qi.bing[k, i, j].name;
                        extrastr += "$" + qi.bing[k, i, j].dll;
                        extrastr += "$" + qi.bing[k, i, j].cla;
                        extrastr += "$" + qi.bing[k, i, j].yuan;
                        extrastr += "$" + qi.bing[k, i, j].ji;
                        extrastr += "$" + qi.bing[k, i, j].hui;

                        for (int r = 0; r < 10; r++)
                        {
                            sendint[++l] = qi.bing[k, i, j].tmp[r];
                        }
                    }
                }
            }//大棋盘

            sendd(exchange);
            sendd(common.tostr(sendint));
            sendd(extrastr);
            //是否死亡
            if (qi.pxue[1] <= 0 || qi.pxue[0] <= 0)
            {

                gameover(qi.pxue[0] <= 0 ? 1 : 0);//这里传递胜者
            }
        }
        public void recvmsg()//接收消息
        {
            int[] sendint = new int[common.MAXI];
            
            while (true)
            {
                Array.Clear(sendint, 0, sendint.Length - 1);
                qi.prtext=recvv();
                common.toint(recvv(), ref sendint);
                string extrastr = recvv();
                string[] extraarr = extrastr.Split('$');
                //解析消息
                int l = 4, ll = 0;
                qi.ndian[0] = sendint[1]; qi.ndian[1] = sendint[2]; qi.pxue[0] = sendint[3]; qi.pxue[1] = sendint[4];
                qi.dead = extraarr[++l];
                for (int i = 1; i >= 0; i--)
                {
                    qi.pwuqi[i].naiju = sendint[++l];
                    qi.pwuqi[i].dianshu = sendint[++l];
                    qi.pwuqi[i].show = sendint[++l];
                    qi.kqian[i] = sendint[++l];
                    qi.pwuqi[i].name = extraarr[++ll];
                    qi.pwuqi[i].dll = extraarr[++ll];
                    qi.pwuqi[i].cla = extraarr[++ll];
                    qi.pwuqi[i].uses = extraarr[++ll];
                    //if (qi.pwuqi[i].bian > 0)
                    //    qi.pwuqi[i].name = common.WNA[qi.pwuqi[i].bian - 2000];
                    // 需要修改
                    for (int j = 0; j <= common.MAXP - 1; j++)
                    {
                         qi.k[i, j] = sendint[++l];
                    }
                }
                for (int k = 1; k >= 0; k--)
                {
                    for (int i = 1; i <= common.MAXP - 1; i++)
                    {
                        for (int j = 1; j < common.MAXN; j++)
                        {
                            qi.bing[k, i, j].gongji = sendint[++l];
                            qi.bing[k, i, j].fanci = sendint[++l];
                            qi.bing[k, i, j].xue = sendint[++l];
                            qi.bing[k, i, j].gjcishu = sendint[++l];
                            qi.bing[k, i, j].shecheng = sendint[++l];
                            qi.bing[k, i, j].dun = sendint[++l];
                            qi.bing[k, i, j].lingjcs = sendint[++l];
                            qi.bing[k, i, j].dianshu = sendint[++l];
                            qi.bing[k, i, j].xixue = sendint[++l];
                            qi.bing[k, i, j].boolpojia = sendint[++l];
                            qi.bing[k, i, j].paishu = sendint[++l];
                            qi.bing[k, i, j].qianfeng = sendint[++l];
                            qi.bing[k, i, j].bihu = sendint[++l];
                            qi.bing[k, i, j].dianji = sendint[++l];
                            qi.bing[k, i, j].a = sendint[++l];
                            qi.bing[k, i, j].b = sendint[++l];
                            qi.bing[k, i, j].c = sendint[++l];
                            qi.bing[k, i, j].maxxue = sendint[++l];
                            qi.bing[k, i, j].show = sendint[++l];
                            qi.bing[k, i, j].name = extraarr[++ll];
                            qi.bing[k, i, j].dll = extraarr[++ll];
                            qi.bing[k, i, j].cla = extraarr[++ll];
                            qi.bing[k, i, j].yuan = extraarr[++ll];
                            qi.bing[k, i, j].ji = extraarr[++ll];
                            qi.bing[k, i, j].hui = extraarr[++ll];  
                               
                            for (int r = 0; r < 10; r++)
                            {
                                sendint[++l] = qi.bing[k, i, j].tmp[r];
                            }
                            //qi.bing[k, i, j].name = common.NAME[qi.bing[k, i, j].bian > 0 ? qi.bing[k, i, j].bian : -qi.bing[k, i, j].bian];
                            //需要修改
                            if (qi.bing[k, i, j].name != "")
                            {
                                qi.bing[k, i, j].a ^= 1;
                            }
                        }
                    }
                }
                flip();
                //判断死亡
                if (qi.pxue[1] <= 0 || qi.pxue[0] <= 0)
                {
                    gameover(qi.pxue[0] <= 0 ? 1 : 0);

                }
                //对方结束了回合
                if (sendint[0] == 1)
                {
                    return;
                }
            }
        }
        public void flip()
        {
            for (int i = 0; i < common.MAXPLAY; i++)
            {
                for (int j = 1; j <= common.MAXP - 1; j++)
                {
                    for (int l = 1; l <= common.MAXN - 1; l++)
                    {


                        bnts[i, j, l].Dispatcher.Invoke(new Action(() => { bnts[i, j, l].Content = "空"; }));//抄的,说是什么线程安全
                        if (qi.bing[i, j, l].name != "")
                        {
                            bnts[i, j, l].Dispatcher.Invoke(new Action(() => { bnts[i, j, l].Content = qi.bing[i, j, l].name; }));
                        }
                    }
                } 
            }
            zhu.Dispatcher.Invoke(new Action(() => { zhu.Text = qi.sendtext; } ));
            for (int i = 0; i < common.MAXPLAY; i++)
            {
                xu[i].Dispatcher.Invoke(new Action(() => { xu[i].Text = "HP:" + qi.pxue[i].ToString(); }));
                w[i].Dispatcher.Invoke(new Action(() => { w[i].Content = qi.pwuqi[i].name.ToString(); }));
                xu[i].Dispatcher.Invoke(new Action(() => { cost[i].Text = "COST:" + qi.ndian[i].ToString(); }));
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void seeMouseDown(object sender, RoutedEventArgs e)//
        {
            if (qi.mode == 0)
            {
                qi.prtext = "对方回合!";
                return;
            }
            common.clean(ref qi,"状态");
            qi.mode = 1;
            flip();
        }

        private void buyMouseDown(object sender, RoutedEventArgs e)
        {
            if(qi.mode == 0){
                qi.prtext = "对方回合!";
                return;
            }
            MainWindow mw = null;
            foreach (Window w in App.Current.Windows)
            {
                if (w is Window1)
                {
                    return;
                }
            }
            if (qi.mode == 3 && qi.mode == 2)
            {
                qi.prtext = "请先进行完之前的操作";
                return;
            }
            if (qi.mode == 1)
            {
                zhuang.Text = "购买中";
                Window1 win = new Window1();
                string add = "";
                for (int i = 1; i < common.MAXIP; i++)
                {
                    if (qi.listty[qi.paip[1, i]]=="fashu")
                    {
                        if(qi.fslist[qi.paip[1, i]].show == 0)
                        {
                            continue;
                        }
                        add = i.ToString() + " " +
                              qi.fslist[qi.paip[1, i]].name + " " +
                              qi.fslist[qi.paip[1, i]].dianshu.ToString() + "点";
                    }
                    else if (qi.listty[qi.paip[1, i]] == "wuqi")
                    {
                        if (qi.wqlist[qi.paip[1, i]].show == 0)
                        {
                            continue;
                        }
                        add = i.ToString() + " " +
                              qi.wqlist[qi.paip[1, i]].name + " " +
                              qi.wqlist[qi.paip[1, i]].dianshu.ToString() + "点";
                    }
                    else if(qi.listty[qi.paip[1, i]] == "xiaobin")
                    {
                        if (qi.xblist[qi.paip[1, i]].show == 0)
                        {
                            continue;
                        }
                        add = i.ToString() + " " +
                              qi.xblist[qi.paip[1, i]].name + " " +
                              qi.xblist[qi.paip[1, i]].dianshu.ToString() + "点";
                    }
                    win.combo.Items.Add(add);
                }
                win.Show();
            }


        }

        private void gongjMouseDown(object sender, RoutedEventArgs e)
        {
            if (qi.mode == 0)
            {
                qi.prtext = "对方回合!";
                return;
            }
            if (qi.mode == 3)
            {
                qi.prtext = "请先进行完之前的操作";
                return;
            }
            if (qi.mode != 0)
            {
                if (qi.huihes == 1)
                {
                    qi.prtext = "第一回合无法攻击";
                    return;
                }
                common.setmode(ref qi,4, new backto("","",""), "请选择攻击对象");
                flip();
            }

        }
        private void anotherturn()
        {
            if (qi.huihes != 0)
            {
                sendmsg(0);
            }
            qi.huihes++;
            string me = "";
            qi.sendtext = "对方回合";
            qi.mode = 0;
            flip();
            recvmsg();
            qi.ndian[1] += 10;
            qi.usefa[1] = 0;
            qi.usewu[1] = 0;
            common.useji(ref qi);
            sendmsg(1);
            common.clean(ref qi,"你的回合");
            flip();
        }
        private void backMouseDown(object sender, RoutedEventArgs e)
        {
            if (qi.mode == 0 && qi.huihes != 0)
            {
                qi.prtext = "对方回合!";
                return;
            }
            if (qi.mode == 3)
            {
                qi.prtext = "请先进行完之前的操作";
                return;
            }
            if (qi.mode != 0 || qi.huihes == 0)
            {

                ThreadStart pointrecv = new ThreadStart(anotherturn);
                Thread recvback = new Thread(pointrecv);
                recvback.Start();
                

            }
        }
        private void bnt_enter(object sender, MouseEventArgs e)
        {
            var the_b = sender as Button;
            if (the_b.Name.IndexOf("n") >= 0)
            {
                shux.Text = the_b.Content.ToString();
                return;
            }
            if (the_b.Name.IndexOf("w") >= 0)
            {
                string mess = the_b.Content.ToString() + "\n耐久:";
                if (the_b.Name.IndexOf("1") >= 0)
                {
                    mess += qi.pwuqi[1].naiju;
                }
                else
                {
                    mess += qi.pwuqi[0].naiju;
                }
                shux.Text = mess;
                return;
            }
            string[] tob = the_b.Name.ToString().Split('_');
            var a = new abc(int.Parse(tob[1]), int.Parse(tob[2]), int.Parse(tob[3]));
            var tbing = qi.bing[a.a, a.b, a.c];
            if (tbing.name == "")
            {
                shux.Text = "空";
                return;
            }
            string mes = (qi.p[a.a]).ToString() + "的" + (a.b).ToString()+"排第"+(a.c).ToString()+"个"+
                "\n血量:" + tbing.xue.ToString() +
                "\n攻击:" + tbing.gongji.ToString() +
                "\n反刺:" + tbing.fanci.ToString() +
                "\n剩余攻击次数:" + tbing.lingjcs.ToString() +
                "\n射程" + tbing.shecheng.ToString() +
                "\n护盾" + tbing.dun.ToString() +
                "\n吸血" + tbing.xixue.ToString() +
                "\n破甲" + tbing.boolpojia.ToString() +
                "\n前锋" + tbing.qianfeng.ToString() +
                "\n庇护" + tbing.bihu.ToString() +
                "\n电击" + tbing.dianji.ToString();
            shux.Text = mes;
            return;
        }

        //mode 0 对方回合
        //mode 1 空闲中
        //mode 2 选择攻击
        //mode 3 选择技能
        //mode 4 选择攻击对象
        private void bnt_cil(object sender, MouseButtonEventArgs e)
        {

            var the_b = sender as Button;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (qi.mode == 0)
                {
                    qi.prtext = "对方回合无法攻击";
                    return;
                }

                else if (qi.mode >= 2 || qi.mode <= 4)
                {
                    int tmp;
                    if (the_b.Name.IndexOf("n") >= 0)
                    {
                        tmp = the_b.Name[2] - '0';
                        if (qi.mode == 4)
                        {
                            common.gj1(ref qi, tmp == 1 ? -1 : -2, 0, 0);
                        }
                        else qi=common.usedll(qi,qi.bback,tmp == 1 ? -1 : -2, 0, 0);
                        sendmsg(1);
                        flip();
                        return;
                    }
                    if (the_b.Name.IndexOf("w") >= 0)
                    {

                        tmp = the_b.Name[1] - '0';
                        if (qi.mode == 4)
                        {
                            common.gj1(ref qi, tmp == 1 ? -3 : -4, 0, 0);
                        }
                        else qi = common.usedll(qi, qi.bback, tmp == 1 ? -3 : -4, 0, 0);
                        sendmsg(1);
                        flip();
                        return;
                    }
                    string[] tob = the_b.Name.ToString().Split('_');
                    var a = new abc(int.Parse(tob[1]), int.Parse(tob[2]), int.Parse(tob[3]));
                    if (qi.mode == 4)
                    {
                        common.gj1(ref qi, a.a, a.b, a.c);
                    }
                    else qi = common.usedll(qi, qi.bback, a.a, a.b, a.c);
                    sendmsg(1);
                    flip();

                }
            }
        }

        private void exiting(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void printtext()
        {
            while (true)
            {
                if (qi.prtext != "")
                {
                    prt.Dispatcher.Invoke(new Action(() => { prt.Text = qi.prtext; }));
                    qi.prtext = "";
                }
            }
        }

        
        private void form1_Loaded(object sender, RoutedEventArgs e)
        {

            zhu = zhuang;
            na[0] = na2; na[1] = na1;
            xu[0] = xu2; xu[1] = xu1;
            cost[0] = di2; cost[1] = di1;
            w[0] = w2; w[1] = w1;
            na[1].Content = qi.p[1];
            na[0].Content = qi.p[0];
            Panel.SetZIndex(buy, 10);
            ThreadStart child = new ThreadStart(printtext);
            Thread th = new Thread(child);
            th.Start();
            for(int i = 0; i <= 1; i++)
            {
                na[i].AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(bnt_cil));
                w[i].AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(bnt_cil));
            }
            for (int i = common.MAXP - 1, l = 1; i > 0; i--, l++)
            {
                for (int j = 1; j <= common.MAXN - 1; j++)
                {
                    qi.bing[1, i, j] = new xiaobin();
                    Button bnt = new Button { Name = "B_1_" + i.ToString() + "_" + j.ToString() };
                    bnt.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(bnt_cil));
                    bnt.AddHandler(MouseEnterEvent, new MouseEventHandler(bnt_enter));
                    bnt.Content = "空";
                    bnt.Height = 50;
                    bnt.Width = 80;
                    bnt.HorizontalAlignment = HorizontalAlignment.Left;
                    bnt.VerticalAlignment = VerticalAlignment.Top;
                    bnt.Margin = new Thickness(j * 100, 12 + (l - 1) * 100, 0, 0);
                    bnt.MouseDown += new MouseButtonEventHandler(bnt_cil);
                    Grid3.Children.Add(bnt);
                    bnts[1, i, j] = bnt;
                }
            }
            for (int i = 1; i <= common.MAXP - 1; i++)
            {
                for (int j = 1; j <= common.MAXN - 1; j++)
                {

                    qi.bing[0, i, j] = new xiaobin();
                    Button bnt = new Button { Name = "B_0_" + i.ToString() + "_" + j.ToString() };
                    bnt.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(bnt_cil));
                    bnt.AddHandler(MouseEnterEvent, new MouseEventHandler(bnt_enter));
                    bnt.Content = "空";
                    bnt.Height = 50;
                    bnt.Width = 80;
                    bnt.HorizontalAlignment = HorizontalAlignment.Left;
                    bnt.VerticalAlignment = VerticalAlignment.Top;
                    bnt.Margin = new Thickness(j * 100, 12 + (i - 1) * 100, 0, 0);
                    bnt.MouseDown += new MouseButtonEventHandler(bnt_cil);
                    Grid4.Children.Add(bnt);
                    bnts[0, i, j] = bnt;
                }
            }
            if (qi.t_fir == 0)//后手
            {
                ThreadStart startbackget = new ThreadStart(stratbackstart);
                Thread startback = new Thread(startbackget);
                startback.Start();//起另一个线程接收对方数据,不影响正常访问

            }
            else
            {
                qi.mode = 1;
                qi.huihes++;
                flip();
            }

        }
        private void stratbackstart()
        {
            object xoje = new object();
            backMouseDown(xoje, ee);//这个代码是在细节上有问题的,但是能跑就行了
        }
    }
}