using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MembershipManager.DataModel;
using MembershipManager.DataModel.People;
using MembershipManager.Engine;
using MembershipManager.View.Utils;
using MembershipManager.View.Utils.ListSelectionForm;

namespace MembershipManager.View.People.Person
{
    /// <summary>
    /// Logique d'interaction pour PersonDetail.xaml
    /// </summary>
    public partial class PersonDetail : Page
    {
        public MembershipManager.DataModel.People.Person Person { get; set; }


        public PersonDetail() => InitializeComponent();
        public PersonDetail(MembershipManager.DataModel.People.Person person)
        {
            InitializeComponent();

            Person = person;
            TextBoxNoAvs.IsEnabled = Person.NoAvs is null;

            this.DataContext = Person;
        }



        private void ButtonCity_Click(object sender, RoutedEventArgs e)
        {
            ListSelection listSelection = new ListSelection(City.Cities);

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