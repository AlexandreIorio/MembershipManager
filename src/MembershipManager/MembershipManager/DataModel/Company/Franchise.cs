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

        public Franchise(object id)
        {
            Franchise? f = ISql.Get<Franchise>(id);
            if (f == null) throw new KeyNotFoundException();
            Id = f.Id;
            StructureName = f.StructureName;
            Address = f.Address;
            City = f.City;
        }

        public void Insert()
        {
            
        }
    }
}
