using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace MembershipManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const int splashScreenDelay = 2000;

        protected override void OnStartup(StartupEventArgs e)
        {
            SplashScreen splashScreen = new SplashScreen();
            splashScreen.Show();
            Stopwatch timer = new Stopwatch();
            timer.Start();
            base.OnStartup(e);
            timer.Stop();
            int remainingTimeToShowSplashScreen = splashScreenDelay - (int)timer.ElapsedMilliseconds;
            if (remainingTimeToShowSplashScreen > 0)
            {
                System.Threading.Thread.Sleep(remainingTimeToShowSplashScreen);
            }
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            splashScreen.Close();
        }
    }

}
