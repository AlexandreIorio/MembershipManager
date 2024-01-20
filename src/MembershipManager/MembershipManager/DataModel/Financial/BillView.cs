using MembershipManager.View.Utils.ListSelectionForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipManager.DataModel.Financial
{
    public class BillView : PaiementView
    {
        [Displayed("Date d'échéance")]
        public DateTime? issue_date { get; set; }

        [Displayed("Date de paiement")]
        public DateTime? payed_date { get; set; }

        [Displayed("Montant payé")]
        public int? payed_amount { get; set; }
    }
}
