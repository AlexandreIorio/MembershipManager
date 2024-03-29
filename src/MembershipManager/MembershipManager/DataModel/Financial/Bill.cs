﻿using MembershipManager.DataModel.Buyable;
using MembershipManager.Engine;
using MembershipManager.Resources;
using MembershipManager.View.Utils.ListSelectionForm;
using Npgsql;
using System.Data;
using System.Text;
using System.Windows;

namespace MembershipManager.DataModel.Financial
{
    /// <summary>
    /// lass representing an open paiement for a member, paiement can be payed or not
    /// <inheritdoc/>
    /// </summary>
    [DbTableName("bill")]
    [DbInherit(typeof(Paiement))]
    public class Bill : Paiement, ISql
    {
        /// <summary>
        /// Enum to represent the status of a bill
        /// </summary>
        public enum BillStatus
        {
            Payed,
            Pending,
            Expired
        }

        /// <summary>
        /// Name of the status of a bill
        /// </summary>
        public static string[] BillStatusNames { get => new string[] { "Payée", "Attente", "Expirée" }; }

        /// <summary>
        /// Issue date of the bill define by the date of the paiement + the payment terms defined in the config
        /// </summary>
        [DbAttribute("issue_date")]
        public DateTime? IssueDate
        {
            get
            {
                return Date?.AddDays(Settings.Values.PaymentTerms ?? 30);
            }
            set
            {
                _issueDate = value;
            }

        }

        private DateTime? _issueDate;

        /// <summary>
        /// Date of the paiement
        /// </summary>
        [DbAttribute("payed_date")]
        public DateTime? PayedDate { get; set; }

        /// <summary>
        /// Amount payed by the member
        /// </summary>
        [DbAttribute("payed_amount")]
        public int? PayedAmount { get; set; }

        /// <summary>
        /// basic constructor
        /// <inheritdoc/>
        /// </summary>
        public Bill() : base()
        {
        }

        /// <summary>
        /// Constructor used to create a new bill
        /// <inheritdoc/>
        /// </summary>
        /// <param name="paiement"></param>
        public Bill(Paiement paiement) : base(paiement)
        {
        }

        /// <summary>
        /// This method is used to generate a bill for a member and assign consumptions to it
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public bool Generate()
        {
            if (Account is null) throw new NullReferenceException("Account is null");
            if (Account.Balance is null || Account.Balance > 0) return false;
            if (Account.PendingAmount is null) return false;

            double? billeableAmount = Account.Balance + Account.PendingAmount;
            if (billeableAmount < 0)
            {
                Amount = (int)(-billeableAmount * 100);
                Date = DateTime.Now;
                Insert();
                AssignConsumptions();
                BillView.Consumptions = Consumption.Views()?.Cast<ConsumptionView>().ToList();
                return true;
            }

            return false;
        }


        private void AssignConsumptions()
        {
            if (Id is null) throw new NullReferenceException("Id is null");
            List<Consumption> consumptions = GetConsumptions();
            consumptions = consumptions.OrderByDescending(c => c.Date).ToList();

            List<int?> consumptionIds = [];
            int? amount = Amount;
            if (amount == null) return;
            foreach (Consumption consumption in consumptions)
            {
                if (amount <= 0) break;
                if (consumption.Bill is not null) continue;
                consumptionIds.Add(consumption.Id);
                amount -= consumption.Amount;
            }

            NpgsqlCommand cmd = new();
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

        /// <summary>
        /// Method to get all consumptions of the member related to the current bill
        /// </summary>
        /// <returns></returns>
        public List<Consumption> GetConsumptions()
        {
            Npgsql.NpgsqlCommand cmd = new()
            {
                CommandText = "SELECT * FROM consumption WHERE account_id = @account_id"
            };
            cmd.Parameters.AddWithValue("@account_id", Account.NoAvs);
            return DbManager.Db.Recieve<Consumption>(cmd);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public new static ISql? Select(params object[] pk)
        {

            if (pk[0] is DBNull) return null;
            if (pk.Length != 1) throw new ArgumentException();
            Bill? bill = ISql.Get<Bill>(pk[0]);
            return bill == null ? throw new KeyNotFoundException() : (ISql)bill;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public new void Insert()
        {
            if (Validate())
            {
                Npgsql.NpgsqlCommand cmd = new()
                {
                    CommandText = @"SELECT insert_paiement_and_bill(@amount, @account_id, @date, @payed, @issue_date, @payed_date, @payed_amount);"
                };

                cmd.Parameters.AddWithValue("@amount", Amount);
                cmd.Parameters.AddWithValue("@account_id", NpgsqlTypes.NpgsqlDbType.Varchar, 13, Account.NoAvs);
                cmd.Parameters.AddWithValue("@date", NpgsqlTypes.NpgsqlDbType.Date, Date is null ? DBNull.Value : Date);
                cmd.Parameters.AddWithValue("@payed", Payed);

                cmd.Parameters.AddWithValue("@issue_date", NpgsqlTypes.NpgsqlDbType.Date, IssueDate is null ? DBNull.Value : IssueDate);
                cmd.Parameters.AddWithValue("@payed_date", NpgsqlTypes.NpgsqlDbType.Date, PayedDate is null ? DBNull.Value : PayedDate);
                cmd.Parameters.AddWithValue("@payed_amount", NpgsqlTypes.NpgsqlDbType.Integer, PayedAmount is null ? DBNull.Value : PayedAmount);

                Id = (int?)DbManager.Db?.InsertReturnigIds(cmd)[0][0];
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public new void Update()
        {
            if (Validate()) DbManager.Db?.Send(ISql.UpdateQuery<Bill>(this));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public new bool Validate()
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
        public static new void Delete(params object[] pk)
        {
            ISql.Erase<Paiement>(pk);
        }

        /// <summary>
        /// Method to get all bills of a member
        /// </summary>
        /// <param name="sqlParam"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public new static List<SqlViewable>? Views(params NpgsqlParameter[] sqlParam)
        {
            if (sqlParam.Length > 2) throw new ArgumentException();

            NpgsqlCommand cmd = new();

            StringBuilder SqlQuery = new(@"SELECT bill.id, bill.issue_date, bill.payed_date, bill.payed_amount, paiement.payed, paiement.account_id, amount, date, person.first_name, person.last_name
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

        /// <summary>
        /// Method to change a bill status
        /// </summary>
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
