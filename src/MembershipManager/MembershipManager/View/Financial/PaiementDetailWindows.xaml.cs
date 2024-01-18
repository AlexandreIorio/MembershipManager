using MembershipManager.View.People.Person;
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
using System.Windows.Shapes;
using MembershipManager.DataModel.Financial;

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
