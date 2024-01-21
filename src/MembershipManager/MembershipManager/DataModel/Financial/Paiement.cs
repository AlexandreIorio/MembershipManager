using MembershipManager.Engine;
using MembershipManager.View.Utils.ListSelectionForm;
using Npgsql;
using NpgsqlTypes;
using System.Text;
using System.Windows;

namespace MembershipManager.DataModel.Financial
{
    [DbTableName("paiement")]
    public class Paiement : ISql, Ilistable
    {

        [DbPrimaryKey(NpgsqlDbType.Integer)]
        [DbAttribute("id")]
        public int? Id { get; set; }

        [DbRelation("account_id")]
        public MemberAccount? Account { get; set; }

        [DbAttribute("date")]
        public DateTime? Date { get; set; }

        [DbAttribute("amount")]
        public int? Amount { get; set; }

        [DbAttribute("payed")]
        public bool Payed { get; set; } = false;

        public Paiement() { }

        public Paiement(Paiement paiement)
        {
            Id = paiement.Id;
            Account = paiement.Account;
            Date = paiement.Date;
            Amount = paiement.Amount;
            Payed = paiement.Payed;
        }

        public static ISql? Select(params object[] pk)
        {
            if (pk.Length != 1) throw new ArgumentException();
            Paiement? p = ISql.Get<Paiement>(pk[0]);
            return p == null ? throw new KeyNotFoundException() : (ISql)p;
        }

        public void Insert()
        {
            if (Validate()) DbManager.Db?.Send(ISql.InsertQuery<Paiement>(this));
        }

        public void Update()
        {
            if (Validate()) DbManager.Db?.Send(ISql.UpdateQuery<Paiement>(this));
        }

        public bool Validate()
        {
            StringBuilder message = new();
            bool valid = true;
            if (Account is null)
            {
                message.AppendLine("Un lien vers un compte est obligatoire");
                valid = false;
            }
            if (Amount == 0)
            {
                message.AppendLine("La valeur d'un paiement ne peut pas être 0");
                valid = false;
            }
            if (!Date.HasValue)
            {
                message.AppendLine("La date doit être spécifiée");
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

        public override string ToString()
        {
            return $"{Amount} {Date}";
        }

        public static List<SqlViewable>? Views(params NpgsqlParameter[] sqlParam)
        {
            if (sqlParam.Length > 2) throw new ArgumentException();

            NpgsqlCommand cmd = new();

            StringBuilder SqlQuery = new(@"SELECT paiement.id, paiement.payed, paiement.account_id, amount, date, p.first_name, p.last_name
                                    FROM paiement
                                    INNER JOIN  memberaccount ON paiement.account_id = memberaccount.id
                                    INNER JOIN member AS m ON memberaccount.id = m.no_avs
                                    INNER JOIN person AS p ON m.no_avs = p.no_avs");


            if (sqlParam.Length == 2)
            {
                SqlQuery.Append(" WHERE paiement.account_id = @id AND Payed = @payed");
                cmd.Parameters.AddRange(sqlParam);
            }
            else if (sqlParam.Length == 1 && sqlParam[0].ParameterName.Equals("@id"))
            {
                SqlQuery.Append(" WHERE paiement.account_id = @id");
                cmd.Parameters.Add(sqlParam[0]);
            }
            else if (sqlParam.Length == 1 && sqlParam[0].ParameterName.Equals("@payed"))
            {
                SqlQuery.Append(" WHERE Payed = @payed");
                cmd.Parameters.Add(sqlParam[0]);
            }



            cmd.CommandText = SqlQuery.ToString();

            return DbManager.Db.Views<PaiementView>(cmd).Cast<SqlViewable>().ToList();
        }

        public static void Delete(params object[] pk)
        {
            ISql.Erase<Paiement>(pk);
        }
    }
}
