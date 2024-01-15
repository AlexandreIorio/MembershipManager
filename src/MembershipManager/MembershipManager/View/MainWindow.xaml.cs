using System.Windows;
using MembershipManager.DataModel;
using MembershipManager.DataModel.People;
using MembershipManager.DataModel.Company;
using MembershipManager.Engine;
using Npgsql;
using MembershipManager.View;
using MembershipManager.View.Utils.ListSelectionForm;
using MembershipManager.View.Utils;
using MembershipManager.View.People.Person;
using System.Windows.Controls;

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
            //p.SubscriptionDate = DateTime.Now;
            //p.Update();

            Person? p = Person.Select("7569854624538") as Person;

            View.People.Person.PersonDetailWindow pd1 = new View.People.Person.PersonDetailWindow(p);
            pd1.Show();


            //Member p = new Member();
            //Person p = new Person();
            //p.NoAvs = "1";
            //p.FirstName = "Jean";
            //p.LastName = "Dupont";
            //p.Address = "Rue de la gare 12";
            //p.City = City.Select(1000) as City;
            //p.Phone = "021 123 45 67";
            //p.Mobile = "079 123 45 67";
            //p.Email = "jean.dupont@tatete.ru";
            //p.Structure = Structure.Select("GoldGym Fitness") as Structure;
            //p.Insert();

        }

        private void ButtonContact_Click(object sender, RoutedEventArgs e)
        {
            ListSelection listSelection = new ListSelection(ISql.GetAll<Person>());
            listSelection.Width = 800;

            listSelection.MouseDoubleClick += (sender, e) =>
            {
                Person? person = listSelection.List.SelectedItem as Person;
                if (person is null) return;
                PersonDetailWindow personDetailWindow = new PersonDetailWindow(person);
                personDetailWindow.ShowDialog();
                listSelection.List.ItemsSource = ISql.GetAll<Person>();
            };

            //Define new button
            Button button = listSelection.ButtonSelect;
            button.Content = "Nouveau contact";
            button.Click += (sender, e) =>
            {
                PersonDetailWindow personDetailWindow = new PersonDetailWindow();
                personDetailWindow.ShowDialog();
                listSelection.List.ItemsSource = ISql.GetAll<Person>();
            };
            listSelection.ShowDialog();
        }

        private void ButtonMembership_Click(object sender, RoutedEventArgs e)
        {
            ListSelection listSelection = new ListSelection(ISql.GetAll<Member>());
            listSelection.Width = 800;

            listSelection.MouseDoubleClick += (sender, e) =>
            {
                Person? person = listSelection.List.SelectedItem as Person;
                if (person is null) return;
                PersonDetailWindow personDetailWindow = new PersonDetailWindow(person);
                personDetailWindow.ShowDialog();
                listSelection.List.ItemsSource = ISql.GetAll<Person>();
            };

            //Define new button
            Button button = listSelection.ButtonSelect;
            button.Content = "Nouveau contact";
            button.Click += (sender, e) =>
            {
                PersonDetailWindow personDetailWindow = new PersonDetailWindow();
                personDetailWindow.ShowDialog();
                listSelection.List.ItemsSource = ISql.GetAll<Person>();
            };

            listSelection.ShowDialog();

        }
    }
}