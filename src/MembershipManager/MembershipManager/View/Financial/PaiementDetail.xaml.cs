using MembershipManager.DataModel;
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
    /// Logique d'interaction pour PaiementDetail.xaml
    /// </summary>
    public partial class PaiementDetail : Page
    {
        public Paiement Paiement;
        public Member? Member;
        public double? ComputedAmount { get => Paiement.Amount / 100.0; set => Paiement.Amount = (int?)(value * 100); }
        public PaiementDetail(Paiement paiement)
        {
            InitializeComponent();
            if (paiement is null) throw new ArgumentNullException(nameof(paiement));
            
            if (paiement.Account?.NoAvs is not null)
            {
                Member = Member.Select(paiement.Account?.NoAvs ?? "") as Member;
                ButtonMember.Content = Member?.ToString();
                ButtonMember.IsEnabled = false;
            }
            Paiement = paiement;
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
    }
}
