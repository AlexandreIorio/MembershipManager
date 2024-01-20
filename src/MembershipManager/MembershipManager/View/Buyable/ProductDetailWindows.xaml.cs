using MembershipManager.DataModel.Buyable;
using System.Windows;

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
