using MembershipManager.Engine;
using System.Text;
using System.Windows;

namespace MembershipManager.DataModel.Buyable
{
    /// <summary>
    /// Class which represent a product.
    /// </summary>
    [DbTableName("product")]
    public class Product : ISql
    {
        /// <summary>
        /// Id of the product.
        /// </summary>
        [DbPrimaryKey(NpgsqlTypes.NpgsqlDbType.Integer)]
        [DbAttribute("id")]
        public int? Id { get; set; }

        /// <summary>
        /// Code of the product. As a bar code etc.
        /// </summary>
        [DbAttribute("code")]
        public string? Code { get; set; }

        /// <summary>
        /// Name of the product.
        /// </summary>
        [DbAttribute("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Amount of the product, in cents.
        /// </summary>
        [DbAttribute("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// Computed amount of the product, in francs.
        /// </summary>
        public double ComputedAmount { get => Amount ?? 0 / 100.0; set => Amount = (int?)(value * 100); }

        /// <summary>
        /// Basic constructor.
        /// </summary>
        public Product()
        {
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="product"></param>
        public Product(Product product)
        {
            Code = product.Code;
            Name = product.Name;
            Amount = product.Amount;
        }

        /// <summary>
        /// <inheritdoc/>
        /// Function to get a product from the database.
        /// throws <see cref="ArgumentException"/> if the number of primary key is not 1.
        /// throws <see cref="KeyNotFoundException"/> if the product is not found.
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public static ISql? Select(params object[] pk)
        {
            if (pk.Length != 1) throw new ArgumentException();
            Product? p = ISql.Get<Product>(pk[0]);
            if (p == null) throw new KeyNotFoundException();

            return p;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            StringBuilder message = new();
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
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
            }
            return valid;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Update()
        {
            if (Validate()) DbManager.Db?.Send(ISql.UpdateQuery<Product>(this));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Insert()
        {
            if (Validate()) DbManager.Db?.Send(ISql.InsertQuery<Product>(this));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="pk"></param>
        public static void Delete(params object[] pk)
        {
            ISql.Erase<Product>(pk);
        }
    }
}
