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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication2
{
    /// <summary>
    /// videopage.xaml 的交互逻辑
    /// </summary>
    public partial class videopage : Page
    {
        public videopage()
        {
            InitializeComponent();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyME.CanPause)
            {
                MyME.Stop();
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            MyME.Play();
        }

        private void MyME_MediaEnded(object sender, RoutedEventArgs e)
        {
            MyME.Position = new TimeSpan(0, 0, 1);
            MyME.Play();
        }
    }
}
