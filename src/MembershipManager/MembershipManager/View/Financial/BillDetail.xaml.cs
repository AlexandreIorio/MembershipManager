using MembershipManager.DataModel.Financial;
using System.Windows;
using System.Windows.Controls;

namespace MembershipManager.View.Financial
{
    /// <summary>
    /// Logique d'interaction pour BillDetail.xaml
    /// </summary>
    public partial class BillDetail : Page
    {

        public Bill Bill { get; set; }
        public double? ComputedAmount { get => Bill.Amount / 100.0; set => Bill.Amount = (int?)(value * 100); }
        public BillDetail(Bill bill)
        {
            InitializeComponent();
            if (bill is null) throw new ArgumentNullException(nameof(bill));

            Bill = bill;
            TextBoxAmount.Text = ComputedAmount.ToString();
            this.DataContext = Bill;
            TextBoxAmount.DataContext = this;
        }

        private void TextBoxAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxAmount.Text == "") return;
            if (double.TryParse(TextBoxAmount.Text, out double amount))
            {
                ComputedAmount = amount;
            }
            else
            {
                MessageBox.Show("La valeur entrée n'est pas un nombre");
                TextBoxAmount.Clear();
                TextBoxAmount.Focus();
            }
        }
    }
}
