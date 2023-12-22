using MembershipManager.Engine;

namespace MembershipManager.DataModel
{
    public class Canton
    {
        [DbAttribute("abbreviation")]
        public string? Abbreviation { get; private set; }

        [DbAttribute("canton_name")]
        public string? Name { get; private set; }
    }
}
