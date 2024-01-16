using MembershipManager.DataModel.People;
using MembershipManager.Engine;
using MembershipManager.View.People.Person;
using MembershipManager.View.Utils.ListSelectionForm;

namespace MembershipManager.DataModel.Financial
{
    public class PaiementView : SqlViewable
    {
        [Displayed("Id")]
        [Filtered("Id")]
        public string? id { get; set; }

        [Sorted]
        [Filtered("Montant")]
        [Displayed("Montant")]
        public string? amount { get; set; }

        [Filtered("Date")]
        [Displayed("Date")]
        public string? date { get; set; }

        [Filtered("Prénom")]
        [Displayed("Prénom")]
        public string? first_name { get; set; }

        [Displayed("Nom")]
        public string? last_name { get; set; }

        [IgnoreSql]
        [Filtered("Tout", true)]
        public string? FullName { get => $"{first_name} {last_name}"; }
    }
}