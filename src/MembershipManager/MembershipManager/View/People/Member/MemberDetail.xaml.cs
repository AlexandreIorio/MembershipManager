using MembershipManager.DataModel.People;
using MembershipManager.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MembershipManager.View.People.Member
{
    /// <summary>
    /// Logique d'interaction pour MemberDetail.xaml
    /// </summary>
    public partial class MemberDetail : Page
    {
        public MemberDetail(MembershipManager.DataModel.People.Member? member)
        {
            InitializeComponent();
            InitializeCombobox();
            ComboboxStructure.SelectedItem = member.Structure;
            this.DataContext = member;
        }

        private void InitializeCombobox()
        {
            ComboboxStructure.ItemsSource = ISql.GetAll<MembershipManager.DataModel.Company.Structure>();
        }
    }
}
