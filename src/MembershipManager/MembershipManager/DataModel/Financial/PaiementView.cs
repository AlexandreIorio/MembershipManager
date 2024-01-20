using MembershipManager.Engine;
using MembershipManager.View.Utils.ListSelectionForm;

namespace MembershipManager.DataModel.Financial
{
    public class PaiementView : SqlViewable, ITransaction
    {

        public int? id { get; set; }

        [Sorted]
        [Filtered("Date")]
        [Displayed("Date")]

        public DateTime? Date { get; set; }
        [Filtered("Prénom")]
        [Displayed("Prénom")]
        public string? first_name { get; set; }

        [Displayed("Nom")]
        public string? last_name { get; set; }

        [Filtered("Montant")]
        [Displayed("Montant")]
        public int? Amount { get; set; }

        public bool? Payed { get; set; }

        [IgnoreSql]
        [Filtered("Tout", true)]
        public string? FullName { get => $"{first_name} {last_name}"; }

        [IgnoreSql]
        public string? Description => $"Paiement du: {Date?.ToString("dd.MM.yyyy")}";


        [IgnoreSql]
        public double ComputedAmount => Math.Round(((Amount ?? 0) / 100.0), 2);

        public string? Account_id { get; set; }
    }
}