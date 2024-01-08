using MembershipManager.Engine;
using Npgsql;
using NpgsqlTypes;

namespace MembershipManager.DataModel
{
    [DbTableName("city")]
    public class City : ISql
    {
        [DbPrimaryKey(NpgsqlDbType.Integer)]
        [DbAttribute("id")]
        public int Id { get; private set; }

        [DbAttribute("city_name")]
        public string? Name { get; private set; }

        [DbAttribute("npa")]
        public int NPA { get; private set; }

        [DbRelation("canton_abbreviation", "abbreviation")]
        public Canton? Canton { get; private set; }

        public City() { }

       
        public void Insert()
        {
            throw new NotImplementedException();
        }

        public void Select(params object[] pk)
        {
            
            if (pk.Length != 1) throw new ArgumentException();
            City? c = ISql.Get<City>(pk[0]);
            if (c == null) throw new KeyNotFoundException();
            Id = c.Id;
            Name = c.Name;
            NPA = c.NPA;
            Canton = c.Canton;
        }
    }
}
