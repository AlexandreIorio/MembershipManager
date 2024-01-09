using MembershipManager.Engine;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipManager.DataModel.Company
{
    [DbTableName("franchise")]
    public class Franchise : ISql
    {
        [DbPrimaryKey(NpgsqlDbType.Integer)]
        [DbAttribute("id")]
        public int Id { get; set; }

        [DbAttribute("strucutre_name")]
        public string? StructureName { get; set; }

        [DbAttribute("address")]
        public string? Address { get; set; }

        [DbRelation("city_id", "id")]
        City? City { get; set; }

        public Franchise() { }

        public void Insert()
        {
            
        }


        public static ISql? Select(params object[] pk)
        {

            if (pk.Length != 1) throw new ArgumentException();
            Franchise? f = ISql.Get<Franchise>(pk[0]);
            if (f == null) throw new KeyNotFoundException();

            return f;

        }


    }
}
