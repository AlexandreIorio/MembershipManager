using MembershipManager.Engine;
using Npgsql;
using NpgsqlTypes;

namespace MembershipManager.DataModel
{
    public class Canton : ISql
    {
        [DbAttribute("abbreviation")]
        public string? Abbreviation { get; private set; }

        [DbAttribute("canton_name")]
        public string? Name { get; private set; }

        public Canton() { } 

        public Canton(string abbreviation)
        {
            Canton? c = (Canton?)Get(abbreviation);
            if (c == null) throw new KeyNotFoundException();
            Abbreviation = c.Abbreviation;
            Name = c.Name;
        }

        public ISql? Get(object pk)
        {
            NpgsqlCommand cmd = new();
            cmd.CommandText = $"SELECT * FROM canton WHERE abbreviation = @value1";
            NpgsqlParameter param = new NpgsqlParameter("@value1", NpgsqlDbType.Char, 2) { Value = pk };
            cmd.Parameters.Add(param);
            return DbManager.Db?.Receive<Canton>(cmd).FirstOrDefault();
        }
    }
}
