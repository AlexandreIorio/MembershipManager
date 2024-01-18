using MembershipManager.DataModel.Financial;
using MembershipManager.Engine;
using MembershipManager.View.Utils.ListSelectionForm;
using Npgsql;
using System.Text;
using System.Windows;

namespace MembershipManager.DataModel.Buyable
{
    [DbTableName("consumption")]
    class Consumption : ISql, Ilistable
    {
        [DbPrimaryKey(NpgsqlTypes.NpgsqlDbType.Integer)]
        [DbAttribute("id")]
        public int? Id { get; set; }

        [DbAttribute("date")]
        public DateTime? Date { get; set; }

        [DbAttribute("name")]
        public string? Name { get; }

        [DbAttribute("code")]
        public string? Code { get; }

        [DbAttribute("amount")]
        public int? Amount { get; }

        [DbRelation("account_id")]
        public MemberAccount? Account { get; set; }

        public Consumption(Product product)
        {
            this.Code = product.Code;
            this.Name = product.Name;
            this.Amount = product.Amount;
        }

        public void Insert()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public static ISql? Select(params object[] pk)
        {
            throw new NotImplementedException();
        }

        public bool Validate()
        {
            StringBuilder message = new StringBuilder();
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

        public static List<SqlViewable>? Views(params NpgsqlParameter[] sqlParam)
        {
            if (sqlParam.Length > 1) throw new ArgumentException();

            NpgsqlCommand cmd = new NpgsqlCommand();

            StringBuilder SqlQuery = new StringBuilder(@"SELECT * FROM consumption");


            if (sqlParam.Length == 1)
            {
                SqlQuery.Append(" WHERE account_id = @id");
                //TODO trouver un moyen de cloner le paramêtre
                NpgsqlParameter param = new NpgsqlParameter("@id", sqlParam[0].NpgsqlValue);
                cmd.Parameters.Add(param);
            }

            cmd.CommandText = SqlQuery.ToString();

            return DbManager.Db.Views<ConsumptionView>(cmd).Cast<SqlViewable>().ToList();
        }

        public static void Delete(params object[] pk)
        {
            throw new NotImplementedException();
        }
    }
}
