using MembershipManager.Engine;
using Npgsql;
using System.Data;
using System.Security.Principal;
using System.Transactions;

namespace MembershipManager.DataModel.Financial
{
    [DbTableName("memberaccount")]
    public class MemberAccount : ISql
    {
        [DbPrimaryKey(NpgsqlTypes.NpgsqlDbType.Char, 13)]
        [DbAttribute("id")]
        public string? NoAvs { get; set; }

        [DbAttribute("available_entry")]
        public int? AvailableEntry { get; set; }

        [DbAttribute("subscription_issue")]
        public DateTime? SubscriptionIssue { get; set; }

        public static ISql? Select(params object[] pk)
        {
            if (pk.Length != 1) throw new ArgumentException();
            MemberAccount? ma = ISql.Get<MemberAccount>(pk[0]);
            return ma == null ? throw new KeyNotFoundException() : (ISql)ma;
        }

        public static void Delete(params object[] pk)
        {
            throw new NotImplementedException();
        }

        public void Insert()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public bool Validate()
        {
            throw new NotImplementedException();
        }

        public double Balance => _finishedTransactions?.Sum(t => t.ComputedAmount) ?? 0;
        public double PendingAmount => _pendingTransactions?.Sum(t => t.ComputedAmount) ?? 0;

  
        private List<ITransaction> _finishedTransactions
        {
            get
            {
                NpgsqlParameter param = new NpgsqlParameter("@id", NoAvs);
                NpgsqlParameter param2 = new NpgsqlParameter("@payed", true);
                param2.DbType = DbType.Boolean;
                return ITransaction.Views(param, param2)?.Cast<ITransaction>().ToList();
            }
        }
 
        private List<PaiementView> _pendingTransactions
        {
            get
            {
                NpgsqlParameter param = new NpgsqlParameter("@id", NoAvs);
                NpgsqlParameter param2 = new("@payed", false);
                param2.DbType = DbType.Boolean;
                return Paiement.Views(param, param2)?.Cast<PaiementView>().ToList();
            }
        }
    }
}
