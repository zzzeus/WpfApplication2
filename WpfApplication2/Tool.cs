using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;

namespace WpfApplication2
{
    class Tool
    {
        public static string getAEinfo(AutomationElement ae)
        {
            StringBuilder s = new StringBuilder();
            //s.AppendLine("AutomationId: " + ae.Current.AutomationId);
            s.AppendLine("BoundingRectangle: " + ae.Current.BoundingRectangle);
            s.AppendLine("ClassName: " + ae.Current.ClassName);
            //s.AppendLine("ControlType: " + ae.Current.ControlType);
            //s.AppendLine("ProcessId: " + ae.Current.ProcessId);
            s.AppendLine("NativeWindowHandle: " + ae.Current.NativeWindowHandle.ToString("X"));
            s.AppendLine("IsOffscreen: " + ae.Current.IsOffscreen);

            s.AppendLine();
            return s.ToString();
        }

        public static void ScreenInfo(Window w)
        {
            var ss=Screen.AllScreens;
            Debug.WriteLine("Screen num: "+ss.Length);
            foreach (var s in ss)
            {
                w.Height = s.WorkingArea.Height;
                w.Width = s.WorkingArea.Width;
                //w.Width = 1000;
                //w.Height = 1000;
                w.Top = s.WorkingArea.Top;
                w.Left = s.WorkingArea.Left;
                Debug.WriteLine("width: " + s.WorkingArea.Width);
                Debug.WriteLine("Height: " + s.WorkingArea.Height);
            }
            
            Debug.WriteLine("the window screen info: "+w.RestoreBounds);
        }
    }
}
