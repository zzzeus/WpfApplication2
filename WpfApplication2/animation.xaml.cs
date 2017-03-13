using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.Globalization;
using System.Speech.Recognition;
using System.Diagnostics;

namespace WpfApplication2
{
    /// <summary>
    /// animation.xaml 的交互逻辑
    /// </summary>
    public partial class animation : Page
    {
        DispatcherTimer timer;

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]//指定坐标处窗体句柄
        public static extern int WindowFromPoint(
           int xPoint,
           int yPoint
       );
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point pt);
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool BringWindowToTop(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point p);

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int x;
            public int y;
        }

        public delegate void NextPrimeDelegate();
        public animation()
        {
            InitializeComponent();
            
            point.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new NextPrimeDelegate(this.dosomething));
            Loaded += (s, e) =>
            {
                Keyboard.Focus(show);
            };
        }
        
        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            // Get the x and y coordinates of the mouse pointer.
            System.Windows.Point position = e.GetPosition(this);
            double pX = position.X;
            double pY = position.Y;

            // Sets the Height/Width of the circle to the mouse coordinates.
            //run((int)pX,(int) pY);
            point.Text = "X:" + pX + " Y:" + pY;
            


        }

        public void dosomething() {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0,0,200);

            timer.Tick += new EventHandler(OnTimedEvent);
            
            timer.IsEnabled = true;
        }

        private  void OnTimedEvent(Object source, EventArgs e)
        {
            System.Drawing.Point p= System.Windows.Forms.Cursor.Position;
            point2.Text = "X:" + p.X+ " Y:" + p.Y;
        }

        private void run1 (int x,int y)
        {
            win32.point p;
            p.x = x;
            p.y = y;
            IntPtr theWindowHandle = new IntPtr(win32.WindowFromPhysicalPoint(p));
            Debug.WriteLine("the window handle: " + theWindowHandle.ToInt32().ToString("X"));
            IntPtr ancestor = win32.GetAncestor(theWindowHandle,1);
            Debug.WriteLine("the ancestor handle: "+ancestor.ToInt32().ToString("X"));
        }
        private void run(int x,int y) {
            win32.point p;
            p.x = x;
            p.y = y;
            IntPtr theWindowHandle=new IntPtr(win32.WindowFromPhysicalPoint(p));
              IntPtr HWND_TOPMOST = new IntPtr(-1);
            const UInt32 SWP_NOSIZE = 0x0001;
            const UInt32 SWP_NOMOVE = 0x0002;
            const UInt32 SWP_SHOWWINDOW = 0x0040;
            hwd.Text = "Hwd: " + theWindowHandle.ToInt32().ToString("X");
            // Call this way:
            //bool b=SetWindowPos(theWindowHandle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
            //bool b = ShowWindow(theWindowHandle, 8);
            //bool b = SetForegroundWindow(theWindowHandle);
            //bool b = BringWindowToTop(theWindowHandle);
            //bool b = IsWindowVisible(theWindowHandle);

            //while (GetParent(theWindowHandle).ToInt64() != 0)
            //{
            //    theWindowHandle = GetParent(theWindowHandle);
            //}
            IntPtr appP;
            if(theWindowHandle==null)
            {
                hwd.Text = "Failed";
                return;
            }
            do
            {
                appP = theWindowHandle;
                theWindowHandle = GetParent(theWindowHandle);
            } while (theWindowHandle.ToInt64() != 0);
            hwd.Text += " \nhandle:" + appP.ToInt64().ToString("X");
            bool b = SetWindowPos(appP, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
            Debug.WriteLine("set the window position:  "+b);
            if (!b)
            {
                hwd.Text += "\nFailed to set the position";
            }
            //IntPtr desktopP = GetDesktopWindow();
            //IntPtr parent= GetParent(theWindowHandle);
            //IntPtr parent1 = GetParent(parent);
            //hwd.Text += "  desktop:"+ desktopP.ToInt64()+" parent:"+parent.ToInt64()+" grandparent:"+parent1;
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Point p = System.Windows.Forms.Control.MousePosition;
            run(p.X, p.Y);
            //hwd.Text = p.X+"   "+ p.Y;
        }

        private void openFile_Click(object sender, RoutedEventArgs e)
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
                //dlg.OpenFile();
                openFile.Content = filename;
                get_content(filename);
            }
        }
        private  void get_content(string filename)
        {
            StreamReader s;
            try
            {
                using (s=new StreamReader(filename,Encoding.GetEncoding("shift-jis")))
                {
                    TextInFile.Text = s.ReadLine();

                }
            }
            catch (Exception)
            {

                TextInFile.Text = "Failed to achieve";
            }
            
        }

        private  void Speak_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TextInFile.Text))
            {
                using (SpeechSynthesizer synth = new SpeechSynthesizer())
                {
                    synth.SetOutputToDefaultAudioDevice();
                    synth.SelectVoiceByHints(VoiceGender.Female,VoiceAge.Child,20,CultureInfo.GetCultureInfo(0x00000411));
                    synth.SpeakAsync(TextInFile.Text);


                    foreach (InstalledVoice voice in synth.GetInstalledVoices())
                    {
                        VoiceInfo info = voice.VoiceInfo;
                        string AudioFormats = "";
                        foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                        {
                            AudioFormats += String.Format("{0}\n",
                            fmt.EncodingFormat.ToString());
                        }

                        Info.Text+=(" \nName:          " + info.Name);
                        Info.Text += ("\n Culture:       " + info.Culture);
                        Info.Text += (" \nAge:           " + info.Age);
                        Info.Text += (" \nGender:        " + info.Gender);
                        Info.Text += (" \nDescription:   " + info.Description);
                        Info.Text += (" \nID:            " + info.Id);
                        Info.Text += (" \nEnabled:       " + voice.Enabled);
                        if (info.SupportedAudioFormats.Count != 0)
                        {
                            Info.Text += ("\n Audio formats: " + AudioFormats);
                        }
                        else
                        {
                            Info.Text += (" \nNo supported audio formats found");
                        }

                        string AdditionalInfo = "";
                        foreach (string key in info.AdditionalInfo.Keys)
                        {
                            AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
                        }

                        Info.Text += ("\n Additional Info - " + AdditionalInfo);

                        //// Retrieve the DrawingContext in order to draw into the visual object.
                        //DrawingContext drawingContext = drawingVisual.RenderOpen();

                        //// Draw a rectangle into the DrawingContext.
                        //Rect rect = new Rect(new Point(160, 100), new Size(320, 80));
                        //drawingContext.DrawRectangle(Brushes.LightBlue, (Pen)null, rect);

                        //// Draw a formatted text string into the DrawingContext.
                        //drawingContext.DrawText(
                        //   new FormattedText("Hello, world",
                        //CultureInfo.GetCultureInfo("en-us"),
                        //      FlowDirection.LeftToRight,
                        //      new Typeface("Verdana"),
                        //      36, Brushes.Black),
                        //      new Point(200, 116));

                        //// Persist the drawing content.
                        //drawingContext.Close();

                    }
                }

            }
        
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            //using (SpeechSynthesizer synth = new SpeechSynthesizer())
            //{

            //    // Configure the audio output. 
            //    synth.SetOutputToDefaultAudioDevice();

            //    // Create a PromptBuilder object and define the data types for some of the added strings.
            //    PromptBuilder sayAs = new PromptBuilder();
            //    sayAs.AppendText("Your");
            //    sayAs.AppendTextWithHint("1st", SayAs.NumberOrdinal);
            //    sayAs.AppendText("request was for");
            //    sayAs.AppendTextWithHint("1", SayAs.NumberCardinal);
            //    sayAs.AppendText("room, on");
            //    sayAs.AppendTextWithHint("10/19/2012,", SayAs.MonthDayYear);
            //    sayAs.AppendText("with early arrival at");
            //    sayAs.AppendTextWithHint("12:35pm", SayAs.Time12);

            //    // Speak the contents of the SSML prompt.
            //    synth.Speak(sayAs);
            //}

            using (SpeechRecognizer recognizer = new SpeechRecognizer())
            {
                // Create and load a sample grammar.
                Grammar testGrammar =
                  new Grammar(new GrammarBuilder("testing testing"));
                testGrammar.Name = "Test Grammar";

                recognizer.LoadGrammar(testGrammar);

                RecognitionResult result;

                // This EmulateRecognize call matches the grammar and returns a
                // recognition result.
                result = recognizer.EmulateRecognize("testing testing");
                if(result!=null)
                TextInFile.Text = result.Text;

                // This EmulateRecognize call does not match the grammar and 
                // returns null.
                result = recognizer.EmulateRecognize("testing one two three");
                if (result != null)
                    TextInFile.Text += result.Text;
            }


        }

        private void goto_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("Page1.xaml", UriKind.Relative);
            this.NavigationService.Navigate(uri);
        }

        private void gotowallpaper_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("wallpaper.xaml", UriKind.Relative);
            this.NavigationService.Navigate(uri);
        }

        private void gotovideo_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("videopage.xaml", UriKind.Relative);
            this.NavigationService.Navigate(uri);
        }
    }
}
