﻿using System.Windows;

namespace rbr_rt
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            // show main app window
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
