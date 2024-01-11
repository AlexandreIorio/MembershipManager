using MembershipManager.Engine;
using MembershipManager.View.Utils.ListSelectionForm;
using Npgsql;
using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;

namespace MembershipManager.DataModel
{
    [DbTableName("city")]
    public class City : ISql
    {
        [DbPrimaryKey(NpgsqlDbType.Integer)]
        [DbAttribute("id")]
        public int Id { get; private set; }
        [DbAttribute("city_name")]
        [Displayed("Ville")]
        [Filtered("Nom de la ville")]
        public string? Name { get; private set; }

        [Filtered("NPA")]
        [Displayed("NPA")]
        [DbAttribute("npa")]
        public int NPA { get; private set; }

        [Displayed("Canton")]
        [DbRelation("canton_abbreviation", "abbreviation")]
        public Canton? Canton { get; private set; }

        [Filtered("Tous", true)]
        public string? FullName { get=> ToString(); } 

        public City() { }

        public void Insert()
        {
            throw new NotImplementedException();
        }

        public static ISql? Select(params object[] pk)
        {

            if (pk.Length != 1) throw new ArgumentException();
            City? c = ISql.Get<City>(pk[0]);
            if (c == null) throw new KeyNotFoundException();

            return c;
        }

        
        public override string ToString()
        {
            return $"{NPA} {Name}";
        }

        public static List<City>? Cities { get; set; }

    }
}
