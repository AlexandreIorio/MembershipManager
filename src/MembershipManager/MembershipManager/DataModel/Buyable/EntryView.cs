using MembershipManager.Engine;
using MembershipManager.View.Buyable;
using MembershipManager.View.Utils.ListSelectionForm;
using Npgsql;

namespace MembershipManager.DataModel.Buyable
{
    /// <summary>
    /// <inheritdoc/>
    /// Specialized for <see cref="Entry"/> 
    /// </summary>
    public class EntryView : SqlViewable, Ilistable
    {
        /// <summary>
        /// Id of the entry
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Quantity of the entry
        /// </summary>
        [Displayed("Quantité")]
        public int Quantity { get; set; }

        /// <summary>
        /// Discriminator to know if the entry is a subscription or a one time entry.
        /// </summary>
        public bool is_subscription { get; set; }

        /// <summary>
        /// String representation of the discriminator.
        /// </summary>
        [IgnoreSql]
        [Filtered("Abonnement")]
        [Displayed("Type")]
        public string? IsSubscriptionName => is_subscription == true ? "Abonnement" : "Entrée";

        /// <summary>
        /// The amount of the entry, in cents.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// The computed amount of the entry, in francs.
        /// </summary>
        [IgnoreSql]
        [TextFormat("{0:N2}")]
        [Filtered("Prix")]
        [Displayed("Prix")]
        public double ComputedAmount { get => Amount / 100.0; }

        /// <summary>
        /// The computed name of the entry.
        /// </summary>
        [IgnoreSql]
        public string ComputedName { get => $"{Quantity} {_unit} {ComputedAmount} CHF"; }

        [IgnoreSql]
        private string _unit { get => is_subscription == true ? "mois" : "entrées"; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="sqlParam"></param>
        /// <returns></returns>
        public static List<SqlViewable>? Views(params NpgsqlParameter[] sqlParam)
        {
            NpgsqlCommand cmd = new()
            {
                CommandText = @"SELECT * FROM entry;"
            };

            return DbManager.Db.Views<EntryView>(cmd).Cast<SqlViewable>().ToList();
        }

        /// <summary>
        /// Method to edit an existant entry
        /// </summary>
        /// <param name="id"></param>
        public static void EditEntry(int? id)
        {
            ArgumentNullException.ThrowIfNull(id);
            Entry? entry = (Entry?)Entry.Select(id);
            if (entry is null) return;
            EntryDetailWindow entryDetailWindow = new(entry);
            entryDetailWindow.ShowDialog();
        }

        /// <summary>
        /// Method to create a new entry and insert it into the database
        /// <summary/>
        public static void NewEntry()
        {
            EntryDetailWindow entryDetailWindow = new(new Entry());
            entryDetailWindow.ShowDialog();
        }

        /// <summary>
        /// Method to cas an entry into a product
        /// <summary/>
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
