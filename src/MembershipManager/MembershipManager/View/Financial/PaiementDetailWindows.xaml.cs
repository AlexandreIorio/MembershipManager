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
        public PaiementDetailWindows(Paiement? p = null)
        {
            InitializeComponent();
            _paiement = new PaiementDetail(p);
            MainFrame.Navigate(_paiement);
            _IsEditMode = p is not null;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

            Paiement paiement = _paiement.Paiement;
            if (_IsEditMode)
            {
                paiement.Update();
            }
            else
            {
                paiement.Insert();
            }
            Close();
        }
    }
}
