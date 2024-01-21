using MembershipManager.Engine;
using System.Text;
using System.Windows;

namespace MembershipManager.DataModel.Buyable
{
    [DbTableName("entry")]
    public class Entry : ISql
    {
        [DbPrimaryKey(NpgsqlTypes.NpgsqlDbType.Integer)]
        [DbAttribute("id")]
        public int? Id { get; set; }

        [DbAttribute("quantity")]
        public int? Quantity { get; set; }

        [DbAttribute("amount")]
        public int? Amount { get; set; }
        public double ComputedAmount { get => (Amount ?? 0) / 100.0; set => Amount = (int?)(value * 100); }

        [DbAttribute("is_subscription")]
        public bool IsSubscription { get; set; }

        public Entry() : base()
        {
        }

        public Entry(Entry entry)
        {
            IsSubscription = entry.IsSubscription;
            Quantity = entry.Quantity;
            Amount = entry.Amount;
        }

        public new void Insert()
        {
            if (Validate()) DbManager.Db?.Send(ISql.InsertQuery<Entry>(this));
        }

        public void Update()
        {
            if (Validate()) DbManager.Db?.Send(ISql.UpdateQuery<Entry>(this));
        }

        public static void Delete(params object[] pk)
        {
            ISql.Erase<Entry>(pk);
        }

        public static ISql? Select(params object[] pk)
        {
            if (pk.Length != 1) throw new ArgumentException();
            Entry? p = ISql.Get<Entry>(pk[0]);
            if (p == null) throw new KeyNotFoundException();

            return p;
        }

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
