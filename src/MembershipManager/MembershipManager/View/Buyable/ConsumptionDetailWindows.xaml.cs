using MembershipManager.DataModel.Buyable;
using MembershipManager.View.Utils;
using System.Windows;
using System.Windows.Controls;

namespace MembershipManager.View.Buyable
{
    /// <summary>
    /// Logique d'interaction pour ConsumableDetailWindows.xaml
    /// </summary>
    public partial class ConsumptionDetailWindows : Window
    {
        private bool _IsEditMode = false;
        private ProductDetail _productDetail;
        private ListSelectionPage _listSelection;
        public Consumption Consumption { get; set; }

        public ConsumptionDetailWindows(Consumption consumption)
        {
            InitializeComponent();
            _listSelection = new ListSelectionPage(ProductView.Views().Cast<ProductView>());
            _listSelection.List.MouseDoubleClick += (sender, e) =>
            {
                if (_listSelection.List.SelectedItem is null) return;
                ButtonSave_Click(sender, e);
            };

            FrameList.Navigate(_listSelection);

            Consumption = consumption;

            _productDetail = new ProductDetail(Consumption);

            _IsEditMode = consumption.Code is not null;
            if (_IsEditMode) _productDetail.TextBoxAmount.Focus();
            else _productDetail.TextBoxCode.Focus();
            FrameDetail.Navigate(_productDetail);

            _listSelection.List.SelectionChanged += List_SelectionChanged;
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductView? productView = (ProductView?)_listSelection.List.SelectedItem;
            if (productView is null) return;

            Product? product = Product.Select(productView.id) as Product;
            if (product is null) return;

            _productDetail.UpdateProduct(product);
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Consumption.SetProduct(_productDetail.Product);


            if (_IsEditMode)
            {
                Consumption.Update();
            }
            else
            {
                Consumption.Insert();
            }
            Close();

        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            ProductDetailWindows productDetailWindows = new ProductDetailWindows(new Product());
            productDetailWindows.ShowDialog();
            _listSelection.UpdateList(ProductView.Views().Cast<ProductView>());
        }
    }
}
