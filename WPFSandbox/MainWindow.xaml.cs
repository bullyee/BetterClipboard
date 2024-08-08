using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPFSandbox
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.WindowState = WindowState.Minimized;

            this.Top = 0;
            this.Left = 0;

            InitializeComponent();
            StartLoop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //String clipboard_text = System.Windows.Clipboard.GetText();
            //System.Console.WriteLine($"current clipboard text: {clipboard_text}");
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(toValue: 0, (Duration)TimeSpan.FromSeconds(0.5));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        private async void StartLoop()
        {
            while (true) {
                await Task.Delay(1000);
                Window new_window = (Window)Application.LoadComponent(new Uri("Window1.xaml", UriKind.Relative));
                Point mp = GetMousePosition();
                Console.WriteLine(mp.ToString());
                new_window.Top = mp.Y/1.5;
                new_window.Left = mp.X/1.5; // adjust to divide by system zoom some time!!
                new_window.Show();
                new_window.Close();
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };
        public static Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);

            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Topmost = true;
        }
    }
}
