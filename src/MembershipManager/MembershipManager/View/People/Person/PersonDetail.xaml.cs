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
        public PersonDetail()
        {
            InitializeComponent();
        }

        private void ButtonCity_Click(object sender, RoutedEventArgs e)
        {
            List<City> cities = ISql.GetAll<City>();
            ListSelection listSelection = new ListSelection(cities);
            listSelection.ShowDialog();
        }
    }
}
