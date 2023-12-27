using MembershipManager.Engine;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipManager.DataModel.Company
{
    public class Franchise
    {
        [DbAttribute("id")]
        public int Id { get; set; }

        [DbAttribute("strucutre_name")]
        public string? StructureName { get; set; }

        [DbAttribute("address")]
        public string? Address { get; set; }

        [DbRelation("city_id")]
        City? City { get; set; }


        public static Franchise? GetFranchise(int id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = $"SELECT * FROM get_franchise WHERE id = @value1";
            NpgsqlParameter param = new NpgsqlParameter("@value1", NpgsqlTypes.NpgsqlDbType.Integer) { Value = id };
            cmd.Parameters.Add(param);

            return DbManager.Db?.Receive<Franchise>(cmd).First() ?? null;
        }

    }
}
