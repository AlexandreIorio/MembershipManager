﻿using MembershipManager.Engine;
using Npgsql;
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

        public double Balance => _transactions?.Sum(t => t.ComputedAmount) ?? 0;

        [IgnoreSql]
        private List<ITransaction> _transactions => ITransaction.Views(new NpgsqlParameter("@id", NoAvs))?.Cast<ITransaction>().ToList();
    }
}
