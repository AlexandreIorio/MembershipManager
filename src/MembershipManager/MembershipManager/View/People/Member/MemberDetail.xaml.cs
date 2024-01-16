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
        public MembershipManager.DataModel.People.Member? Member { get; set; }
        public MemberDetail(MembershipManager.DataModel.People.Member? member = null)
        {
            InitializeComponent();
            InitializeCombobox();
            if (Member == null)
            {
                Member = new MembershipManager.DataModel.People.Member();
            }
            else
            {
                Member = member;
                int index = ComboboxStructure.Items.IndexOf(Member.Structure);
                //ComboboxStructure.SelectedItem = Se;
            }
            this.DataContext = Member;
        }

        private void InitializeCombobox()
        {
            ComboboxStructure.ItemsSource = ISql.GetAll<MembershipManager.DataModel.Company.Structure>();
        }
    }
}
