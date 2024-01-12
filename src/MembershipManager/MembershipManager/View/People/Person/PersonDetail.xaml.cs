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
    public partial class PersonDetail : Page, IGui
    {
        public MembershipManager.DataModel.People.Person Person { get; set; }
        private bool _IsEditing;


        public PersonDetail(MembershipManager.DataModel.People.Person? person = null)
        {
            InitializeComponent();
            if (person == null)
            {
                Person = new MembershipManager.DataModel.People.Person();
                _IsEditing = false;
            }
            else
            {
                Person = person;
                _IsEditing = true;
            }
            UpdateGui();
        }

        

        private void ButtonCity_Click(object sender, RoutedEventArgs e)
        {
            ListSelection listSelection = new ListSelection(City.Cities);
            listSelection.ShowDialog();
            if (listSelection.DialogResult == true)
            {
                Person.City = (City)listSelection.List.SelectedItem;
                UpdateGui();
            }
        }
        private void UpdateGui()
        {
            UpdateGui(Person ?? throw new ArgumentNullException());
        }


        public void UpdateGui(object content)
        {
            EntryNoAvs.Text = Person.NoAvs ?? "";
            EntryFirstName.Text = Person.FirstName ?? "" ;
            EntryLastName.Text = Person.LastName ?? "";
            EntryAddress.Text = Person.Address ?? "";
            if (Person.City == null) ButtonCity.Content = "Sélectionner";
            else ButtonCity.Content = Person.City.ToString();
            EntryPhone.Text = Person.Phone ?? "";
            EntryMobile.Text = Person.Mobile ?? "";
            EntryEmail.Text = Person.Email ?? "";
        }
    }
}