using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApplication2
{
    class VideoController
    {
        private static MediaElement me;
        private static Window w;
        private static Uri videoUri;
        public static void playback(string v)
        {
            if (w == null)
            {
                init();
            }
            Grid g = new Grid();
            g.Background = Brushes.Blue;
            w.Content = g;
            g.Children.Add(me);
            me.Height = 200;
            me.Width = 200;
            me.LoadedBehavior = MediaState.Manual;
            me.UnloadedBehavior = MediaState.Close;
            videoUri = new Uri(@"pack://application:,,,/Assets/Media/climb.mp3");
            if (videoUri == null)
            { release(); }
            else
            {
                me.Source = videoUri;
                w.Show();
            }

        }
        public static void play()
        {
            if (me!=null)
            {
                
                me.Play();
            }
        }
        public static void pause()
        {
            if (me != null&&me.CanPause)
            {
                me.Pause();
            }
        }
        public static void resume()
        {
            if (me != null)
            {
                me.Play();
            }
        }
        public static void init()
        {
            me = new MediaElement();
            w = new Window();
        }
        public static void release()
        {
            if (w!=null)
            {
                me.Close();
                w.Close();
            }
        }
    }
}
