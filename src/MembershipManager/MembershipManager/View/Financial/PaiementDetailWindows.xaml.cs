using MembershipManager.DataModel.Financial;
using System.Windows;

namespace MembershipManager.View.Financial
{
    /// <summary>
    /// Logique d'interaction pour PaiementDetailWindows.xaml
    /// </summary>
    public partial class PaiementDetailWindows : Window
    {

        private bool _IsEditMode = false;
        private PaiementDetail _paiement;

        public PaiementDetailWindows(MemberAccount account) : this(new Paiement() { Account = account, Date = DateTime.Now }) { }

        public PaiementDetailWindows(Paiement? p)
        {
            InitializeComponent();
            _paiement = new PaiementDetail(p);
            _paiement.TextBoxAmount.Focus();
            MainFrame.Navigate(_paiement);
            _IsEditMode = p.Id is not null;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Paiement paiement = _paiement.Paiement;
            if (paiement is null) throw new ArgumentNullException(nameof(paiement));

            if (_IsEditMode)
            {
                paiement.Update();
            }
            else
            {
                paiement.Payed = true;
                paiement.Insert();
            }
            Close();
        }
    }
}
