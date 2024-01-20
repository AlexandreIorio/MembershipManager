using MembershipManager.DataModel.Financial;
using System.Data;
using System.Windows;

namespace MembershipManager.View.Financial
{
    /// <summary>
    /// Logique d'interaction pour BillDashBoard.xaml
    /// </summary>
    public partial class BillDashBoard : Window
    {
        public List<BillView>? Bills { get; set; }
        public MemberAccount Account { get; private set; }
        public BillDashBoard()
        {
            InitializeComponent();
            RefreshTransactions();
        }

        private void RefreshTransactions()
        {
            Bills = Bill.Views()?.Cast<BillView>().ToList();
            BillsDataGrid.ItemsSource = Bills;
        }
    }
}
