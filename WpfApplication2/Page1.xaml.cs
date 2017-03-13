using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Automation;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.IO.Packaging;

namespace WpfApplication2
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class Page1 : Page
    {
        private VideoWindow vw;
        Dictionary<int, string> d;
        static AutomationElement root = AutomationElement.RootElement;
        public Page1()
        {
            InitializeComponent();
        }
        [DllImport("User32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr dc);

        //[DllImport("Shobjidl.idl")]
        //static extern IntPtr GetStatus(IntPtr state);

        //SystemParametersInfo

        private void DrawDesk_Click(object sender, RoutedEventArgs e)
        {
            IntPtr desktop = GetDC(IntPtr.Zero);
            using (Graphics g = Graphics.FromHdc(desktop))
            {
                g.FillRectangle(System.Drawing.Brushes.Red, 0, 0, 100, 100);
            }
            ReleaseDC(IntPtr.Zero, desktop);

        }
        private void Draw(int x, int y, int a, int b)
        {
            IntPtr desktop = GetDC(IntPtr.Zero);
            using (Graphics g = Graphics.FromHdc(desktop))
            {
                g.FillRectangle(System.Drawing.Brushes.Red, x, y, x + a, y + b);
            }
            ReleaseDC(IntPtr.Zero, desktop);
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);
        private static UInt32 SPI_SETDESKWALLPAPER = 20;
        private static UInt32 SPIF_UPDATEINIFILE = 0x1;
        //private String imageFileName = "c:\\sample.bmp";

        public void SetImage(string filename)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename, SPIF_UPDATEINIFILE);
        }

        private string getScreenInfo(Screen s)
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("DeviceName:" + s.DeviceName);
            info.AppendLine("Bounds:" + s.Bounds);
            info.AppendLine("BitsPerPixel:" + s.BitsPerPixel);
            info.AppendLine("Primary:" + s.Primary);
            info.AppendLine("WorkingArea:" + s.WorkingArea);
            return info.ToString();
        }
        private string getScreenList()
        {
            string info = "";
            info += "screen length:" + Screen.AllScreens.Length + "\n";
            foreach (var item in Screen.AllScreens)
            {
                info += getScreenInfo(item);

            }
            return info;
        }

        private void show_screenInfo_Click(object sender, RoutedEventArgs e)
        {
            ScreenInfo.Text = getScreenList();
        }

        private void getstatus_Click(object sender, RoutedEventArgs e)
        {
            //ScreenInfo.Text += "\n";
            //IntPtr i = new IntPtr();
            //GetStatus(i);
            //ScreenInfo.Text += i.ToInt32();
        }

        private void Getwindowslist_Click(object sender, RoutedEventArgs e)
        {

            print(root);
            //IntPtr top= win32.GetWindow(new IntPtr(root.Current.NativeWindowHandle), 5);
            //for (int i = 0; i < d.Count; i++)
            //{
            //    ScreenInfo.Text += d[top.ToInt32()];
            //    top=win32.GetNextWindow(top, 2);
            //    if (top == null)
            //    {
            //        break;
            //    }
            //}
        }

        private void GetWin32List_Click(object sender, RoutedEventArgs e)
        {
            IntPtr top = win32.GetWindow(new IntPtr(root.Current.NativeWindowHandle), 5);
            StringBuilder s1 = new StringBuilder();
            
            ScreenInfo.Text += "\n10: " + top.ToInt32() + "\n";
            ScreenInfo.Text += "\n16: " + top.ToInt32().ToString("X") + "\n";

            AutomationElement desk= AutomationElement.FromHandle(new IntPtr(0x10010));
            s1.AppendLine("BoundingRectangle: " + desk.Current.BoundingRectangle);
            s1.AppendLine("ClassName: " + desk.Current.ClassName);

            ScreenInfo.Text += s1.ToString();

            print(desk);
        }

        private void Gettoplevel_Click(object sender, RoutedEventArgs e)
        {
            int num = 0;
            StringBuilder sb = new StringBuilder();
            
            win32.EnumWindows(new win32.EnumWindowsProc((tophandle, topparamhandle) =>
            {
                AutomationElement a1 = AutomationElement.FromHandle(tophandle);
                sb.AppendLine("handle: " + tophandle);
                sb.AppendLine("BoundingRectangle: " + a1.Current.BoundingRectangle);
                sb.AppendLine("ClassName: " + a1.Current.ClassName);
                sb.AppendLine();
                num++;
                return true;
            }), IntPtr.Zero
            );
            sb.AppendLine("num: " + num);
            ScreenInfo.Text += sb.ToString();
        }
        private void print(AutomationElement root)
        {
            AutomationElementCollection ac = root.FindAll(TreeScope.Children, System.Windows.Automation.Condition.TrueCondition);
            ScreenInfo.Text += "Count" + ac.Count + "\n";
            d = new Dictionary<int, string>();
            foreach (AutomationElement ae in ac)
            {
                ScreenInfo.Text+= Tool.getAEinfo(ae);
                //d.Add(ae.Current.NativeWindowHandle, s.ToString());
            }
        }

        private void GetKuGou_Click(object sender, RoutedEventArgs e)
        {
            IntPtr kg= win32.FindWindow("kugou_ui",null);
            if (kg==IntPtr.Zero||kg==null)
            {
                ScreenInfo.Text += "failed to achieve kugou_ui";
                return;
            }
            //AutomationElement a = AutomationElement.FromHandle(kg);
            //ScreenInfo.Text += getAEinfo(a);
            //ScreenInfo.Text += "\nthe parent:\n";
            //IntPtr p = win32.GetParent(IntPtr.Zero);
            // a = AutomationElement.FromHandle(IntPtr.Zero);
            //ScreenInfo.Text += getAEinfo(a);
            //p = win32.GetAncestor(IntPtr.Zero, 1);
            //a = AutomationElement.FromHandle(IntPtr.Zero);
            //ScreenInfo.Text += getAEinfo(a);

        }
        

        private void Notification_Click(object sender, RoutedEventArgs e)
        {
            new Form1();
        }

        private void setDesktop_Click(object sender, RoutedEventArgs e)
        {

            if (vw!=null)
            {
                vw.Close();
                return;
            }
            //Debug.WriteLine(" UriSchemePack :  " + PackUriHelper.UriSchemePack);
            string s = System.IO.Packaging.PackUriHelper.UriSchemePack;
            vw = new VideoWindow();
            vw.Closed += (s3, e3) =>
            {
                Debug.WriteLine("vw is closed");
            };
            App.Current.MainWindow.Closing += (e2, s2)=>{
                if (vw!=null)
                {
                    vw.Close();
                }
                
            };
            vw.Loaded += (s1, e1) =>
            {

                //Tool.ScreenInfo(vw);
                win32.CustomDesktop(vw);
            };
            
            //Uri u = new Uri(CreateAbsolutePathTo("Assets/Media/121.gif"), UriKind.Relative);
            //Uri u = new Uri("ms-appx:///Assets/Media/121.gif", UriKind.Absolute);
            //Uri u1 = new Uri("App.xaml",UriKind.Relative);
            //Uri u = new Uri("pack://application:,,,/Assets/Media/121.gif", UriKind.Absolute);
            Uri u = new Uri("Assets/Media/121.gif", UriKind.Relative);
            //Uri u3 = new Uri("Animation.ico", UriKind.Relative);
            //System.Windows.MessageBox.Show("u.uri: "+u.AbsoluteUri);
            //Debug.WriteLine("URL:" + u);
            //Debug.WriteLine("is file:"+u.IsFile);
            //Debug.WriteLine("path: "+AppDomain.CurrentDomain.BaseDirectory);

            //Debug.WriteLine("helper: " + PackUriHelper.GetPartUri(u));
            vw.MyME.Source = u;
            vw.MyME.Play();
            
            vw.UpdateLayout();
            vw.Show();
            
        }
        private static string CreateAbsolutePathTo(string mediaFile)
        {
            return Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, mediaFile);
        }

        private void showthewindow_Click(object sender, RoutedEventArgs e)
        {
            win32.show();
        }
    }
}
