using MembershipManager.DataModel.Buyable;
using MembershipManager.View.Utils.ListSelectionForm;
using Npgsql;

namespace MembershipManager.DataModel.Financial
{
    public interface ITransaction
    {
        public string? Account_id { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; }
        public int? Amount { get; set; }

        public double ComputedAmount { get; }

        public static List<SqlViewable>? Views(params NpgsqlParameter[] sqlParam)
        {
            List<SqlViewable> list = new List<SqlViewable>();
            List<SqlViewable>? paiements = Paiement.Views(sqlParam);
            if (paiements != null) list.AddRange(paiements);
            List<SqlViewable>? consumption = Consumption.Views(sqlParam[0]);
            if (consumption != null) list.AddRange(consumption);

            return list.Cast<SqlViewable>().ToList();
        }
    }
}
