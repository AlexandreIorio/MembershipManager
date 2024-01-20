using MembershipManager.DataModel.Financial;
using MembershipManager.Engine;
using MembershipManager.View.Utils.ListSelectionForm;

namespace MembershipManager.DataModel.Buyable
{
    /// <summary>
    /// <inheritdoc/>
    /// Specialized for <see cref="Consumption"/>
    /// </summary>
    public class ConsumptionView : SqlViewable, ITransaction
    {
        /// <summary>
        /// Id of the consumption
        /// </summary>
        public int? Id { get; set; }
        [Sorted]
        [Filtered("Code")]
        [Displayed("Code")]
        public string? Code { get; set; }

        /// <summary>
        /// Name of the consumption
        /// </summary>
        [Filtered("Nom")]
        [Displayed("Nom")]
        public string? Name { get; set; }

        /// <summary>
        /// Amount of the consumption, in cents.
        /// </summary>
        [Filtered("Prix")]
        [Displayed("Prix")]
        public int? Amount { get; set; }

        /// <summary>
        /// Date when the consumption was done.
        /// </summary>
        [Filtered("Prix")]
        [Displayed("Prix")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Description of the descritpion
        /// </summary>
        [IgnoreSql]
        public string? Description => Name;

        /// <summary>
        /// Account id of the member who did the consumption.
        /// </summary>
        public string? Account_id { get; set; }

        /// <summary>
        /// Bill id of the bill where the consumption is. Can be null if no bill was already created.
        /// </summary>
        public int? Bill_id { get; set; }

        /// <summary>
        /// The computed amount of the consumption, in francs.
        /// </summary>
        [IgnoreSql]
        public double ComputedAmount => Math.Round(-(Amount ?? 0) / 100.0, 2);
    }
}
