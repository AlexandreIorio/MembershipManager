using MembershipManager.DataModel.Buyable;
using MembershipManager.Engine;
using MembershipManager.View.Utils.ListSelectionForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace MembershipManager.DataModel.Financial
{
    public class BillView : PaiementView
    {
        const string NUMBER_FORMAT = "#####";

        [Displayed("Date d'échéance")]
        public DateTime? issue_date { get; set; }

        [Displayed("Date de paiement")]
        public DateTime? payed_date { get; set; }

        [Displayed("Montant payé")]
        public int? payed_amount { get; set; }

        [IgnoreSql]
        public string Number
        {
            get
            {
                if (Date is null) return id.ToString();
                return $"{((DateTime)Date).Year}{id}";
            }
        }
        [IgnoreSql]
        public double ? ComputedPayedAmount => Math.Round(((payed_amount ?? 0) / 100.0), 2);

        [IgnoreSql]
        public string Status
        {
            get
            {
                if ((bool)Payed) return "Payé";
                else if (((DateTime)issue_date).Date < DateTime.Now.Date) return "Expirée";
                else return "En attente";
            }
        }

        [IgnoreSql]
        public static List<ConsumptionView>? Consumptions { get; set; } = Consumption.Views()?.Cast<ConsumptionView>().ToList();
        
        [IgnoreSql]
        public List<ConsumptionView>? ConsumptionsDetail
        {
            get
            {;
                return Consumptions?.Where(c => c.Bill_id == id).ToList();
            }
        }

        [IgnoreSql]
        public new string? Description => $"Facture du: {Date?.ToString("dd.MM.yyyy")}";
    }
}
