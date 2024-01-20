using MembershipManager.DataModel.Buyable;
using MembershipManager.DataModel.Financial;
using MembershipManager.DataModel.People;
using MembershipManager.View.Buyable;
using MembershipManager.View.Financial;
using MembershipManager.View.Utils;
using System.Windows;
using System.Windows.Controls;

namespace MembershipManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonMembership_Click(object sender, RoutedEventArgs e)
        {
            ListSelection listSelection = new(Member.Views().Cast<MemberView>())
            {
                Width = 800
            };
            listSelection.List.ContextMenu = MemberView.ContextMenu(listSelection);
            listSelection.MouseDoubleClick += (sender, e) =>
            {
                string? noAvs = (listSelection.List.SelectedItem as MemberView)?.no_avs;
                if (noAvs is null) return;
                MemberView.EditMember(noAvs);
                listSelection.List.ItemsSource = Member.Views().Cast<MemberView>();
            };

            //Define new button
            Button button = listSelection.ButtonSelect;
            button.Content = "Nouveau Membre";
            button.Click += (sender, e) =>
            {
                MemberView.NewMember();
                listSelection.List.ItemsSource = Member.Views().Cast<MemberView>();
            };

            listSelection.ShowDialog();

        }

        private void ButtonPlan_Click(object sender, RoutedEventArgs e)
        {
            ListSelection listSelection = new ListSelection(EntryView.Views().Cast<EntryView>());

            listSelection.List.MouseDoubleClick += (sender, e) =>
            {
                EntryView selectItem = (EntryView)listSelection.List.SelectedItem;
                if (selectItem is null) return;
                EntryView.EditEntry(selectItem.Id);
                listSelection.UpdateList(EntryView.Views().Cast<EntryView>());
            };

            Button button = new Button() { Content = "Nouveau plan" };
            button.Click += (sender, e) =>
            {
                EntryDetailWindow entryDetailWindow = new(new Entry());
                entryDetailWindow.ShowDialog();
                listSelection.List.ItemsSource = EntryView.Views().Cast<EntryView>();
            };
            listSelection.StackPanelButtons.Children.Insert(1, button);

            listSelection.Width = 800;
            listSelection.ShowDialog();
        }

        private void ButtonTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonProduct_Click(object sender, RoutedEventArgs e)
        {
            ListSelection listSelection = new ListSelection(ProductView.Views().Cast<ProductView>());
            listSelection.Width = 800;
            Button button = new Button() { Content="Nouveau produit"};
            button.Click += (sender, e) =>
            {
                ProductDetailWindows productDetailWindows = new(new Product());
                productDetailWindows.ShowDialog();
                listSelection.List.ItemsSource = ProductView.Views().Cast<ProductView>();
            };
            listSelection.StackPanelButtons.Children.Insert(1, button) ;
            listSelection.ShowDialog();
        }

        private void ButtonBill_Click(object sender, RoutedEventArgs e)
        {
            BillDashBoard billDashBoard = new();
            billDashBoard.ShowDialog();
        }
    }
}