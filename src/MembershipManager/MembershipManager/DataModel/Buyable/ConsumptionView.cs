using MembershipManager.DataModel.Financial;
using MembershipManager.Engine;
using MembershipManager.View.Utils.ListSelectionForm;

namespace MembershipManager.DataModel.Buyable
{
    public class ConsumptionView : SqlViewable, ITransaction
    {

        public int? Id { get; set; }
        [Sorted]
        [Filtered("Code")]
        [Displayed("Code")]
        public string? Code { get; set; }

        [Filtered("Nom")]
        [Displayed("Nom")]
        public string? Name { get; set; }

        [Filtered("Prix")]
        [Displayed("Prix")]
        public int? Amount { get; set; }

        [Filtered("Prix")]
        [Displayed("Prix")]
        public DateTime? Date { get; set; }

        [IgnoreSql]
        public string? Description => Name;


        public string? Account_id { get; set; }
        public int? Bill_id { get; set; }

        [IgnoreSql]
        public double ComputedAmount => Math.Round(-(Amount ?? 0) / 100.0, 2);
    }
}
