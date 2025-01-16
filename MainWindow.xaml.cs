using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace AeroDesk
{
    public partial class MainWindow : Window
    {
        // Importing the DWM API for blur effect
        [DllImport("dwmapi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

        // Structure for the margins
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;

            public MARGINS(int all)
            {
                Left = Right = Top = Bottom = all;
            }
        }

        // Variables for handling window drag
        private bool isDragging = false;
        private Point clickPosition;

        // Initialize the main window and apply the blur effect
        public MainWindow()
        {
            InitializeComponent();
            ApplyBlurEffect(5); // Default blur intensity

            // Set initial window properties
            this.ShowInTaskbar = true;
            this.Topmost = true;
            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.Background = Brushes.Transparent;
            this.Left = 100;
            this.Top = 100;
            this.Width = 300;
            this.Height = 200;

            // Set event handlers for right-click menu and dragging
           // this.MouseRightButtonUp += MainWindow_MouseRightButtonUp;
            this.MouseMove += MainWindow_MouseMove;
            this.MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;

            // For window dragging, you need to handle MouseLeftButtonDown event
            this.MouseLeftButtonDown += Window_MouseLeftButtonDown; 
        }

        // Event to initiate dragging
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            clickPosition = e.GetPosition(null);
            this.CaptureMouse();
        }

        // Event to stop dragging
        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            this.ReleaseMouseCapture();
        }

        // Event for dragging the window
        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                // Move the window with the mouse cursor
                var currentPosition = e.GetPosition(null);
                var offset = currentPosition - clickPosition;
                this.Left += offset.X;
                this.Top += offset.Y;
                clickPosition = currentPosition;
            }
        }

        // Apply blur effect based on intensity value (from the slider)
        public void ApplyBlurEffect(int intensity)
        {
            MARGINS margins = new MARGINS(intensity);
            DwmExtendFrameIntoClientArea(new System.Windows.Interop.WindowInteropHelper(this).Handle, ref margins);
        }

        // Set whether the widget overlays on top of all other windows
        public void SetOverlayMode(bool isOverlay)
        {
            if (isOverlay)
            {
                this.Topmost = true; // Always on top
            }
            else
            {
                this.Topmost = false; // Normal desktop window behavior
            }
        }

        public void SetTaskbar(bool isShown)
        {
            // The same as SetOverlayMode, but with the Taskbar
            if (isShown)
            {
                this.ShowInTaskbar = true;
            }
            else
            {
                this.ShowInTaskbar = false;
            }
        }

        // Mouse event handler for right-click to open the context menu
        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Show the context menu when right-clicked
            WidgetContextMenu.IsOpen = true;
        }

        // Event handler for "Exit" menu item in the context menu
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Close the application when "Exit" is clicked
            Application.Current.Shutdown();
        }

        // Event handler for "Control Panel" menu item to show control panel
        private void ControlPanelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Show the control panel window (for changing blur and overlay settings)
            ControlPanelWindow controlPanel = new ControlPanelWindow(this);
            controlPanel.Show();
        }
    }
}
