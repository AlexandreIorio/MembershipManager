using MembershipManager.Engine;

namespace MembershipManager.DataModel
{
    public class City
    {
        [DbAttribute("city_name")]
        public string? Name { get; private set; }

        [DbAttribute("npa")]
        public int NPA { get; private set; }

        [DbRelation("canton_abbreviation")]
        public Canton? Canton { get; private set; }
    }
}
