using MembershipManager.Engine;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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


        public void Insert()
        {
            throw new NotImplementedException();
        }

        public void Select(params object[] pk)
        {
            if (pk.Length != 1) throw new ArgumentException();
            Structure? s = ISql.Get<Structure>(pk[0]);
            if (s == null) throw new KeyNotFoundException();
            Name = s.Name;
            HeadOfficeAddress = s.HeadOfficeAddress;
            City = s.City;
        }
    }
}
