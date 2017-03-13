using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Interop;

namespace WpfApplication2
{
    class win32
    {
        private static IntPtr win32_w2;
        private static IntPtr win32_wI;
        private static Timer aTimer;
        private static Window window;

        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
            SMTO_ERRORONEXIT = 0x20
        }

        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr dc);

        [DllImport("User32.dll")]
        public static extern IntPtr GetWindow(IntPtr hwd, uint u);

        [DllImport("User32.dll")]
        public static extern IntPtr GetParent(IntPtr hwd);
        
        [DllImport("User32.dll")]
        public static extern IntPtr GetAncestor(IntPtr hwd,uint s);

        [DllImport("User32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, IntPtr windowTitle);

        [DllImport("User32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("User32.dll")]
        public static extern IntPtr FindWindow(string classname, string windowname);

        [DllImport("User32.dll")]
        public static extern IntPtr SetParent(IntPtr child, IntPtr parent);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);

        [DllImport("User32.dll")]
        public static extern IntPtr ShowWindow(IntPtr child, int  cmd);

        [DllImport("User32.dll")]
        public static extern int WindowFromPoint(int x, int y);

        [DllImport("User32.dll")]
        public static extern int WindowFromPhysicalPoint(point p);

        [StructLayout(LayoutKind.Sequential)]
        public struct point
        {
            public int x;
            public int y;
        }
        //[DllImport("User32.dll")]
        //public static extern IntPtr SetParent(IntPtr child, IntPtr parent);

        public static void CustomDesktop(Window w)
        {
            w.Closing += (s, e) =>
            {
                if (aTimer!=null)
                {
                aTimer.Stop();
                aTimer.Dispose();
                }
                
            };
            sendmessage();
            window = w;
            Debug.WriteLine("window: " + window);
            var wh = new WindowInteropHelper(window);
            var wIntptr = wh.Handle;
            if (wIntptr==IntPtr.Zero)
            {
                Debug.WriteLine("failed: can't get the child handle!");
                return;
            }
            win32_wI = wIntptr;
            win32_w2= findworkerW();
            if (win32_w2 != IntPtr.Zero)
            {
                SetParent(wIntptr, win32_w2);
                Debug.WriteLine("设置成功！");
            }
            
            //if (w1 != null)
            //{
            //    var a = AutomationElement.FromHandle(w1);
            //    string info = Tool.getAEinfo(a);
            //    Debug.WriteLine(info);
            //}
            //IntPtr w2 = GetWindow(w1,2);
            //if (w2!=null)
            //{
            //    var a=AutomationElement.FromHandle(w2);
            //    string info = Tool.getAEinfo(a);
            //    Debug.WriteLine(info);
            //}
        }

        private static void sendmessage()
        {
            IntPtr progman = FindWindow("Progman", null);

            IntPtr result = IntPtr.Zero;

            SendMessageTimeout(progman,
                                   0x052C,//user code
                                   new IntPtr(0),
                                   IntPtr.Zero,
                                   SendMessageTimeoutFlags.SMTO_NORMAL,
                                   1000,
                                   out result);
        }
        public static void checkout()
        {
            
            var p=GetParent(win32_wI);
            if (p!=win32_w2)
            {
                SetParent(win32_wI, win32_w2);
            }
        }
        public static void dotime()
        {
             aTimer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            checkout();
        }

        public static void show()
        {
            Debug.WriteLine("do show");
            if (window!=null&&win32_wI!=IntPtr.Zero)
            {
                ShowWindow(win32_wI, 8);
            }
            
        }
        public static IntPtr findworkerW()
        {
            IntPtr w2 = IntPtr.Zero;
            IntPtr Progman = FindWindow("Progman", "Program Manager");
            w2 = IntPtr.Zero;
            if ((Progman != null) && Progman != IntPtr.Zero)
            {
                w2 = GetWindow(Progman, 3);
                if (w2 != IntPtr.Zero)
                {
                    var a = AutomationElement.FromHandle(w2);
                    string info = Tool.getAEinfo(a);
                    Debug.WriteLine(info);
                }
            }
            return w2;
        }
        private static IntPtr findworkerW1()
        {
            IntPtr workerw=IntPtr.Zero;
            EnumWindows(new EnumWindowsProc((tophandle, topparamhandle) =>
            {
                IntPtr p = FindWindowEx(tophandle,
                                            IntPtr.Zero,
                                            "SHELLDLL_DefView",
                                            IntPtr.Zero);

                if (p != IntPtr.Zero)
                {
                    workerw = FindWindowEx(IntPtr.Zero,
                                               tophandle,
                                               "WorkerW",
                                               IntPtr.Zero);

                }
                

                return true;
            }), IntPtr.Zero);
            return workerw;
        }
    }
}
