using MembershipManager.DataModel.Person;
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


namespace MembershipManager.View
{
    /// <summary>
    /// Logique d'interaction pour PersonDetail.xaml
    /// </summary>
    public partial class PersonDetail : Window
    {
        private Person _person;

        public bool ValideFk = false;
        public PersonDetail(Person? person = null)
        {
            InitializeComponent();
            if (person is null)
            {
                _person = new Person();
            } else
            {
                _person = person;
                ValideFk = true;
                FillGui();
            }

        }





        private void FillGui()
        {
            
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void TbAvs_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TbLastNAme_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void TbFirstName_LostFocus_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
