using MembershipManager.Engine;
using System.Windows.Controls;

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
