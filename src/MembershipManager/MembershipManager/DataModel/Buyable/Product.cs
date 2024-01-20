using MembershipManager.Engine;
using System.Text;
using System.Windows;

namespace MembershipManager.DataModel.Buyable
{
    [DbTableName("product")]
    public class Product : ISql
    {
        [DbPrimaryKey(NpgsqlTypes.NpgsqlDbType.Integer)]
        [DbAttribute("id")]
        public int? Id { get; set; }

        [DbAttribute("code")]
        public string? Code { get; set; }

        [DbAttribute("name")]
        public string? Name { get; set; }

        [DbAttribute("amount")]
        public int? Amount { get; set; }
        public double ComputedAmount { get => Amount ?? 0 / 100.0; set => Amount = (int?)(value * 100); }

        public Product()
        {

        }
        public Product(Product product)
        {
            Code = product.Code;
            Name = product.Name;
            Amount = product.Amount;
        }

        public static ISql? Select(params object[] pk)
        {

            if (pk.Length != 1) throw new ArgumentException();
            Product? p = ISql.Get<Product>(pk[0]);
            if (p == null) throw new KeyNotFoundException();

            return p;

        }

        public bool Validate()
        {
            StringBuilder message = new StringBuilder();
            bool valid = true;
            if (string.IsNullOrEmpty(Code))
            {
                message.AppendLine("Le code produit est obligatoire");
                valid = false;
            }

            if (string.IsNullOrEmpty(Name))
            {
                message.AppendLine("Le nom du produit est obligatoire");
                valid = false;
            }

            if (Amount is null)
            {
                message.AppendLine("Le prix du produit est obligatoire");
                valid = false;
            }


            if (!valid)
            {
                MessageBox.Show(message.ToString(),
                    "Erreur",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);

            }
            return valid;
        }

        public void Update()
        {
            if (Validate()) DbManager.Db?.Send(ISql.UpdateQuery<Product>(this));
        }

        public void Insert()
        {
            if (Validate()) DbManager.Db?.Send(ISql.InsertQuery<Product>(this));
        }

        public static void Delete(params object[] pk)
        {
            ISql.Erase<Product>(pk);
        }
    }
}
