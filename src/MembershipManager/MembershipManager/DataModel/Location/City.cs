using MembershipManager.Engine;
using Npgsql;
using NpgsqlTypes;

namespace MembershipManager.DataModel
{
    public class City : ISql
    {
        [DbAttribute("city_name")]
        public string? Name { get; private set; }

        [DbAttribute("npa")]
        public int NPA { get; private set; }

        [DbRelation("canton_abbreviation")]
        public Canton? Canton { get; private set; }

        public City() { }

        public City(object id)
        {
            City? c = (City?)Get(id);
            if (c == null) throw new KeyNotFoundException();
            Name = c.Name;
            NPA = c.NPA;
            Canton = c.Canton;
        }

        public ISql? Get(object pk)
        {
            NpgsqlCommand cmd = new();
            cmd.CommandText = $"SELECT * FROM city WHERE id = @value1";
            NpgsqlParameter param = new NpgsqlParameter("@value1", NpgsqlDbType.Integer) { Value = pk };
            cmd.Parameters.Add(param);
            return DbManager.Db?.Receive<City>(cmd).FirstOrDefault();
        }
    }
}
