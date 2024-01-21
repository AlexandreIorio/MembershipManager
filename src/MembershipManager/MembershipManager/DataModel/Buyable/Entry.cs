using MembershipManager.Engine;
using System.Text;
using System.Windows;

namespace MembershipManager.DataModel.Buyable
{
    /// <summary>
    /// Class which represent an entry as a one time entry or a subscription.
    /// </summary>
    [DbTableName("entry")]
    public class Entry : ISql
    {
        /// <summary>
        /// The id of the entry.
        /// </summary>
        [DbPrimaryKey(NpgsqlTypes.NpgsqlDbType.Integer)]
        [DbAttribute("id")]
        public int? Id { get; set; }

        /// <summary>
        /// The quantity of the entry, can be month for subscription, or number of entries for one time entry.
        /// </summary>
        [DbAttribute("quantity")]
        public int? Quantity { get; set; }

        /// <summary>
        /// The amount of the entry, in cents.
        /// </summary>
        [DbAttribute("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// The amount of the entry, in francs.
        /// </summary>
        public double ComputedAmount { get => (Amount ?? 0) / 100.0; set => Amount = (int?)(value * 100); }

        /// <summary>
        /// Discriminator to know if the entry is a subscription or a one time entry.
        /// </summary>
        [DbAttribute("is_subscription")]
        public bool IsSubscription { get; set; }

        /// <summary>
        /// Simple Entry constructor.
        /// </summary>
        public Entry()
        {
        }

        /// <summary>
        /// Entry copy constructor
        /// </summary>
        /// <param name="entry"></param>
        public Entry(Entry entry)
        {
            IsSubscription = entry.IsSubscription;
            Quantity = entry.Quantity;
            Amount = entry.Amount;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Insert()
        {
            if (Validate()) DbManager.Db?.Send(ISql.InsertQuery<Entry>(this));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Update()
        {
            if (Validate()) DbManager.Db?.Send(ISql.UpdateQuery<Entry>(this));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public static void Delete(params object[] pk)
        {
            ISql.Erase<Entry>(pk);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public static ISql? Select(params object[] pk)
        {
            if (pk.Length != 1) throw new ArgumentException();
            Entry? p = ISql.Get<Entry>(pk[0]);
            if (p == null) throw new KeyNotFoundException();

            return p;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Validate()
        {
            StringBuilder message = new();
            bool valid = ControlParamValidity(message);

            ShowErrorMessage(message, valid);
            return valid;
        }

        private bool ControlParamValidity(StringBuilder message)
        {
            bool valid = true;

            if (Quantity == 0)
            {
                message.AppendLine("La quantité ne peut être inférieur ou égale à 0");
                valid = false;
            }

            if (Amount == 0)
            {
                message.AppendLine("Le prix ne peut être inférieur ou égale à 0");
                valid = false;
            }

            return valid;
        }

        private static void ShowErrorMessage(StringBuilder message, bool valid)
        {
            if (!valid)
            {
                MessageBox.Show(message.ToString(),
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

            }
        }
    }
}
