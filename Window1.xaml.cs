using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using pil;

namespace pilgrims
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void comboed(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("changed");
            //pil.common.addxb()
            string[] tmp = combo.SelectedItem.ToString().Split(' ');
            var i = int.Parse(tmp[0]);
            if (common.paip[1, i] == 0)
            {
                MessageBox.Show("此队伍格子没有有效士员编号", "pilgrims");
                return;
            }
            if (common.paip[1, i] >= 1000 && common.paip[1, i] < 2000)
            {
                if (common.ndian[1] - common.fslist[common.paip[1, i] - 1000].dianshu < 0)
                {
                    MessageBox.Show("点数不够", "pilgrims");
                    return;
                }
                if (common.usefa[1] != 0)
                {
                    MessageBox.Show("本回合已经使用过法术", "pilgrims");
                    return;
                }
                common.addfs(common.paip[1, i], 1);
                MainWindow.flip();
            }
            else if (common.paip[1, i] >= 2000)
            {
                if (common.ndian[1] - common.wqlist[common.paip[1, i] - 2000].dianshu < 0)
                {
                    MessageBox.Show("点数不够", "pilgims");
                    return;
                }
                common.addwq(common.paip[1, i], 1);
                MainWindow.flip();
            }
            else
            {
                if (common.ndian[1] - common.xblist[common.paip[1, i]].dianshu < 0)
                {
                    MessageBox.Show("点数不够", "pilgrims");
                    return;
                }
                if (common.k[1, common.xblist[common.paip[1, i]].paishu] >= common.MAXN)
                {
                    MessageBox.Show("此牌拥有太多士员", "pilgrims");
                    return;
                }
                common.ndian[1] -= common.xblist[common.paip[1, i]].dianshu;
                common.addxb(common.paip[1, i], 1);
                MainWindow.flip();
            }
            if (MainWindow.zhu.Text == "购买中") common.sendtext = "购买结束";
            MainWindow.flip();
            Close();

        }
    }
}
