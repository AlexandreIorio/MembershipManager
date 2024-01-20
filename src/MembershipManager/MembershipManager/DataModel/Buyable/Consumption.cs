using MembershipManager.DataModel.Financial;
using MembershipManager.Engine;
using MembershipManager.View.Utils.ListSelectionForm;
using Npgsql;
using System.Text;
using System.Windows;

namespace MembershipManager.DataModel.Buyable
{
    /// <summary>
    /// <inheritdoc/>
    /// Consumption represent a product bought by a member.
    /// </summary>
    [DbTableName("consumption")]
    public class Consumption : Product, ISql, Ilistable
    {
        /// <summary>
        /// The id of the consumption.
        /// </summary>
        [DbAttribute("date")]
        public DateTime? Date { get; set; } = DateTime.Now;

        /// <summary>
        /// The account of the member who bought the product.
        /// </summary>
        [DbRelation("account_id")]
        public MemberAccount? Account { get; set; }

        /// <summary>
        /// The bill of the consumption. can be null if no bill was emitted.
        /// </summary>
        [DbRelation("bill_id")]
        public Bill? Bill { get; set; }

        /// <summary>
        /// basic constructor
        /// </summary>
        public Consumption() : base()
        { }

        /// <summary>
        /// Constructor used to create a new consumption
        /// </summary>
        /// <param name="product">The buy product</param>
        /// <param name="memberAccount">The memberAccount which buy the product</param>
        public Consumption(Product product, MemberAccount memberAccount) : base(product)
        {
            Account = memberAccount;
        }

        /// <summary>
        /// Method to set the product of a consumption
        /// <summary/>
        public void SetProduct(Product product)
        {
            if (product is null) return;
            Code = product.Code;
            Name = product.Name;
            Amount = product.Amount;

        }

        /// <summary>
        /// <inheritdoc/>
        /// <summary/>
        public void Update()
        {
            if (Validate()) DbManager.Db?.Send(ISql.UpdateQuery<Consumption>(this));
        }

        /// <summary>
        /// <inheritdoc/>
        /// <summary/>
        public new void Insert()
        {
            if (Validate()) DbManager.Db?.Send(ISql.InsertQuery<Consumption>(this));
        }

        /// <summary>
        /// <inheritdoc/>
        /// <summary/>
        public new static ISql? Select(params object[] pk)
        {
            if (pk.Length != 1) throw new ArgumentException();
            Consumption? c = ISql.Get<Consumption>(pk[0]);
            if (c == null) throw new KeyNotFoundException();

            return c;
        }

        /// <summary>
        /// <inheritdoc/>
        /// <summary/>
        public new bool Validate()
        {
            StringBuilder message = new();
            bool valid = true;
            if (Date is null)
            {
                message.AppendLine("La date est obligatoire");
                valid = false;
            }
            if (Amount is null)
            {
                message.AppendLine("Le montant est obligatoire");
                valid = false;
            }
            if (Name is null)
            {
                message.AppendLine("Le produit est obligatoire");
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

        /// <summary>
        /// <inheritdoc/>
        /// <summary/>
        public static List<SqlViewable>? Views(params NpgsqlParameter[] sqlParam)
        {
            if (sqlParam.Length > 1) throw new ArgumentException();

            NpgsqlCommand cmd = new();

            StringBuilder SqlQuery = new(@"SELECT * FROM consumption");


            if (sqlParam.Length == 1)
            {
                SqlQuery.Append(" WHERE account_id = @id");
                NpgsqlParameter param = new("@id", sqlParam[0].NpgsqlValue);
                cmd.Parameters.Add(param);
            }

            cmd.CommandText = SqlQuery.ToString();

            return DbManager.Db.Views<ConsumptionView>(cmd).Cast<SqlViewable>().ToList();
        }

        /// <summary>
        /// <inheritdoc/>
        /// <summary/>
        public new static void Delete(params object[] pk)
        {
            ISql.Erase<Consumption>(pk);
        }
    }
}
