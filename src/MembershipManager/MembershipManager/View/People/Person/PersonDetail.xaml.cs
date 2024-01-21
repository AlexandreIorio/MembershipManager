using MembershipManager.DataModel;
using MembershipManager.View.Utils;
using System.Windows;
using System.Windows.Controls;

namespace MembershipManager.View.People.Person
{
    /// <summary>
    /// Logique d'interaction pour PersonDetail.xaml
    /// </summary>
    public partial class PersonDetail : Page
    {
        public MembershipManager.DataModel.People.Person Person { get; set; }

        public PersonDetail(MembershipManager.DataModel.People.Person person)
        {
            InitializeComponent();

            Person = person;
            TextBoxNoAvs.IsEnabled = Person.NoAvs is null;

            this.DataContext = Person;
        }



        private void ButtonCity_Click(object sender, RoutedEventArgs e)
        {
            ListSelection listSelection = new(City.Cities);

            listSelection.List.MouseDoubleClick += (sender, e) =>
            {
                listSelection.DialogResult = true;
                listSelection.Close();
            };

            listSelection.ButtonSelect.Click += (sender, e) =>
            {
                listSelection.DialogResult = true;
                listSelection.Close();
            };

            listSelection.ButtonCancel.Click += (sender, e) =>
            {
                listSelection.DialogResult = false;
                listSelection.Close();
            };

            listSelection.ShowDialog();
            if (listSelection.DialogResult == true)
            {
                Person.City = (City)listSelection.List.SelectedItem;
                ButtonCity.Content = Person.City;

            }
        }

    }
}