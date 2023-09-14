using pil;
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
            var _mainWindow = Application.Current.Windows
            .Cast<Window>()
            .FirstOrDefault(window => window is MainWindow) as MainWindow;
            string[] tmp = combo.SelectedItem.ToString().Split(' ');
            var i = int.Parse(tmp[0]);
            if (_mainWindow.qi.paip[1, i] == "")
            {
                _mainWindow.qi.prtext = "此队伍格子没有有效士员编号";
                common.clean(ref _mainWindow.qi, "状态");
                return;
            }
            if (_mainWindow.qi.listty[_mainWindow.qi.paip[1, i]] == "fashu")
            {
                if (_mainWindow.qi.ndian[1] - _mainWindow.qi.fslist[_mainWindow.qi.paip[1, i]].dianshu < 0)
                {
                    _mainWindow.qi.prtext = "点数不够";
                    common.clean(ref _mainWindow.qi, "状态");
                    return;
                }
                if (_mainWindow.qi.usefa[1] != 0)
                {
                    _mainWindow.qi.prtext = "本回合已经使用过法术";
                    common.clean(ref _mainWindow.qi, "状态");
                    return;
                }
                common.addfs(ref _mainWindow.qi,_mainWindow.qi.paip[1, i], 1);
                _mainWindow.flip();
            }
            else if (_mainWindow.qi.listty[_mainWindow.qi.paip[1, i]] == "wuqi")
            {
                if (_mainWindow.qi.ndian[1] - _mainWindow.qi.wqlist[_mainWindow.qi.paip[1, i]].dianshu < 0)
                {
                    _mainWindow.qi.prtext = "点数不够";
                    common.clean(ref _mainWindow.qi, "状态");
                    return;
                }
                common.addwq(ref _mainWindow.qi,_mainWindow.qi.paip[1, i], 1);
                _mainWindow.flip();
            }
            else if (_mainWindow.qi.listty[_mainWindow.qi.paip[1, i]] == "xiaobin")
            {
                if (_mainWindow.qi.ndian[1] - _mainWindow.qi.xblist[_mainWindow.qi.paip[1, i]].dianshu < 0)
                {
                    _mainWindow.qi.prtext = "点数不够";
                    common.clean(ref _mainWindow.qi, "状态");
                    return;
                }
                if (_mainWindow.qi.k[1, _mainWindow.qi.xblist[_mainWindow.qi.paip[1, i]].paishu] >= common.MAXN)
                {
                    _mainWindow.qi.prtext = "此牌拥有太多士员";
                    common.clean(ref _mainWindow.qi,"状态");
                    return;
                }
                _mainWindow.qi.ndian[1] -= _mainWindow.qi.xblist[_mainWindow.qi.paip[1, i]].dianshu;
                common.addxb(ref _mainWindow.qi,_mainWindow.qi.paip[1, i], 1);
                _mainWindow.flip();
            }
            if (_mainWindow.zhu.Text == "购买中") _mainWindow.qi.sendtext = "购买结束";
            _mainWindow.flip();
            _mainWindow.sendmsg(1);
            Close();

        }
    }
}
