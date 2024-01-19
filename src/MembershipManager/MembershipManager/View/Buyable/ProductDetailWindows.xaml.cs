using MembershipManager.DataModel.Buyable;
using MembershipManager.DataModel.Financial;
using MembershipManager.DataModel.People;
using MembershipManager.View.Financial;
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
    /// Logique d'interaction pour ProductDetailWindows.xaml
    /// </summary>
    public partial class ProductDetailWindows : Window
    {
        private bool _IsEditMode = false;
        private ProductDetail _productDetail;
        public ProductDetailWindows(Product product)
        {
            InitializeComponent();
            _productDetail = new ProductDetail(product);

            _IsEditMode = product.Code is not null;
            if (_IsEditMode) _productDetail.TextBoxAmount.Focus();
            else _productDetail.TextBoxCode.Focus();
            MainFrame.Navigate(_productDetail);
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

            Product product = _productDetail.Product;
            if (_IsEditMode)
            {
                product.Update();
            }
            else
            {
                product.Insert();
            }
            Close();
        }

    }
}
