using System.Windows;
using Serilog;

namespace rbr_rt
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            // configure logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Debug()
                .WriteTo.File("logs/rbrrt.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // show main app window
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
