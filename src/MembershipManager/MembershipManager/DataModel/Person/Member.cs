using MembershipManager.Engine;
using MembershipManager.DataModel.Company;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace MembershipManager.DataModel.Person
{
    public class Member : Person
    {
        [DbRelation("structure_name")]
        public Structure Structure { get; set; }

        [DbAttribute("subscription_date")]
        public DateTime SubscriptionDate { get; set; }

        public Member() : base()
        {
            this.SubscriptionDate = DateTime.Now;
        }

        public Member(string noAvs) : base(noAvs)
        {

            Member? m = (Member?)Get(noAvs);
            if (m == null) throw new KeyNotFoundException();
            Structure = m.Structure;
            SubscriptionDate = m.SubscriptionDate;
        }

        public new ISql? Get(object pk)
        {
            NpgsqlCommand cmd = new();
            cmd.CommandText = $"SELECT * FROM member WHERE no_avs = @value1";
            NpgsqlParameter param = new NpgsqlParameter("@value1", NpgsqlDbType.Char, 13) { Value = pk };
            cmd.Parameters.Add(param);
            return DbManager.Db?.Receive<Member>(cmd).FirstOrDefault();
        }

    }
}
