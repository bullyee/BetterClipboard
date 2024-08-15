using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
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
using static WPFSandbox.InterceptKeys;
using System.Drawing; // 引用 System.Drawing 命名空間
using System.Windows.Media.Imaging;

namespace WPFSandbox
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.Height = 400;
            this.Width = 200;
            this.Top = SystemParameters.MaximizedPrimaryScreenHeight/2 - this.Height/2;
            this.Left = SystemParameters.MaximizedPrimaryScreenWidth - this.Width;

            InitializeComponent();
            Thread thread = new Thread(detect);
            thread.Start();

            TaskbarIcon tbi = new TaskbarIcon();
            ImageSource img = new BitmapImage(new Uri("pack://application:,,,/WPFSandbox;component//7978227a.ico"));
            tbi.IconSource = img;
            tbi.ToolTipText = "hello world";
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            RemoveClipboardFormatListener(hwndSource.Handle);
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(toValue: 0, (Duration)TimeSpan.FromSeconds(0.5));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        private void ShowPopup()
        {
            Window new_window = (Window)Application.LoadComponent(new Uri("Window1.xaml", UriKind.Relative));
            Point mp = GetMousePosition();
            Console.WriteLine(mp.ToString());
            new_window.Top = mp.Y / 1.5;
            new_window.Left = (mp.X - new_window.Width) / 1.5; // adjust to divide by system zoom some time!!
            new_window.Show();
            new_window.Close();
        }

        // code for monitering 
        private void ClipboardMonitering()
        {
            IDataObject prev_obj = Clipboard.GetDataObject();
            while (true)
            {
                if (Clipboard.IsCurrent(prev_obj) is false)
                {
                    prev_obj = Clipboard.GetDataObject();
                    Console.WriteLine(prev_obj.ToString());
                    ShowPopup();
                }
                Thread.Sleep(50);
            }
        }

        // code for detecting a certain keystate
        [DllImport("user32.dll")]
        public static extern short GetKeyState(int vKey);
        private void detect()
        {
            bool prev_state = GetKeyState(0x11) < 0 && GetKeyState(0xC0) < 0 && GetKeyState(0x12) < 0;
            while (true)
            {
                bool curr_state =  GetKeyState(0x11) < 0  && GetKeyState(0xC0) < 0 && GetKeyState(0x12) < 0;
                if (prev_state != curr_state)
                {
                    prev_state = curr_state;
                    if (curr_state is true)
                    {
                        Dispatcher.Invoke(onClick);
                    }
                }
                Thread.Sleep(50);
            }
        }

        private void onClick()
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Minimized;
            }
            else this.WindowState = WindowState.Normal;
        }



        // code for global mouse position detection
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

        // keeps window at topmost
        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Topmost = true;
        }


        // clipboard hook managements
        private HwndSource hwndSource;
        private const int WM_CLIPBOARDUPDATE = 0x031D;
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            hwndSource.AddHook(WndProc); // Add the hook

            // Register for clipboard notifications
            AddClipboardFormatListener(hwndSource.Handle);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref
 bool handled)
        {
            if (msg == WM_CLIPBOARDUPDATE)
            {
                // Clipboard has been updated
                Dispatcher.Invoke(() =>
                {
                    ShowPopup();
                    TextBlock newitem = new TextBlock{ Text = Clipboard.GetText() };
                    clipboard_stack_panel.Children.Add(newitem);
                });
            }

            return IntPtr.Zero;
        }

        // drag window code
        private DispatcherTimer moveTimer;
        private Point mp;

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (GetKeyState(0x10) < 0)
            {
                Mouse.OverrideCursor = Cursors.ScrollAll;
                StartDrag();
            }
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseDrag();
        }

        private void StartDrag()
        {
            mp = GetMousePosition();

            moveTimer = new DispatcherTimer();
            moveTimer.Interval = TimeSpan.FromMilliseconds(1);
            moveTimer.Tick += WindowFollow;
            moveTimer.Start();
        }

        private void WindowFollow(object sender, EventArgs e)
        {
            if (GetKeyState(0x10) >= 0)
            {
                ReleaseDrag();
            }
            Point curr_mp = GetMousePosition();

            double deltaX = curr_mp.X - mp.X;
            double deltaY = curr_mp.Y - mp.Y;

            Left += deltaX / 1.5;
            Top += deltaY / 1.5;

            mp = curr_mp;
        }

        private void ReleaseDrag()
        {
            if (moveTimer != null)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                moveTimer.Stop();
            }
        }
    }
}
