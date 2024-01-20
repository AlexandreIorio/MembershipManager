using MembershipManager.DataModel.Financial;
using MembershipManager.DataModel.People;
using MembershipManager.View.Utils;
using System.Windows;
using System.Windows.Controls;

namespace MembershipManager.View.Financial
{
    /// <summary>
    /// Logique d'interaction pour PaiementDetail.xaml
    /// </summary>
    public partial class PaiementDetail : Page
    {
        public Paiement Paiement { get; set; }
        public Member? Member { get; set; }
        public double? ComputedAmount { get => Paiement.Amount / 100.0; set => Paiement.Amount = (int?)(value * 100); }
        public PaiementDetail(Paiement paiement)
        {
            InitializeComponent();
            if (paiement is null) throw new ArgumentNullException(nameof(paiement));

            if (paiement.Account?.NoAvs is not null)
            {
                Member = Member.Select(paiement.Account?.NoAvs ?? "") as Member;
                ButtonMember.IsEnabled = false;
                ButtonMember.Content = Member.ToString();
            }

            Paiement = paiement;
            TextBoxAmount.Text = ComputedAmount.ToString();
            this.DataContext = Paiement;
            TextBoxAmount.DataContext = this;
        }

        private void ButtonMember_Click(object sender, RoutedEventArgs e)
        {
            List<MemberView>? members = Member.Views()?.Cast<MemberView>().ToList();
            if (members is null)
            {
                MessageBox.Show("Aucun membre trouvé");
                return;
            }

            ListSelection listSelection = new ListSelection(members);

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
            if (listSelection.DialogResult == true)
            {
                Member? member = (Member)listSelection.List.SelectedItem;
                if (member is null) return;
                Paiement.Account = (MemberAccount?)MemberAccount.Select(member.NoAvs ?? "");
                ButtonMember.Content = member.ToString();
            }
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
