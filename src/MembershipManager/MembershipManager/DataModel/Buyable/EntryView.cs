using MembershipManager.DataModel.People;
using MembershipManager.Engine;
using MembershipManager.View.Buyable;
using MembershipManager.View.People.Member;
using MembershipManager.View.Utils.ListSelectionForm;
using Npgsql;
using System.CodeDom;

namespace MembershipManager.DataModel.Buyable
{
    public class EntryView : SqlViewable, Ilistable
    {
        public int? Id { get; set; }

        [Displayed("Quantité")]
        public int Quantity { get; set; }

        public bool is_subscription { get; set; }

        [IgnoreSql]
        [Filtered("Abonnement")]
        [Displayed("Type")]
        public string? IsSubscriptionName => is_subscription == true ? "Abonnement" : "Entrée";

        public int Amount { get; set; }

        [IgnoreSql]
        [TextFormat("{0:N2}")]
        [Filtered("Prix")]
        [Displayed("Prix")]
        public double ComputedAmount { get => Amount / 100.0; }

        [IgnoreSql]
        public string ComputedName { get => $"{Quantity} {_unit}"; }

        [IgnoreSql]
        private string _unit { get => is_subscription == true ? "mois" : "entrées"; }

        public static List<SqlViewable>? Views(params NpgsqlParameter[] sqlParam)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT * FROM entry;";

            return DbManager.Db.Views<EntryView>(cmd).Cast<SqlViewable>().ToList();
        }

        public static void EditEntry(int? id)
        {
            ArgumentNullException.ThrowIfNull(id);
            Entry? entry = (Entry?)Entry.Select(id);
            if (entry is null) return;
            EntryDetailWindow entryDetailWindow = new(entry);
            entryDetailWindow.ShowDialog();
        }

        public static void NewEntry()
        {
            EntryDetailWindow entryDetailWindow = new(new Entry());
            entryDetailWindow.ShowDialog();
        }

        public Product ToProduct()
        {
            return new Product()
            {
                Code = IsSubscriptionName,
                Name = ComputedName,
                Amount = Amount
            };
        }
    }
}
