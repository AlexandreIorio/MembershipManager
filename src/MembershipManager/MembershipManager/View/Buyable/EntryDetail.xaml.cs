using MembershipManager.DataModel.Buyable;
using System.Windows;
using System.Windows.Controls;

namespace MembershipManager.View.Buyable
{
    /// <summary>
    /// Interaction logic for EntryDetail.xaml
    /// </summary>
    public partial class EntryDetail : Page
    {
        public Entry Entry { get; set; }

        public EntryDetail(Entry entry)
        {
            this.Entry = entry;
            InitializeComponent();
            TextBoxAmount.Text = Entry.ComputedAmount.ToString();
            TextBoxQuantity.Text = Entry.Quantity.ToString();
            RadioSubscription.IsChecked = Entry.IsSubscription;

        }

        private void RadioButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Entry.IsSubscription = (bool)RadioSubscription.IsChecked;
        }

        private void TextBoxQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(TextBoxQuantity.Text, out int quantity))
            {
                Entry.Quantity = quantity;
            }
            else
            {
                MessageBox.Show("La quantité doit être un nombre entier");
                TextBoxQuantity.Text = "0";
                TextBoxQuantity.Focus();
            }
        }

        private void TextBoxAmount_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (double.TryParse(TextBoxAmount.Text, out double price))
            {
                Entry.ComputedAmount = price;
            }
            else
            {
                MessageBox.Show("Le prix doit être un nombre");
                TextBoxAmount.Text = "0";
                TextBoxAmount.Focus();
            }

        }
    }
}
