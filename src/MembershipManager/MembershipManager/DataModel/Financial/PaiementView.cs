using MembershipManager.DataModel.People;
using MembershipManager.Engine;
using MembershipManager.View.People.Person;
using MembershipManager.View.Utils.ListSelectionForm;

namespace MembershipManager.DataModel.Financial
{
    public class PaiementView : SqlViewable
    {

        public int? id { get; set; }

        [Sorted]
        [Filtered("Date")]
        [Displayed("Date")]
        public DateTime? date { get; set; }

        [Filtered("Prénom")]
        [Displayed("Prénom")]
        public string? first_name { get; set; }

        [Displayed("Nom")]
        public string? last_name { get; set; }

        [Filtered("Montant")]
        [Displayed("Montant")]
        public int? amount { get; set; }


        [IgnoreSql]
        [Filtered("Tout", true)]
        public string? FullName { get => $"{first_name} {last_name}"; }
    }
}