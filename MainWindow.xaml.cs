using pil;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using System.Net.Sockets;

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
        public MainWindow()
        {
            /*
            host = Interaction.InputBox("输入IP地址", "pilgrims", "");
            port = int.Parse(Interaction.InputBox("输入连接端口", "pilgrims", ""));
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndPoint实例
            Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket
            c.Connect(ipe);//连接到服务器
            */

            //tmpname1 = Interaction.InputBox("输入名称", "pilgrims", "");
            //调试代码区
            common.mode = 1;
            common.t_fir = 1;
            //调试代码结束
            Title = "pilgrims" + common.BAN + " made by cmach_socket";
            InitializeComponent();
            common.init();

        }
        public static void flip()
        {
            for (int i = 0; i < common.MAXPLAY; i++)
            {
                for (int j = 1; j <= common.MAXP - 1; j++)
                {
                    for (int l = 1; l < common.MAXN; l++)
                    {
                        bnts[i, j, l].Content = "空";
                        if (common.vis[i, j, l] != 0)
                        {
                            bnts[i, j, l].Content = common.bing[i, j, l].name;
                        }
                    }
                }
            }
            zhu.Text = common.sendtext;
            for (int i = 0; i < common.MAXPLAY; i++)
            {
                xu[i].Text = "HP:" + common.pxue[i].ToString();
                w[i].Content = common.pwuqi[i].name.ToString();
                cost[i].Text = "COST:" + common.ndian[i].ToString();
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void seeMouseDown(object sender, RoutedEventArgs e)
        {
            if (common.mode != 0 && common.mode != 3)
            {
                common.clean("状态");
                //common.mode = 1;
                flip();
            }
        }

        private void buyMouseDown(object sender, RoutedEventArgs e)
        {

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
            if (common.mode == 3)
            {
                MessageBox.Show("请先进行完之前的操作", "pilgrims");
                return;
            }
            if (common.mode != 0)
            {
                if (common.t_fir == 0)
                {
                    MessageBox.Show("第一回合无法攻击", "pilgrims");
                    return;
                }
                common.setmode(2, common.gj1, "请选择攻击对象");
                flip();
            }

        }
        private void backMouseDown(object sender, RoutedEventArgs e)
        {

            if (common.mode == 3)
            {
                MessageBox.Show("请先进行完之前的操作", "pilgrims");
                return;
            }
            if (common.mode != 0)
            {
                string me="";
                common.sendtext = "对方回合";
                common.mode = 0;
                flip();
                //begin
                c.Receive(common.get_b);
                common.blong = (int)(common.get_b[1] | common.get_b[2] << 8 | common.get_b[3] << 16 | common.get_b[4] << 24);
                common.ilong = common.btoi(ref common.put_i, common.get_b, common.blong);
                int j;
                for (j = 2; common.put_i[j] != -6; j++)
                {
                    int i = common.put_i[j];
                    if (i == 1)
                    {
                        j++;
                        i = common.put_i[j];
                        if (common.paip[1, i] >= 1000 && common.paip[1, i] < 2000)
                        {
                            me += "使用了" + common.fslist[i - 1000].name + "\n";
                        }
                        else if (common.paip[1, i] >= 2000)
                        {
                            me += "购买了" + common.wqlist[i - 2000].name + "\n";
                        }
                        else
                        {
                            me += "购买了" + common.xblist[i].name + "\n";
                        }
                    }
                    else if (i == 2)
                    {
                        j++;
                        i = common.put_i[j];
                        if (common.paip[1, i] >= 2000)
                        {
                            me += common.wqlist[i - 2000].name;
                        }
                        else
                        {
                            me += common.xblist[i].name;
                        }
                        j++;
                        i = common.put_i[j];
                        me += "攻击了";
                        if (common.paip[1, i] >= 2000)
                        {
                            me += common.wqlist[i - 2000].name + "\n";
                        }
                        else
                        {
                            me += common.xblist[i].name + "\n";
                        }
                    }
                    for(int f = 0; f <= common.MAXPLAY; f++)
                    {
                        common.pxue[f] = common.put_i[++j];
                        common.ndian[f] = common.put_i[++j];
                        common.pwuqi[f] = common.wqlist[common.put_i[++j]].copy();
                        common.pwuqi[f].naiju = common.put_i[++j];
                    }
                   
                    while( j < common.ilong)
                    {
                        int a=common.put_i[++j],b=common.put_i[++j],c=common.put_i[++j];
                        common.bing[a, b, c].gongji = common.put_i[++j];
                        common.bing[a, b, c].fanci = common.put_i[++j];
                        common.bing[a, b, c].xue = common.put_i[++j];
                        common.bing[a, b, c].gjcishu = common.put_i[++j];
                        common.bing[a, b, c].shecheng = common.put_i[++j];
                        common.bing[a, b, c].dun = common.put_i[++j];
                        common.bing[a, b, c].lingjcs = common.put_i[++j];
                        common.bing[a, b, c].dianshu = common.put_i[++j];
                        common.bing[a, b, c].xixue = common.put_i[++j];
                        common.bing[a, b, c].boolpojia = common.put_i[++j];
                        common.bing[a, b, c].paishu = common.put_i[++j];
                        common.bing[a, b, c].qianfeng = common.put_i[++j];
                        common.bing[a, b, c].boolji = common.put_i[++j];
                        common.bing[a, b, c].bihu = common.put_i[++j];
                        common.bing[a, b, c].dianji = common.put_i[++j];
                        common.bing[a, b, c].bian = common.put_i[++j];
                        common.bing[a, b, c].maxxue = common.put_i[++j];
                        common.bing[a, b, c].tmp= common.put_i[++j];
                    }
                    
                }
                //end
                common.mode = 1;
                common.ndian[1] += 10;
                common.usefa[1] = 0;
                common.usewu[1] = 0;
                common.useji();

            }
        }

        //mode 0 对方回合
        //mode 1 空闲中
        //mode 2
        private void bnt_cil(object sender, MouseButtonEventArgs e)
        {

            var the_b = sender as Button;
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (the_b.Name.IndexOf("n") >= 0)
                {
                    MessageBox.Show(the_b.Content.ToString(), "pilgirms");
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
                        mess += common.pwuqi[2].naiju;
                    }
                    MessageBox.Show(mess, "pilgirms");
                    return;
                }
                string[] tob = the_b.Name.ToString().Split('_');
                var a = new abc(int.Parse(tob[1]), int.Parse(tob[2]), int.Parse(tob[3]));
                var tbing = common.bing[a.a, a.b, a.c];
                if (tbing.bian == 0)
                {
                    MessageBox.Show("空", "pilgirms");
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
                MessageBox.Show(mes, "pilgrims");
            }
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (common.mode == 0)
                {
                    MessageBox.Show(common.p[0] + "对方回合无法攻击", "pilgrims");
                    return;
                }

                else if (common.mode == 2 || common.mode == 3)
                {
                    int tmp;
                    if (the_b.Name.IndexOf("n") >= 0)
                    {
                        tmp = the_b.Name[2] - '0';
                        common.bback(tmp == 1 ? -1 : -2, 0, 0);
                        flip();
                        return;
                    }
                    if (the_b.Name.IndexOf("w") >= 0)
                    {

                        tmp = the_b.Name[1] - '0';
                        common.bback(tmp == 1 ? -3 : -4, 0, 0);
                        flip();
                        return;
                    }
                    string[] tob = the_b.Name.ToString().Split('_');
                    var a = new abc(int.Parse(tob[1]), int.Parse(tob[2]), int.Parse(tob[3]));
                    common.bback(a.a, a.b, a.c);
                    flip();
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
            na[1].Content = tmpname1;
            na[0].Content = tmpname2;
            na[1].AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(bnt_cil));
            na[0].AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(bnt_cil));
            w[1].AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(bnt_cil));
            w[0].AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(bnt_cil));
            Panel.SetZIndex(buy, 10);
            for (int i = common.MAXP - 1, l = 1; i > 0; i--, l++)
            {
                for (int j = 1; j <= common.MAXN - 1; j++)
                {
                    common.bing[1, i, j] = new xiaobin();
                    Button bnt = new Button { Name = "B_1_" + i.ToString() + "_" + j.ToString() };
                    bnt.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(bnt_cil));
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
            bnts[1, 1, 1].Content = common.xblist[1].name;
            bnts[0, 1, 1].Content = common.xblist[1].name;
            common.addxb(19, 1);
            common.addxb(1, 0);
            flip();
            common.bing[1, 1, 1].lingjcs = 1;
            //
        }
    }
}