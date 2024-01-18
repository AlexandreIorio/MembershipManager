using MembershipManager.DataModel.Buyable;
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

namespace MembershipManager.View.Buyable
{
    /// <summary>
    /// Logique d'interaction pour ProductDetail.xaml
    /// </summary>
    public partial class ProductDetail : Page
    {
        public Product Product { get; set; }
        public double? ComputedAmount { get => Product.Amount / 100.0; set => Product.Amount = (int?)(value * 100); }
        public ProductDetail(Product product)
        {
            Product = product;
            InitializeComponent();
            this.DataContext = Product;
        }

        private void ButtonMember_Click(object sender, RoutedEventArgs e)
        {
            List<ProductView>? products = ProductView.Views()?.Cast<ProductView>().ToList();
            if (products is null)
            {
                MessageBox.Show("Aucun membre trouvé");
                return;
            }

            ListSelection listSelection = new ListSelection(products);

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
