using System.Windows;
using MembershipManager.DataModel;
using MembershipManager.DataModel.People;
using MembershipManager.DataModel.Company;
using MembershipManager.Engine;
using Npgsql;
using MembershipManager.View;
using MembershipManager.View.Utils.ListSelectionForm;
using MembershipManager.View.Utils;
using MembershipManager.View.People.Person;
using System.Windows.Controls;
using MembershipManager.View.People.Member;
using MembershipManager.DataModel.Financial;

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
            var PersonView = Person.Views().Cast<PersonView>().ToList();

            int i = 0;


        }
  

        private void ButtonMembership_Click(object sender, RoutedEventArgs e)
        {
            ListSelection listSelection = new ListSelection(Member.Views().Cast<MemberView>());
            listSelection.Width = 800;
            listSelection.List.ContextMenu = MemberView.ContextMenu();
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

        private void ButtonTest_Click(object sender, RoutedEventArgs e)
        {
            ListSelection listSelection = new ListSelection(Paiement.Views().Cast<PaiementView>());
            listSelection.Width = 800;
            listSelection.ShowDialog();
        }
    }
}