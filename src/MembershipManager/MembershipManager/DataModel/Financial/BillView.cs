using MembershipManager.DataModel.Buyable;
using MembershipManager.Engine;
using MembershipManager.View.Utils.ListSelectionForm;
using static MembershipManager.DataModel.Financial.Bill;

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
        public string[] StatusList { get => Bill.BillStatusNames; }


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
        public double? ComputedPayedAmount => Math.Round((payed_amount ?? 0) / 100.0, 2);

        [IgnoreSql]
        public string Status
        {
            get
            {
                if ((bool)Payed) return StatusList[0];
                else if (((DateTime)issue_date).Date < DateTime.Now.Date) return StatusList[2];
                else return StatusList[1];
            }
        }

        [IgnoreSql]
        public static List<ConsumptionView>? Consumptions { get; set; } = Consumption.Views()?.Cast<ConsumptionView>().ToList();

        [IgnoreSql]
        public List<ConsumptionView>? ConsumptionsDetail
        {
            get
            {
                ;
                return Consumptions?.Where(c => c.Bill_id == id).ToList();
            }
        }

        [IgnoreSql]
        public new string? Description => $"Facture du: {Date?.ToString("dd.MM.yyyy")}";

        public BillStatus GetStatus()
        {
            if (Payed == true) return BillStatus.Payed;
            if (issue_date < DateTime.Now) return BillStatus.Expired;
            return BillStatus.Pending;
        }

        public string GetStatusName()
        {
            return BillStatusNames[(int)GetStatus()];
        }
    }
}
