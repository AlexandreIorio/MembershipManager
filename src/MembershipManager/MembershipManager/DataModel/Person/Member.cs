using MembershipManager.Engine;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MembershipManager.DataModel.Person
{
    public class Member : Person
    {
        [DbAttribute("strucuture_name")]
        public string StructureName { get; set; }

        [DbAttribute("subscription_date")]
        public DateTime SubscriptionDate { get; set; }


        public static Person? getMember(object noAvs)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = $"SELECT * FROM get_member WHERE no_avs = @value1";
            NpgsqlParameter param = new NpgsqlParameter("@value1", NpgsqlDbType.Char, 13) { Value = noAvs };
            cmd.Parameters.Add(param);

            return DbManager.Db?.Receive<Member>(cmd).First() ?? null;
        }

    }
}
