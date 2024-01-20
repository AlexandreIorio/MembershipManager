using MembershipManager.DataModel.Financial;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;

namespace MembershipManager.View.Financial
{
    /// <summary>
    /// Logique d'interaction pour PaiementDetailWindows.xaml
    /// </summary>
    public partial class BillDetailWindows : Window
    {

        private bool _IsEditMode = false;
        private PaiementDetail _paiement;
        private BillDetail _bill;

        public BillDetailWindows(MemberAccount account) : this(new Bill() { Account = account, Date = DateTime.Now }) { }

        public BillDetailWindows(Bill? bill)
        {
            InitializeComponent();
            _paiement = new PaiementDetail(bill);
            _bill = new BillDetail(bill);
            _paiement.TextBoxAmount.Focus();
            FramePaiement.Navigate(_paiement);
            FrameBill.Navigate(_bill);
            _IsEditMode = bill.Id is not null;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Paiement paiement = _paiement.Paiement;
            Bill bill = _bill.Bill;
            if (paiement is null) throw new ArgumentNullException(nameof(paiement));

            if (_IsEditMode)
            {
                bill.Update();
            }
            else
            {
                bill.Insert();
            }
            Close();
        }
    }
}
