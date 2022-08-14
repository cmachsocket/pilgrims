﻿//using Microsoft.VisualBasic;
using pil;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace pilgrims
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Button[,,] bnts = new Button[common.MAXPLAY, common.MAXP, common.MAXN];
        public static TextBox[] xu = new TextBox[common.MAXPLAY];
        public static TextBox[] cost = new TextBox[common.MAXPLAY];
        public static Button[] w = new Button[common.MAXPLAY];
        public static Button[] na = new Button[common.MAXPLAY];
        public static TextBox zhu;
        public int port;
        public string host, tmpname1 = "", tmpname2 = "";
        public static Socket c;
        public static RoutedEventArgs ee;
        public static byte[] buf1 = new byte[1024];
        public static byte[] buf2 = new byte[1024];
        public MainWindow()
        {

            //host = Interaction.InputBox("输入IP地址", "pilgrims", "");
            //port = int.Parse(Interaction.InputBox("输入连接端口", "pilgrims", ""));
            //调试代码区
            host = "127.0.0.1";
            port = 13579;
            tmpname1 = "114514";
            //
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndPoint实例
            c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket
            c.Connect(ipe);//连接到服务器
            
            c.Receive(buf1);
            common.t_fir = buf1[0];
            //tmpname1 = Interaction.InputBox("输入名称", "pilgrims", "");
            common.p[1] = tmpname1;
            Array.Clear(buf1, 0, buf1.Length);
            buf1 = System.Text.UnicodeEncoding.Default.GetBytes(tmpname1);
            c.Send(buf1);
            c.Receive(buf2);
            common.p[0] = System.Text.UnicodeEncoding.Default.GetString(buf2);

            //调试代码区
            //common.mode = 1;
            //common.t_fir = 1;
            //调试代码结束
            Title = "pilgrims" + common.BAN + " made by cmach_socket";
            InitializeComponent();
            common.initpai();
            StreamReader sr = new StreamReader("player.txt");
            common.ppai[1] = sr.ReadLine();
            sr.Close();
            StreamReader fr = new StreamReader(common.ppai[1]);
            for (int i = 1; i < common.MAXIP; i++)
            {
                common.paip[1, i] = int.Parse(fr.ReadLine());
            }
            fr.Close();

        }
        public static void gameover(int x)
        {
            string prmess = "游戏结束!" + common.p[x] + "胜利!";
            MessageBox.Show(prmess, "pilgrims");
            string last = DateTime.Now.ToString();
            StreamWriter sw = File.AppendText("result.txt");
            last += " " + common.p[x] + "战胜了" + common.p[x ^ 1];
            sw.WriteLine(last);
            sw.Close();
            while (common.prtext != "") ;
            Environment.Exit(0);
        }
        public static void sendmsg(int x)
        {
            int[] sendint = new int[common.MAXI];
            byte[] btmp = new byte[1024];
            byte[] tmpb = new byte[1024];
            byte[] exchange = new byte[1024];
            exchange = System.Text.UnicodeEncoding.Default.GetBytes(common.fatext);
            for(int i = 1; i < exchange.Length; i++)
            {
                btmp[i] = exchange[i];
            }
            c.Send(btmp);
            int l = 4;
            sendint[1] = common.ndian[1]; sendint[2] = common.ndian[0]; sendint[3] = common.pxue[1]; sendint[4] = common.pxue[0];
            if (x == 0)
            {
                sendint[0] = 1;
            }
            for (int i = 0; i <= 1; i++)
            {
                sendint[++l] = common.pwuqi[i].naiju;
                sendint[++l] = common.pwuqi[i].dianshu;
                sendint[++l] = common.pwuqi[i].bian;
                sendint[++l] = common.pwuqi[1].p;
            }
            for (int k = 0; k <= 1; k++)
            {
                for (int i = 1; i <= common.MAXP - 1; i++)
                {
                    for (int j = 1; j < common.MAXN; j++)
                    {
                        sendint[++l] = common.bing[k, i, j].gongji;
                        sendint[++l] = common.bing[k, i, j].fanci;
                        sendint[++l] = common.bing[k, i, j].xue;
                        sendint[++l] = common.bing[k, i, j].gjcishu;
                        sendint[++l] = common.bing[k, i, j].shecheng;
                        sendint[++l] = common.bing[k, i, j].dun;
                        sendint[++l] = common.bing[k, i, j].lingjcs;
                        sendint[++l] = common.bing[k, i, j].dianshu;
                        sendint[++l] = common.bing[k, i, j].xixue;
                        sendint[++l] = common.bing[k, i, j].boolpojia;
                        sendint[++l] = common.bing[k, i, j].paishu;
                        sendint[++l] = common.bing[k, i, j].qianfeng;
                        sendint[++l] = common.bing[k, i, j].boolji;
                        sendint[++l] = common.bing[k, i, j].bihu;
                        sendint[++l] = common.bing[k, i, j].dianji;
                        sendint[++l] = common.bing[k, i, j].bian;
                        sendint[++l] = common.bing[k, i, j].a;
                        sendint[++l] = common.bing[k, i, j].b;
                        sendint[++l] = common.bing[k, i, j].c;
                        sendint[++l] = common.bing[k, i, j].maxxue;
                        sendint[++l] = common.bing[k, i, j].yuan;
                        sendint[++l] = common.bing[k, i, j].ji;
                        sendint[++l] = common.bing[k, i, j].hui;
                        for (int r = 0; r < 10; r++)
                        {
                            sendint[++l] = common.bing[k, i, j].tmp[r];
                        }
                    }
                }
            }
            common.itob(ref common.get_b, sendint, l);
            //c.Receive(tmpb);
            c.Send(common.get_b);
            if (common.pxue[1] == 0 || common.pxue[0] == 0)
            {

                gameover(common.pxue[0] == 0 ? 1 : 0);
            }
            Array.Clear(common.get_b, 0, common.get_b.Length - 1);
        }
        public static void recvmsg()
        {
            int[] sendint = new int[common.MAXI];
            
            while (true)
            {
                byte[] btmp = new byte[1024];
                byte[] tmpb = new byte[1024];
                Array.Clear(common.get_b, 0, common.get_b.Length - 1);
                Array.Clear(btmp, 0, btmp.Length - 1);
                Array.Clear(sendint, 0, sendint.Length - 1);
                c.Receive(btmp);
                //c.Send(tmpb);
                common.prtext = System.Text.UnicodeEncoding.Default.GetString(btmp);
                c.Receive(common.get_b);
                common.btoi(ref sendint, common.get_b, common.get_b.Length - 1);
                int l = 4;
                common.ndian[0] = sendint[1]; common.ndian[1] = sendint[2]; common.pxue[0] = sendint[3]; common.pxue[1] = sendint[4];
                for (int i = 1; i >= 0; i--)
                {
                    common.pwuqi[i].naiju = sendint[++l];
                    common.pwuqi[i].dianshu = sendint[++l];
                    common.pwuqi[i].bian = sendint[++l];
                    common.pwuqi[i].p = sendint[++l];
                    if(common.pwuqi[i].bian > 0)
                        common.pwuqi[i].name = common.FNA[common.pwuqi[i].bian - 1000];
                }
                for (int k = 1; k >= 0; k--)
                {
                    for (int i = 1; i <= common.MAXP - 1; i++)
                    {
                        for (int j = 1; j < common.MAXN; j++)
                        {
                            common.bing[k, i, j].gongji = sendint[++l];
                            common.bing[k, i, j].fanci = sendint[++l];
                            common.bing[k, i, j].xue = sendint[++l];
                            common.bing[k, i, j].gjcishu = sendint[++l];
                            common.bing[k, i, j].shecheng = sendint[++l];
                            common.bing[k, i, j].dun = sendint[++l];
                            common.bing[k, i, j].lingjcs = sendint[++l];
                            common.bing[k, i, j].dianshu = sendint[++l];
                            common.bing[k, i, j].xixue = sendint[++l];
                            common.bing[k, i, j].boolpojia = sendint[++l];
                            common.bing[k, i, j].paishu = sendint[++l];
                            common.bing[k, i, j].qianfeng = sendint[++l];
                            common.bing[k, i, j].boolji = sendint[++l];
                            common.bing[k, i, j].bihu = sendint[++l];
                            common.bing[k, i, j].dianji = sendint[++l];
                            common.bing[k, i, j].bian = sendint[++l];
                            common.bing[k, i, j].a = sendint[++l];
                            common.bing[k, i, j].b = sendint[++l];
                            common.bing[k, i, j].c = sendint[++l];
                            common.bing[k, i, j].maxxue = sendint[++l];
                            common.bing[k, i, j].yuan = sendint[++l];
                            common.bing[k, i, j].ji = sendint[++l];
                            common.bing[k, i, j].hui = sendint[++l];
                            for (int r = 0; r < 10; r++)
                            {
                                sendint[++l] = common.bing[k, i, j].tmp[r];
                            }
                            common.bing[k, i, j].name = common.NAME[common.bing[k, i, j].bian > 0 ? common.bing[k, i, j].bian : -common.bing[k, i, j].bian];
                            if (common.bing[k, i, j].bian != 0)
                            {
                                common.vis[k, i, j] = 1;
                            }
                        }
                    }
                }
                flip();
                if (common.pxue[1] == 0 || common.pxue[0] == 0)
                {
                    gameover(common.pxue[0] == 0 ? 0 : 1);
                }
                if (sendint[0] == 1)
                {
                    return;
                }
            }
        }
        public static void flip()
        {
            for (int i = 0; i < common.MAXPLAY; i++)
            {
                for (int j = 1; j <= common.MAXP - 1; j++)
                {
                    for (int l = 1; l <= common.MAXN - 1; l++)
                    {


                        bnts[i, j, l].Dispatcher.Invoke(new Action(() => { bnts[i, j, l].Content = "空"; }));//抄的,说是什么线程安全
                        if (common.vis[i, j, l] != 0)
                        {
                            bnts[i, j, l].Dispatcher.Invoke(new Action(() => { bnts[i, j, l].Content = common.bing[i, j, l].name; }));
                        }
                    }
                } 
            }
            zhu.Dispatcher.Invoke(new Action(() => { zhu.Text = common.sendtext; } ));
            for (int i = 0; i < common.MAXPLAY; i++)
            {
                xu[i].Dispatcher.Invoke(new Action(() => { xu[i].Text = "HP:" + common.pxue[i].ToString(); }));
                w[i].Dispatcher.Invoke(new Action(() => { w[i].Content = common.pwuqi[i].name.ToString(); }));
                xu[i].Dispatcher.Invoke(new Action(() => { cost[i].Text = "COST:" + common.ndian[i].ToString(); }));
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void seeMouseDown(object sender, RoutedEventArgs e)//
        {
            if (common.mode == 0)
            {
                common.prtext = "对方回合!";
                return;
            }
            common.clean("状态");
            common.mode = 1;
            flip();
        }

        private void buyMouseDown(object sender, RoutedEventArgs e)
        {
            if(common.mode == 0){
                common.prtext = "对方回合!";
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
            if (common.mode == 3 && common.mode == 2)
            {
                MessageBox.Show("请先进行完之前的操作", "pilgrims");
                return;
            }
            if (common.mode == 1)
            {
                zhuang.Text = "购买中";
                Window1 win = new Window1();
                string add = "";
                for (int i = 1; i < common.MAXIP; i++)
                {
                    if (common.paip[1, i] > 0)
                    {
                        if (common.paip[1, i] >= 1000 && common.paip[1, i] < 2000)
                        {
                            add = i.ToString() + " " +
                                  common.fslist[common.paip[1, i] - 1000].name + " " +
                                  common.fslist[common.paip[1, i] - 1000].dianshu.ToString() + "点";
                        }
                        else if (common.paip[1, i] >= 2000)
                        {
                            add = i.ToString() + " " +
                                  common.wqlist[common.paip[1, i] - 2000].name + " " +
                                  common.wqlist[common.paip[1, i] - 2000].dianshu.ToString() + "点";
                        }
                        else
                        {
                            add = i.ToString() + " " +
                                  common.xblist[common.paip[1, i]].name + " " +
                                  common.xblist[common.paip[1, i]].dianshu.ToString() + "点";
                        }
                        win.combo.Items.Add(add);
                    }
                }
                win.Show();
            }


        }

        private void gongjMouseDown(object sender, RoutedEventArgs e)
        {
            if (common.mode == 0)
            {
                common.prtext = "对方回合!";
                return;
            }
            if (common.mode == 3)
            {
                MessageBox.Show("请先进行完之前的操作", "pilgrims");
                return;
            }
            if (common.mode != 0)
            {
                if (common.huihes == 1)
                {
                    MessageBox.Show("第一回合无法攻击", "pilgrims");
                    return;
                }
                common.setmode(2, common.gj1, "请选择攻击对象");
                flip();
            }

        }
        private void anotherturn()
        {
            if (common.huihes != 0)
            {
                sendmsg(0);
            }
            common.huihes++;
            string me = "";
            common.sendtext = "对方回合";
            common.mode = 0;
            flip();
            recvmsg();
            common.ndian[1] += 10;
            common.usefa[1] = 0;
            common.usewu[1] = 0;
            common.useji();
            sendmsg(1);
            common.clean("你的回合");
            flip();
        }
        private void backMouseDown(object sender, RoutedEventArgs e)
        {
            if (common.mode == 0 && common.huihes != 0)
            {
                common.prtext = "对方回合!";
                return;
            }
            if (common.mode == 3)
            {
                MessageBox.Show("请先进行完之前的操作", "pilgrims");
                return;
            }
            if (common.mode != 0 || common.huihes == 0)
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
                    mess += common.pwuqi[1].naiju;
                }
                else
                {
                    mess += common.pwuqi[0].naiju;
                }
                shux.Text = mess;
                return;
            }
            string[] tob = the_b.Name.ToString().Split('_');
            var a = new abc(int.Parse(tob[1]), int.Parse(tob[2]), int.Parse(tob[3]));
            var tbing = common.bing[a.a, a.b, a.c];
            if (tbing.bian == 0)
            {
                shux.Text = "空";
                return;
            }
            string mes = "血量:" + tbing.xue.ToString() +
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
        private void bnt_cil(object sender, MouseButtonEventArgs e)
        {

            var the_b = sender as Button;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (common.mode == 0)
                {
                    MessageBox.Show( "对方回合无法攻击", "pilgrims");
                    return;
                }

                else if (common.mode == 2 || common.mode == 3)
                {
                    int tmp;
                    if (the_b.Name.IndexOf("n") >= 0)
                    {
                        tmp = the_b.Name[2] - '0';
                        common.bback(tmp == 1 ? -1 : -2, 0, 0);
                        sendmsg(1);
                        flip();
                        return;
                    }
                    if (the_b.Name.IndexOf("w") >= 0)
                    {

                        tmp = the_b.Name[1] - '0';
                        common.bback(tmp == 1 ? -3 : -4, 0, 0);
                        sendmsg(1);
                        flip();
                        return;
                    }
                    string[] tob = the_b.Name.ToString().Split('_');
                    var a = new abc(int.Parse(tob[1]), int.Parse(tob[2]), int.Parse(tob[3]));
                    common.bback(a.a, a.b, a.c);
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
                if (common.prtext != "" && common.prtext[0]!='\0')
                {
                    System.Windows.Forms.MessageBox.Show(common.prtext, "pilgrims");
                    common.prtext = "";
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
            na[1].Content = common.p[1];
            na[0].Content = common.p[0];
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
                    common.bing[1, i, j] = new xiaobin();
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

                    common.bing[0, i, j] = new xiaobin();
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
            //调试代码区
            //bnts[1, 1, 1].Content = common.xblist[1].name;
            //bnts[0, 1, 1].Content = common.xblist[1].name;
            //common.addxb(19, 1);
            //common.addxb(1, 0);
            //flip();
            //common.bing[1, 1, 1].lingjcs = 1;
            if (common.t_fir == 1)
            {
                ThreadStart startbackget = new ThreadStart(stratbackstart);
                Thread startback = new Thread(startbackget);
                startback.Start();

            }
            else
            {
                common.mode = 1;
                common.huihes++; 
            }

        }
        private void stratbackstart()
        {
            object xoje = new object();
            backMouseDown(xoje, ee);//这个代码是在细节上有问题的,但是能跑就行了
        }
    }
}