using MembershipManager.Engine;
using MembershipManager.View.Utils.ListSelectionForm;
using Npgsql;

namespace MembershipManager.DataModel.Buyable
{
    /// <summary>
    /// <inheritdoc/>
    /// Specialized for <see cref="Product"/>
    /// </summary>
    class ProductView : SqlViewable, Ilistable
    {
        /// <summary>
        /// Id of the product
        /// </summary>
        public int? id { get; set; }

        /// <summary>
        /// Code of the product
        /// </summary>
        [Filtered("Code")]
        [Displayed("Code")]
        public string? Code { get; set; }

        /// <summary>
        /// Name of the product
        /// </summary>
        [Filtered("Nom")]
        [Displayed("Nom")]
        public string? Name { get; set; }

        /// <summary>
        /// Amount of the product, in cents.
        /// </summary>
        public int? Amount { get; set; }

        /// <summary>
        /// The computed amount of the product, in francs.
        /// </summary>
        [IgnoreSql]
        [TextFormat("{0:N2}")]
        [Filtered("Prix")]
        [Displayed("Prix")]
        public double ComputedAmount { get => (Amount ?? 0) / 100.0; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="sqlParam"></param>
        /// <returns></returns>
        public static List<SqlViewable>? Views(params NpgsqlParameter[] sqlParam)
        {
            NpgsqlCommand cmd = new()
            {
                CommandText = @"SELECT * FROM Product;"
            };

            return DbManager.Db.Views<ProductView>(cmd).Cast<SqlViewable>().ToList();
        }
    }
}
