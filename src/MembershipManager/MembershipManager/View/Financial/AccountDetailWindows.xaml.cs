using MembershipManager.DataModel.Buyable;
using MembershipManager.DataModel.Financial;
using MembershipManager.DataModel.People;
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
        public MemberAccount? Account { get; private set; }

        public string MemberName
        {
            get
            {
                return Member.FirstName + " " + Member.LastName;
            }
        }

        public double Balance
        {
            get
            {
                double balance = 0;
                if (Transactions != null)
                {
                    foreach (ITransaction t in Transactions)
                    {
                        balance += t.ComputedAmount;
                    }
                }
                return balance;
            }
        }

        public AccountDetailWindows(Member member)
        {
            InitializeComponent();
            Member = member;
            Account = (MemberAccount?)MemberAccount.Select(member.NoAvs);
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
                Paiement? paiement = Paiement.Select(paiementView.id) as Paiement;
                if (paiement is null) return;
                PaiementDetailWindows paiementDetailWindows = new PaiementDetailWindows(paiement);
                paiementDetailWindows.ShowDialog();
                RefreshTransactions();
            }
            else if (TransactionsDataGrid.SelectedItem is ConsumptionView consumption)
            {
                MessageBox.Show("Consumption selected");
            }

        }

        private void ButtonAddPaiement_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAddConsuption_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
