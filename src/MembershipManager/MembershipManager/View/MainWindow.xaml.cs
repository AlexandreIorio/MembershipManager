using System.Windows;
using MembershipManager.DataModel;
using MembershipManager.DataModel.People;
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
           

        }

        private void ButtonTest_Click(object sender, RoutedEventArgs e)
        {
            //Member? p = Member.Select("7569854624538") as Member;


            View.People.Person.PersonDetailWindow pd1 = new();
            pd1.Show();


            //Member p = new Member();
            //p.NoAvs = "7566923410409";
            //p.FirstName = "Jean";
            //p.LastName = "Dupont";
            //p.Address = "Rue de la gare 12";
            //p.City = City.Select(1000) as City;
            //p.Phone = "021 123 45 67";
            //p.MobilePhone = "079 123 45 67";
            //p.Email = "jean.dupont@tatete.ru";
            //p.Structure = Structure.Select("GoldGym Fitness") as Structure;
            //p.Insert();

        }
    }
}