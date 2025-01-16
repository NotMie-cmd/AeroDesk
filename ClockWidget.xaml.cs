using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AeroDesk.Widget
{
    public partial class ClockWidget : UserControl
    {
        public ClockWidget()
        {
            InitializeComponent();  // Initializes the components defined in ClockWidget.xaml
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                ClockDisplay.Text = DateTime.Now.ToString("hh:mm:ss tt");
            };
            timer.Start();
        }
    }
}
