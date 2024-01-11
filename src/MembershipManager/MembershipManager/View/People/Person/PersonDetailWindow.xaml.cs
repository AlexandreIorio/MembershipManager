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
        public PersonDetailWindow(MembershipManager.DataModel.People.Person? p = null)
        {
            InitializeComponent();
            MainFrame.Navigate(new PersonDetail(p));
        }
    }
}
