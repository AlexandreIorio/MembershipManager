using MembershipManager.DataModel.Company;
using MembershipManager.Engine;
using Npgsql;
using NpgsqlTypes;

namespace MembershipManager.DataModel
{
    [DbTableName("canton")]
    public class Canton : ISql
    {
        [DbPrimaryKey(NpgsqlDbType.Char, 2)]
        [DbAttribute("abbreviation")]
        public string? Abbreviation { get; private set; }

        [DbAttribute("canton_name")]
        public string? Name { get; private set; }

        public Canton() { } 

      

        public void Insert()
        {
            throw new NotImplementedException();
        }

        public static ISql? Select(params object[] pk)
        {

            if (pk.Length != 1) throw new ArgumentException();
            Canton? c = ISql.Get<Canton>(pk[0]);
            if (c == null) throw new KeyNotFoundException();

            return c;

        }
    }
}
