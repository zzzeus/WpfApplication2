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

namespace WpfApplication2
{
    /// <summary>
    /// VideoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class VideoWindow : Window
    {
        public VideoWindow()
        {
            Tool.ScreenInfo(this);
            InitializeComponent();

        }

        private void MyME_MediaEnded(object sender, RoutedEventArgs e)
        {
            MyME.Position = new TimeSpan(0, 0, 1);
            MyME.Play();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void show_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CanPause: " + MyME.CanPause);
            sb.AppendLine("Source: " + MyME.Source);
            
            MessageBox.Show(sb.ToString());
        }
    }
}
