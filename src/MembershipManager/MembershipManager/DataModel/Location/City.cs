using MembershipManager.Engine;
using MembershipManager.View.Utils.ListSelectionForm;
using NpgsqlTypes;
using System.Linq;

namespace MembershipManager.DataModel
{
    /// <summary>
    /// This class represents a city of switzerland
    /// </summary>

    [DbTableName("city")]
    public class City : ISql, IComparable
    {
        [DbPrimaryKey(NpgsqlDbType.Integer)]
        [DbAttribute("id")]
        public int Id { get; private set; }
        [DbAttribute("name")]
        [Displayed("Ville")]
        [Filtered("Nom de la ville")]
        public string? Name { get; private set; }

        [Filtered("NPA")]
        [Displayed("NPA")]
        [DbAttribute("npa")]
        public int NPA { get; private set; }

        [Displayed("Canton")]
        [DbRelation("canton_abbreviation")]
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
            return Cities.FirstOrDefault(c => c.Id == (int)pk[0]); 
        }

        
        public override string ToString()
        {
            return $"{NPA} {Name}";
        }

        bool ISql.Validate()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object? obj)
        {
            return ToString().CompareTo(obj?.ToString());
        }

        public static List<City> Cities { get; set; } = new List<City>();

    }
}
