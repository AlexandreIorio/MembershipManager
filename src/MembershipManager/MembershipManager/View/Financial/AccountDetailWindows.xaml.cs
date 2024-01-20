using MembershipManager.DataModel.Buyable;
using MembershipManager.DataModel.Financial;
using MembershipManager.DataModel.People;
using MembershipManager.View.Buyable;
using Npgsql;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace MembershipManager.View.Financial
{
    /// <summary>
    /// Logique d'interaction pour AccountDetailWindows.xaml
    /// </summary>
    public partial class AccountDetailWindows : Window
    {
        public List<ITransaction>? Transactions { get; set; }
        public Member Member { get; private set; }
        public MemberAccount Account { get; private set; }

        public string MemberName
        {
            get
            {
                return Member.FirstName + " " + Member.LastName;
            }
        }

        public double Balance => Account?.Balance ?? 0;
        public double PendingAmount => Account?.PendingAmount ?? 0;

        public AccountDetailWindows(Member member)
        {
            InitializeComponent();
            Member = member ?? throw new NullReferenceException("Member can't be null ");
            Account = MemberAccount.Select(Member.NoAvs) as MemberAccount ?? throw new NullReferenceException("Member account can't be null");
            EntryTypeComboBox.ItemsSource = EntryView.Views().Cast<EntryView>();
            EntryTypeComboBox.DisplayMemberPath = "ComputedName";
            EntryTypeComboBox.SelectedValuePath = "Id";
            RefreshTransactions();
            DataContext = this;
        }

        private void RefreshTransactions()
        {
            NpgsqlParameter param = new("@id", Account?.NoAvs);
            NpgsqlParameter param2 = new("@payed", true)
            {
                DbType = DbType.Boolean
            };
            Transactions = ITransaction.Views(param, param2)?.Cast<ITransaction>().ToList();
            TransactionsDataGrid.ItemsSource = Transactions;
            LabelBalance.Content = Balance;
            LabelPending.Content = PendingAmount;
            LabelSubscribe.Content = Account.SubscriptionIssue?.ToShortDateString();
            LabelEntry.Content = Account.AvailableEntry;
        }


        private void TransactionsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TransactionsDataGrid.SelectedItem is null) return;

            if (TransactionsDataGrid.SelectedItem is PaiementView paiementView)
            {
                if (paiementView is null) return;
                Paiement? paiement = Paiement.Select(paiementView.id ?? throw new NullReferenceException("The id of paiement can't be null")) as Paiement;
                if (paiement is null) return;
                PaiementDetailWindows paiementDetailWindows = new(paiement);
                paiementDetailWindows.ShowDialog();
                RefreshTransactions();
            }
            else if (TransactionsDataGrid.SelectedItem is ConsumptionView consumptionView)
            {
                if (consumptionView is null) return;
                Consumption? consumption = Consumption.Select(consumptionView.Id ?? throw new NullReferenceException("The id of consumption can't be null")) as Consumption;
                if (consumption is null) return;
                ConsumptionDetailWindows consumptionDetailWindows = new(consumption);
                consumptionDetailWindows.ShowDialog();
                RefreshTransactions();
            }

        }

        private void ButtonAddPaiement_Click(object sender, RoutedEventArgs e)
        {
            PaiementDetailWindows paiementDetailWindows = new(Account);
            paiementDetailWindows.ShowDialog();
            RefreshTransactions();
        }

        private void ButtonAddConsuption_Click(object sender, RoutedEventArgs e)
        {
            Consumption consumption = new(new Product(), Account);
            ConsumptionDetailWindows consumptionDetailWindows = new(consumption);
            consumptionDetailWindows.ShowDialog();
            RefreshTransactions();
        }

        private void TransactionsDataGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (TransactionsDataGrid.SelectedItems.Count == 0) return;

                foreach (ITransaction transaction in TransactionsDataGrid.SelectedItems)
                {
                    if (transaction is PaiementView paiementView)
                    {
                        if (paiementView is null) return;
                        Paiement? paiement = Paiement.Select(paiementView.id ?? throw new NullReferenceException("The id of paiement can't be null")) as Paiement;
                        if (paiement is null) return;
                        Paiement.Delete(paiement.Id);
                    }
                    else if (transaction is ConsumptionView consumptionView)
                    {
                        if (consumptionView is null) return;
                        Consumption? consumption = Consumption.Select(consumptionView.Id ?? throw new NullReferenceException("The id of consumption can't be null")) as Consumption;
                        if (consumption is null) return;
                        Consumption.Delete(consumption.Id);
                    }
                }
                RefreshTransactions();
            }
        }

        private void ButtonGenerateBill_Click(object sender, RoutedEventArgs e)
        {
            Bill bill = new()
            {
                Account = Account
            };
            bill.Generate();
            RefreshTransactions();
        }

        private void ButtonBuyEntry_Click(object sender, RoutedEventArgs e)
        {
            if (EntryTypeComboBox.SelectedItem is null)
            {
                MessageBox.Show("Veuillez sélectionner un type d'entrée", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            EntryView entry = (EntryView)EntryTypeComboBox.SelectedItem;

            MessageBoxResult result = ValidateEntry(entry);

            if (result == MessageBoxResult.Yes)
            {
                Account.AddEntry(entry);
                Consumption consumption = new(entry.ToProduct(), Account);
                consumption.Insert();
                RefreshTransactions();
            }
        }

        private MessageBoxResult ValidateEntry(EntryView entry)
        {
            MessageBoxResult result = MessageBox.Show($"Voulez-vous acheter {entry.ComputedName} ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result;
        }
    }
}
