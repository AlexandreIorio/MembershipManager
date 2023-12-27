using System.Windows;
using MembershipManager.DataModel;
using MembershipManager.DataModel.Person;
using MembershipManager.Engine;
using Npgsql;

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
            //Member member = new Member("7569854624538");


            Person p = new Person();
            p.NoAvs = "7566923410409";
            p.FirstName = "Jean";
            p.LastName = "Dupont";
            p.Address = "Rue de la gare 12";
            p.City = new City(1000);
            p.Phone = "021 123 45 67";
            p.MobilePhone = "079 123 45 67";
            p.Email = "jean.dupont@tatete.ru";
            p.Insert();

        }
    }
}