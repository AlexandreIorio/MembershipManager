using MembershipManager.DataModel;
using MembershipManager.DataModel.Buyable;
using MembershipManager.DataModel.Financial;
using MembershipManager.DataModel.People;
using MembershipManager.Engine;
using MembershipManager.View.Utils;
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
        private List<BillView> _sortedBills;
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
            if (member is not null)
            {
                CheckBoxMember.IsChecked = true;
                Member = member;
                ButtonMember.Content = Member.ToString();
            }
            ComboBoxStatus.ItemsSource = Bill.BillStatusNames;
            SortDatas();
            RefreshTransactions();
        }

        private void RefreshTransactions()
        {

            Bills = Bill.Views()?.Cast<BillView>().ToList();
            BillsDataGrid.ItemsSource = Bills;
        }

        private void SortDatas()
        {
            _sortedBills = Bills;
            if (CheckBoxMember.IsChecked == true)
            {
                _sortedBills = _sortedBills.Where(b => b.Account_id == Member.NoAvs).ToList();
            }
            if (CheckBoxStatus.IsChecked == true)
            {
                string? status = (string?)ComboBoxStatus.SelectedItem;
                if (status is not null) _sortedBills = _sortedBills.Where(b => b.GetStatusName().Equals(status)).ToList();
            }
            if (CheckBoxDate.IsChecked == true)
            {
                DateTime? from = DatePickerDateFrom.SelectedDate;
                DateTime? To = DatePickerDateTo.SelectedDate;

                if (from is null) _sortedBills = _sortedBills.Where(b => b.Date?.Date <= To?.Date).ToList();
                else if (To is null) _sortedBills = _sortedBills.Where(b => b.Date?.Date >= from?.Date).ToList();
                else if (from is null && To is null) return;
                else _sortedBills = _sortedBills.Where(b => b.Date?.Date >= from?.Date && b.Date?.Date <= To?.Date).ToList();
            }
            BillsDataGrid.ItemsSource = _sortedBills;
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

        private void FilterValue_Changed(object sender, SelectionChangedEventArgs e)
        {
            SortDatas();
        }

        private void ButtonMember_Click(object sender, RoutedEventArgs e)
        {
            ListSelection listSelection = new(Member.Views().Cast<MemberView>())
            {
                Width = 400
            };
            listSelection.List.MouseDoubleClick += (sender, e) =>
            {
                listSelection.DialogResult = true;
                listSelection.Close();
            };
            listSelection.ButtonSelect.Click += (sender, e) =>
            {
                listSelection.DialogResult = true;
                listSelection.Close();
            };
            listSelection.ButtonCancel.Click += (sender, e) =>
            {
                listSelection.DialogResult = false;
                listSelection.Close();
            };
            
            listSelection.ShowDialog();
            if (listSelection.DialogResult == true && listSelection.List.SelectedItem is not null)
            {
                Member = Member.Select((listSelection.List.SelectedItem as MemberView)?.no_avs) as Member;
                ButtonMember.Content = Member.ToString();
                SortDatas();
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SortDatas();
        }
    }
}
