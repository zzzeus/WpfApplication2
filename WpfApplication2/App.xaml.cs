using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace WpfApplication2
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : System.Windows.Application
    {
        
        public void launchNotification()
        {
            //Uri u = new Uri("pack://application:,,,/Animation.ico");
            // Configure and show a notification icon in the system tray
            var _notifyIcon = new NotifyIcon
            {
                BalloonTipText = @"Hello, NotifyIcon!",
                Text = @"Hello, NotifyIcon!",

                Icon = new Icon("Animation.ico"),
                //Icon = new Icon(),
                Visible = true
            };
            _notifyIcon.ShowBalloonTip(10000);
        }
       

    }
}
