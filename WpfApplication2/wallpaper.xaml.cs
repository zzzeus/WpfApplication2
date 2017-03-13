using System;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace WpfApplication2
{
    /// <summary>
    /// wallpaper.xaml 的交互逻辑
    /// </summary>
    public partial class wallpaper : Page
    {
       
       

        public wallpaper()
        {
            InitializeComponent();
            Unloaded += Wallpaper_Unloaded;
            //setNotification();
        }

        private void Wallpaper_Unloaded(object sender, RoutedEventArgs e)
        {
            //System.Windows.MessageBox.Show("The page is unloaded");

        }

        private void PLAY_Click(object sender, RoutedEventArgs e)
        {
            //if (choosedFile.Text != null)
            //{
            //    System.Windows.MessageBox.Show("The file is setted!");
            //}
            //Uri iconUri = new Uri("pack://application:,,,/Animation.ico");
            //Icon  i= new Icon("Animation.ico");
            ////(App.Current as App).launchNotification();
            //StringBuilder s = new StringBuilder();
            //s.AppendLine("File: " + iconUri.AbsolutePath);
            //s.AppendLine("Uri: " + iconUri.AbsoluteUri);
            ////s.AppendLine("FileExist: "+ FileExists());
            //choosedFile.Text = "File:"+iconUri.AbsolutePath;
            //(App.Current as App).launchNotification();


            //VideoController.playback("11");
            //VideoController.play();

            VideoWindow v= new VideoWindow();
            v.Show();
        }

        private void STOP_Click(object sender, RoutedEventArgs e)
        {

        }

        private void choose_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                choosedFile.Text = filename;
            }
        }
        
        
    }
}
