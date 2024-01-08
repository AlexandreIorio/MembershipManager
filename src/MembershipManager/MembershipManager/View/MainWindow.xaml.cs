using System.Windows;
using MembershipManager.DataModel;
using MembershipManager.DataModel.Person;
using MembershipManager.DataModel.Company;
using MembershipManager.Engine;
using Npgsql;
using MembershipManager.View;

namespace MembershipManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Member p = new Member();
            p.Select("7569854624538");

            //PersonDetail pd1 = new PersonDetail();
            //pd1.Show();


            //Member p = new Member();
            //p.NoAvs = "7566923410409";
            //p.FirstName = "Jean";
            //p.LastName = "Dupont";
            //p.Address = "Rue de la gare 12";
            //p.City = new City(1000);
            //p.Phone = "021 123 45 67";
            //p.MobilePhone = "079 123 45 67";
            //p.Email = "jean.dupont@tatete.ru";
            //p.Structure = new Structure("GoldGym Fitness");
            //p.Insert();

        }
    }
}