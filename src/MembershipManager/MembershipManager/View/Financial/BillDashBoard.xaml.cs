using MembershipManager.DataModel.Buyable;
using MembershipManager.DataModel.Financial;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MembershipManager.View.Financial
{
    /// <summary>
    /// Logique d'interaction pour BillDashBoard.xaml
    /// </summary>
    public partial class BillDashBoard : Window
    {
        public List<BillView>? Bills { get; set; }
        private BillView? SelectedBillView { get { return (BillView?)BillsDataGrid.SelectedItem; } }
        private BillView? LastSelection;

        public MemberAccount? Account
        {
            get
            {
                if (SelectedBillView is null) return null;
                return MemberAccount.Select(SelectedBillView.Account_id) as MemberAccount;
            }
        }

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

        private void BillsDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void BillsDataGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (SelectedBillView is null) return;
            if (LastSelection == SelectedBillView)
            {
                BillsDataGrid.SelectedItem = null;
                LastSelection = null;
            }
            else LastSelection = SelectedBillView;
        }

        private void ButtonPay_Click(object sender, RoutedEventArgs e)
        {
            Bill? bill = (Bill?)Bill.Select(SelectedBillView?.id);
            if (bill is null) return;

            bill.ChangeStatus();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
