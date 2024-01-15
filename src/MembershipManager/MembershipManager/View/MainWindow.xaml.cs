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
            var PersonView = Person.Views.Cast<PersonView>().ToList();

            int i = 0;
           

        }

        private void ButtonTest_Click(object sender, RoutedEventArgs e)
        {



        }

        private void ButtonContact_Click(object sender, RoutedEventArgs e)
        {
            ListSelection listSelection = new ListSelection(Person.Views.Cast<PersonView>().ToList());
            listSelection.Width = 800;

            listSelection.List.MouseDoubleClick += (sender, e) =>
            {
                string? noAvs = (listSelection.List.SelectedItem as PersonView)?.no_avs;
                if (noAvs is null) return;
                PersonView.EditPerson(noAvs);
                listSelection.List.ItemsSource = Person.Views.Cast<PersonView>().ToList();
            };

            //Define new button
            Button button = listSelection.ButtonSelect;
            button.Content = "Nouveau contact";
            button.Click += (sender, e) =>
            {
                PersonView.NewPerson();
                listSelection.List.ItemsSource = Person.Views.Cast<PersonView>().ToList();
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