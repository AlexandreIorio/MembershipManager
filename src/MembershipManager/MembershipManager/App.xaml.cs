using MembershipManager.DataModel;
using MembershipManager.Engine;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace MembershipManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
#if DEBUG
        private const int splashScreenDelay = 0;
#else
        private const int splashScreenDelay = 3000;
#endif
        private List<Tuple<Action, string>> InitialisationActions = new List<Tuple<Action, string>>();

        public App()
        {
            InitialisationActions.Add(new Tuple<Action, string>(() => { Canton.Cantons = ISql.GetAll<Canton>(); }, "Chargement des cantons"));
            InitialisationActions.Add(new Tuple<Action, string>(() => { City.Cities = ISql.GetAll<City>(); }, "Chargement des villes"));
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            SplashScreen splashScreen = new SplashScreen();
            splashScreen.Show();

            Stopwatch timer = new Stopwatch();
            timer.Start();

            int minActionTime = splashScreenDelay / (InitialisationActions.Count + 1);
            int increment = 100 / (InitialisationActions.Count + 1);
            await Task.Run(() =>
            {
                foreach (Tuple<Action, string> action in InitialisationActions)
                {
                    long entryActionTime = timer.ElapsedMilliseconds;


                    splashScreen.Dispatcher.Invoke(() =>
                    {
                        splashScreen.ProgressMessage.Content = action.Item2;
                        splashScreen.progressBar.Value += increment;
                    });

                    action.Item1.Invoke();

                    long actionTime = timer.ElapsedMilliseconds - entryActionTime;
                    if (actionTime < minActionTime)
                    {
                        Thread.Sleep((int)(minActionTime - actionTime));
                    }
                }
                splashScreen.Dispatcher.Invoke(() => splashScreen.progressBar.Value = 100);
                Thread.Sleep((int)minActionTime);
                return true;
            });

            splashScreen.Close();
            mainWindow.Show();
        }

        private void LoadInitialisationActions()
        {

        }

    }

}
