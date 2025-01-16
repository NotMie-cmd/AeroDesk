using System;
using System.Windows;

namespace AeroDesk
{
    public partial class ControlPanelWindow : Window
    {
        private MainWindow mainWindow;

        // Constructor
        public ControlPanelWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        // Event handler for the Apply button click
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            // Get blur intensity from slider
            int blurIntensity = (int)BlurIntensitySlider.Value;

            // Get overlay option from checkbox
            bool isOverlay = OverlayCheckBox.IsChecked ?? false;
            bool isShown = ShowOnTaskbar.IsChecked ?? false;

            // Apply the changes to the main window
            mainWindow.ApplyBlurEffect(blurIntensity);
            mainWindow.SetOverlayMode(isOverlay);
            //mainWindow.SetTaskbar(isShown);

            // Close the control panel window
            this.Close();
        }
    }
}
