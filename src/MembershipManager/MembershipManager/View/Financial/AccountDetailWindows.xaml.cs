using MembershipManager.DataModel.Buyable;
using MembershipManager.DataModel.Financial;
using MembershipManager.DataModel.People;
using MembershipManager.View.Buyable;
using Npgsql;
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

        public AccountDetailWindows(Member member)
        {
            InitializeComponent();
            Member = member ?? throw new NullReferenceException("Member can't be null ");
            Account = MemberAccount.Select(Member.NoAvs) as MemberAccount ?? throw new NullReferenceException("Member account can't be null");
            RefreshTransactions();
            DataContext = this;
        }

        private void RefreshTransactions()
        {
            NpgsqlParameter param = new NpgsqlParameter("@id", Account?.NoAvs);
            Transactions = ITransaction.Views(param)?.Cast<ITransaction>().ToList();
            TransactionsDataGrid.ItemsSource = Transactions;
            LabelBalance.Content = Balance;
        }


        private void TransactionsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TransactionsDataGrid.SelectedItem is null) return;

            if (TransactionsDataGrid.SelectedItem is PaiementView paiementView)
            {
                if (paiementView is null) return;
                Paiement? paiement = Paiement.Select(paiementView.id ?? throw new NullReferenceException("The id of paiement can't be null")) as Paiement;
                if (paiement is null) return;
                PaiementDetailWindows paiementDetailWindows = new PaiementDetailWindows(paiement);
                paiementDetailWindows.ShowDialog();
                RefreshTransactions();
            }
            else if (TransactionsDataGrid.SelectedItem is ConsumptionView consumptionView)
            {
                if (consumptionView is null) return;
                Consumption? consumption = Consumption.Select(consumptionView.Id ?? throw new NullReferenceException("The id of consumption can't be null")) as Consumption;
                if (consumption is null) return;
                ConsumptionDetailWindows consumptionDetailWindows = new ConsumptionDetailWindows(consumption);
                consumptionDetailWindows.ShowDialog();
                RefreshTransactions();
            }

        }

        private void ButtonAddPaiement_Click(object sender, RoutedEventArgs e)
        {
            PaiementDetailWindows paiementDetailWindows = new PaiementDetailWindows(Account);
            paiementDetailWindows.ShowDialog();
            RefreshTransactions();
        }

        private void ButtonAddConsuption_Click(object sender, RoutedEventArgs e)
        {
            Consumption consumption = new Consumption(new Product(), Account);
            ConsumptionDetailWindows consumptionDetailWindows = new ConsumptionDetailWindows(consumption);
            consumptionDetailWindows.ShowDialog();
            RefreshTransactions();
        }
    }
}
