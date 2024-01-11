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
        private MembershipManager.DataModel.People.Person? _person;
        private bool _IsEditing;


        public PersonDetail(MembershipManager.DataModel.People.Person? person = null)
        {
            InitializeComponent();
            if (person == null)
            {
                _person = new MembershipManager.DataModel.People.Person();
                _IsEditing = false;
            }
            else
            {
                _person = person;
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
                _person.City = (City)listSelection.List.SelectedItem;
                UpdateGui();
            }
        }
        private void UpdateGui()
        {
            UpdateGui(_person ?? throw new ArgumentNullException());
        }


        public void UpdateGui(object content)
        {
            EntryNoAvs.Text = _person.NoAvs;
            EntryFirstName.Text = _person.FirstName;
            EntryLastName.Text = _person.LastName;
            EntryAddress.Text = _person.Address;
            if (_person.City == null) ButtonCity.Content = "Sélectionner";
            else ButtonCity.Content = _person.City.ToString();
            EntryPhone.Text = _person.Phone;
            EntryMobile.Text = _person.Mobile;
            EntryEmail.Text = _person.Email;
        }
    }
}
