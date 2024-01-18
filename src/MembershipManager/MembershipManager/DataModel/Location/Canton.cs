using MembershipManager.Engine;
using NpgsqlTypes;

namespace MembershipManager.DataModel
{
    /// <summary>
    /// This class represents a canton of switzerland
    /// </summary>
    [DbTableName("canton")]
    public class Canton : ISql
    {
        [DbPrimaryKey(NpgsqlDbType.Char, 2)]
        [DbAttribute("abbreviation")]
        public string? Abbreviation { get; private set; }

        [DbAttribute("name")]
        public string? Name { get; private set; }

        public Canton() { }



        public void Insert()
        {
            throw new NotImplementedException();
        }

        public static ISql? Select(params object[] pk)
        {

            return Cantons.FirstOrDefault(c => c.Abbreviation == (string)pk[0]);

        }

        public override string ToString()
        {
            return $"{Name} - {Abbreviation}";
        }

        bool ISql.Validate()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public static void Delete(params object[] pk)
        {
            throw new NotImplementedException();
        }

        public static List<Canton> Cantons { get; set; } = new List<Canton>();
    }
}
