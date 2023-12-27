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
    public class Structure : ISql
    {
        [DbPrimaryKey(NpgsqlDbType.Varchar, 50)]
        [DbAttribute("name")]
        public string? Name { get; set; }

        [DbAttribute("head_office_address")]
        public string? HeadOfficeAddress { get; set; }

        [DbRelation("city_id")]
        public City? City { get; set; }

        public Structure() { }

        public Structure(string name)
        {
            Structure? s = (Structure?)Get(name);
            if (s == null) throw new KeyNotFoundException();
            Name = s.Name;
            HeadOfficeAddress = s.HeadOfficeAddress;
            City = s.City;
        }


        public ISql? Get(object pk)
        {
            NpgsqlCommand cmd = new();
            cmd.CommandText = $"SELECT * FROM structure WHERE name = @value1";
            NpgsqlParameter param = new NpgsqlParameter("@value1", NpgsqlDbType.Varchar, 50) { Value = pk };
            cmd.Parameters.Add(param);
            return DbManager.Db?.Receive<Structure>(cmd).FirstOrDefault();
        }

        public void Insert()
        {
            throw new NotImplementedException();
        }
    }
}
