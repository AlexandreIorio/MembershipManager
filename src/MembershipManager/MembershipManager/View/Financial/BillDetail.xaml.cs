using MembershipManager.DataModel.Financial;
using MembershipManager.DataModel.People;
using MembershipManager.View.Utils;
using System;
using System.Collections.Generic;
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
