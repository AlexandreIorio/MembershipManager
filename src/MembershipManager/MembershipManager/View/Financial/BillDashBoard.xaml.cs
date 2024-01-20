using MembershipManager.DataModel.Buyable;
using MembershipManager.DataModel.Financial;
using MembershipManager.DataModel.People;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MembershipManager.View.Financial
{
    /// <summary>
    /// Logique d'interaction pour BillDashBoard.xaml
    /// </summary>
    public partial class BillDashBoard : Window, INotifyPropertyChanged
    {
        public List<BillView> Bills { get; private set; }
        private BillView? SelectedBillView { get { return (BillView?)BillsDataGrid.SelectedItem; } }
        private BillView? LastSelection;

        public Member Member { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MemberAccount? Account
        {
            get
            {
                if (SelectedBillView is null) return null;
                return MemberAccount.Select(SelectedBillView.Account_id) as MemberAccount;
            }
        }

        public BillDashBoard(Member? member = null)
        {
            InitializeComponent();
            RefreshTransactions();
        }

        private void RefreshTransactions()
        {

            Bills = Bill.Views()?.Cast<BillView>().ToList();
            BillsDataGrid.ItemsSource = Bills;
        }

        private void SortDatasByMember()
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
            RefreshTransactions();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer cette facture?", "Supprimer", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Paiement? paiement = (Paiement?)Paiement.Select(SelectedBillView?.id);
                Paiement.Delete(paiement.Id);
                Bills.Remove(SelectedBillView);
                BillsDataGrid.ItemsSource = Bills;
                BillsDataGrid.Items.Refresh();
            }
        }
    }
}
