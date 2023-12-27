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
    [DbTableName("structure")]
    public class Structure : ISql
    {
        [DbPrimaryKey(NpgsqlDbType.Varchar, 50)]
        [DbAttribute("name")]
        public string? Name { get; set; }

        [DbAttribute("head_office_address")]
        public string? HeadOfficeAddress { get; set; }

        [DbRelation("city_id", "id")]
        public City? City { get; set; }

        public Structure() { }

        public Structure(string name)
        {
            Structure? s = ISql.Get<Structure>(name);
            if (s == null) throw new KeyNotFoundException();
            Name = s.Name;
            HeadOfficeAddress = s.HeadOfficeAddress;
            City = s.City;
        }


        public void Insert()
        {
            throw new NotImplementedException();
        }
    }
}
