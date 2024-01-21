using MembershipManager.DataModel;
using MembershipManager.DataModel.Company;
using MembershipManager.Engine;
using MembershipManager.Resources;
using Npgsql;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Markup;

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
        private List<Tuple<Action, string>> InitialisationActions = [];

        bool isInitialized = false;

        public App()
        {
            CheckDatabase();
            if (!isInitialized)
            {
                InitialisationActions.Add(new Tuple<Action, string>(() => { InitializeSchema(); }, "Initialisation du schema de la base de donnée"));
                InitialisationActions.Add(new Tuple<Action, string>(() => { InitalizeTables(); }, "Initialisation des tables de la base de donnée"));
                InitialisationActions.Add(new Tuple<Action, string>(() => { InitializeLocation(); }, "Initialisation des villes et cantons"));
            }
            InitialisationActions.Add(new Tuple<Action, string>(() => { Canton.Cantons = ISql.GetAll<Canton>(); }, "Chargement des cantons"));
            InitialisationActions.Add(new Tuple<Action, string>(() => { City.Cities = ISql.GetAll<City>(); }, "Chargement des villes"));
            if (!isInitialized)
            {
                InitialisationActions.Add(new Tuple<Action, string>(() => { InitializeCompany(); }, "Initialisation de la structure et des franchises"));
            }
            InitialisationActions.Add(new Tuple<Action, string>(() => { Settings.Values = (Settings)Settings.Select(Franchise.CurrentFranchise.Id); }, "Chargement des settings"));
        }



        protected override async void OnStartup(StartupEventArgs e)
        {

            InitializeCultures();
            SplashScreen splashScreen = new();
            splashScreen.Show();

            Stopwatch timer = new();
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
                Thread.Sleep(minActionTime);
                return true;
            });

            MainWindow mainWindow = new();
            splashScreen.Close();
            mainWindow.Show();
        }

        private static void InitializeCultures()
        {
            CultureInfo cultureInfo = new("fr-CH");
            cultureInfo.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            cultureInfo.DateTimeFormat.LongDatePattern = "dddd, d MMMM yyyy";
            cultureInfo.DateTimeFormat.ShortTimePattern = "HH:mm";
            cultureInfo.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            cultureInfo.NumberFormat.NumberGroupSeparator = " ";

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        private void CheckDatabase()
        {
            isInitialized = DbManager.Db.CheckSchema();
        }

        private void InitializeSchema()
        {
            NpgsqlCommand cmd = new();
            cmd.Connection = new NpgsqlConnection(DbManager.GetConnectionString(false));
            cmd.CommandText = @"CREATE SCHEMA @schema;";
            cmd.Parameters.AddWithValue("@schema", ConfigurationManager.AppSettings["Schema"]);
        }

        private void InitalizeTables()
        {
            StringBuilder commandText = new(File.ReadAllText("Resources/SQL/database.sql"));
            NpgsqlCommand command = new(DbManager.GetConnectionString());
            command.CommandText = commandText.ToString();
            DbManager.Db.Send(command);

        }

        private void InitializeLocation()
        {
            StringBuilder commandText = new(File.ReadAllText("Resources/SQL/canton.sql"));
            commandText.AppendLine(File.ReadAllText("Resources/SQL/city.sql"));
            NpgsqlCommand command = new(DbManager.GetConnectionString());
            command.CommandText = commandText.ToString();
            DbManager.Db.Send(command);

        }

        private void InitializeCompany()
        {
            City? city = (City?)City.Select(1000);

            if (city is null) throw new KeyNotFoundException();

            new Structure()
            {
                Name = "MySportStructure",
                City = city,
                HeadOfficeAddress = "Avenue Caserne 10"
            }.Insert();

            new Franchise()
            {
                Address = "Avenue Caserne 10",
                City = city,
                StructureName = "MySportStructure"
            }.Insert();
        }

    }

}
