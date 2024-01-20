using MembershipManager.DataModel.Buyable;
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
        private int _previousTextLength = 0;
        private ListSelectionPage _listSelection;

        public MainWindow()
        {
            InitializeComponent();

            _listSelection = new(Member.Views().Cast<MemberView>());
            ListFrame.Navigate(_listSelection);

            _listSelection.List.ContextMenu = MemberView.ContextMenu(_listSelection);

            MouseDoubleClick += (sender, e) =>
            {
                string? noAvs = (_listSelection.List.SelectedItem as MemberView)?.no_avs;
                if (noAvs is null) return;
                Member? member = (Member?)Member.Select(noAvs);
                if (member is null) return;
                AccountDetailWindows accountDetailWindow = new AccountDetailWindows(member);
                accountDetailWindow.ShowDialog();
            };

            _listSelection.TextBoxSearch.TextChanged += (sender, e) =>
            {
              int currentTextLenght = _listSelection.TextBoxSearch.Text.Length;

                if (_listSelection.List.Items.Count == 1 && currentTextLenght > _previousTextLength)
                    ValidateEntry.Focus();

                _previousTextLength = currentTextLenght;
            };
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

        private void ButtonProduct_Click(object sender, RoutedEventArgs e)
        {
            ListSelection listSelection = new ListSelection(ProductView.Views().Cast<ProductView>());
            listSelection.Width = 800;
            Button button = new Button() { Content = "Nouveau produit" };
            button.Click += (sender, e) =>
            {
                ProductDetailWindows productDetailWindows = new(new Product());
                productDetailWindows.ShowDialog();
                listSelection.List.ItemsSource = ProductView.Views().Cast<ProductView>();
            };
            listSelection.StackPanelButtons.Children.Insert(1, button);
            listSelection.ShowDialog();
        }

        private void ButtonBill_Click(object sender, RoutedEventArgs e)
        {
            BillDashBoard billDashBoard = new();
            billDashBoard.ShowDialog();
        }

        private void ButtonValidateEntry_Click(object sender, RoutedEventArgs e)
        {
            MemberView member = _listSelection.List.SelectedItem as MemberView;
            if (member is null) return;

            Member? memberSelected = Member.Select(member.no_avs) as Member;
            if (memberSelected is null) return;

            if (memberSelected.Account.TryConsumeEntry())
                MessageBox.Show($"Entrée validée pour {memberSelected.FirstName} {memberSelected.LastName} !", "OK", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
                MessageBox.Show($"Entrée impossible pour {memberSelected.FirstName} {memberSelected.LastName} !", "KO", MessageBoxButton.OK, MessageBoxImage.Error);

            _listSelection.TextBoxSearch.Focus();
        }
    }
}