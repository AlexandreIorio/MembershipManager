using MembershipManager.DataModel.Buyable;
using MembershipManager.Engine;
using MembershipManager.Resources;
using MembershipManager.View.Utils.ListSelectionForm;
using Npgsql;
using System.Data;
using System.Text;
using System.Windows;

namespace MembershipManager.DataModel.Financial
{
    [DbTableName("bill")]
    [DbInherit(typeof(Paiement))]
    public class Bill : Paiement, ISql
    {

        [DbAttribute("issue_date")]
        public DateTime IssueDate { get; set; }

        [DbAttribute("payed_date")]
        public DateTime? PayedDate { get; set; }

        [DbAttribute("payed_amount")]
        public int? PayedAmount { get; set; }

        public Bill() : base()
        {
        }

        public Bill(Paiement paiement) : base(paiement)
        {
        }

        /// <summary>
        /// This method is used to generate a bill for a member and assign consumptions to it
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void Generate()
        {
            if (Account is null) throw new NullReferenceException("Account is null");

            double billeableAmount = Account.Balance + Account.PendingAmount;
            if (billeableAmount < 0)
            {
                Amount = (int)(-billeableAmount * 100);
                Date = DateTime.Now;
                Insert();
                AssignConsumptions();
            }
            else
            {
                MessageBox.Show("Aucun montant facturable",
                                       "Information",
                                        System.Windows.MessageBoxButton.OK,
                                        System.Windows.MessageBoxImage.Information);
            }
        }

        private void AssignConsumptions()
        {
            if (Id is null) throw new NullReferenceException("Id is null");
            List<Consumption> consumptions = GetConsumptions();

            List<int?> consumptionIds = new();
            int? amount = Amount;
            if (amount == null) return;
            foreach (Consumption consumption in consumptions)
            {
                if (amount <= 0) break;
                if (consumption.Bill is not null) continue;
                consumptionIds.Add(consumption.Id);
                amount -= consumption.Amount;
            }

            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sb = new();

            for (int i = 0; i < consumptionIds.Count(); i++)
            {
                if (consumptionIds[i] is null) continue;
                sb.Append($"UPDATE consumption SET bill_id = @bill_id WHERE id = @id{i};");
                cmd.Parameters.AddWithValue($"@id{i}", consumptionIds[i]);
            }

            cmd.Parameters.AddWithValue("@bill_id", Id);
            cmd.CommandText = sb.ToString();

            DbManager.Db?.Send(cmd);
        }

        public List<Consumption> GetConsumptions()
        {
            Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand();
            cmd.CommandText = "SELECT * FROM consumption WHERE account_id = @account_id";
            cmd.Parameters.AddWithValue("@account_id", Account.NoAvs);
            return DbManager.Db.Recieve<Consumption>(cmd);
        }

        public void CheckAccount()
        {
            if (Account is null) throw new NullReferenceException("Account is null");
            if (Account.Balance < 0) return;

            MessageBoxResult result = MessageBox.Show("Le solde du compte est positif, aucun paiement n'est nécessaire,\nVoulez-vous marquer la facture comme payée ? ",
                                                      "Information",
                                                        System.Windows.MessageBoxButton.YesNo,
                                                        System.Windows.MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                Payed = true;
                Amount = (int)(-Account.Balance * 100);
                Date = DateTime.Now;
                if (Validate()) DbManager.Db?.Send(ISql.InsertQuery<Bill>(this));
            }

        }

        public new static ISql? Select(params object[] pk)
        {

            if (pk[0] is DBNull) return null;
            if (pk.Length != 1) throw new ArgumentException();
            Bill? bill = ISql.Get<Bill>(pk[0]);
            return bill == null ? throw new KeyNotFoundException() : (ISql)bill;
        }

        public new void Insert()
        {
            if (Validate())
            {
                Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand();
                cmd.CommandText = @"SELECT insert_paiement_and_bill(@amount, @account_id, @date, @payed, @issue_date, @payed_date, @payed_amount);";

                cmd.Parameters.AddWithValue("@amount", Amount);
                cmd.Parameters.AddWithValue("@account_id", NpgsqlTypes.NpgsqlDbType.Varchar, 13, Account.NoAvs);
                cmd.Parameters.AddWithValue("@date", NpgsqlTypes.NpgsqlDbType.Date, Date);
                cmd.Parameters.AddWithValue("@payed", Payed);

                cmd.Parameters.AddWithValue("@issue_date", NpgsqlTypes.NpgsqlDbType.Date, IssueDate);
                cmd.Parameters.AddWithValue("@payed_date", NpgsqlTypes.NpgsqlDbType.Date, PayedDate is null ? DBNull.Value : PayedDate);
                cmd.Parameters.AddWithValue("@payed_amount", NpgsqlTypes.NpgsqlDbType.Integer, PayedAmount is null ? DBNull.Value : PayedAmount);

                Id = (int?)DbManager.Db?.InsertReturnigIds(cmd)[0][0];
            }
        }

        public new void Update()
        {
            if (Validate()) DbManager.Db?.Send(ISql.UpdateQuery<Bill>(this));
        }

        public new bool Validate()
        {
            StringBuilder message = new StringBuilder();
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
        public static new void Delete(params object[] pk)
        {
            ISql.Erase<Paiement>(pk);
        }

        public new static List<SqlViewable>? Views(params NpgsqlParameter[] sqlParam)
        {
            if (sqlParam.Length > 2) throw new ArgumentException();

            NpgsqlCommand cmd = new NpgsqlCommand();

            StringBuilder SqlQuery = new StringBuilder(@"SELECT bill.id, bill.issue_date, bill.payed_date, bill.payed_amount, paiement.payed, paiement.account_id, amount, date, person.first_name, person.last_name
                                                        FROM Bill
                                                            LEFT JOIN paiement ON bill.id = paiement.id
                                                            LEFT JOIN memberaccount ON paiement.account_id = memberaccount.id
                                                            LEFT JOIN person ON memberaccount.id = person.no_avs;");
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

            return DbManager.Db.Views<BillView>(cmd).Cast<SqlViewable>().ToList();
        }

        public void ChangeStatus()
        {
            if (Payed == true)
            {
                MessageBoxResult result = MessageBox.Show("La facture est déjà payée, voulez-vous changer son status", "Information", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    Payed = false;
                    PayedDate = null;
                    PayedAmount = null;
                    base.Update();
                    Update();

                }
                return;
            }
            Payed = true;
            PayedDate = DateTime.Now;
            PayedAmount = Amount;
            base.Update();
            Update();
        }
    }
}
