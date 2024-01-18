using MembershipManager.DataModel.People;
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
using System.Windows.Shapes;

namespace MembershipManager.View.People.Person
{
    /// <summary>
    /// Logique d'interaction pour PersonDetailWindow.xaml
    /// </summary>
    public partial class PersonDetailWindow : Window
    {
        private bool _IsEditMode = false;
        private PersonDetail _PersonDetail;
        public PersonDetailWindow(MembershipManager.DataModel.People.Person? p = null)
        {
            InitializeComponent();
            _PersonDetail = new PersonDetail(p);
            MainFrame.Navigate(_PersonDetail);
            _IsEditMode = p is not null;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            MembershipManager.DataModel.People.Person person = _PersonDetail.Person;
            if (_IsEditMode)
            {
                person.Update();
            }
            else
            {
                person.Insert();
            }
            Close();
        }
    }
}
