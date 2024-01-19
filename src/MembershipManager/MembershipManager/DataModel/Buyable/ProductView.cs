using MembershipManager.DataModel.People;
using MembershipManager.Engine;
using MembershipManager.View.Utils.ListSelectionForm;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipManager.DataModel.Buyable
{
    class ProductView : SqlViewable, Ilistable
    {
        public int? id { get; set; }

        [Filtered("Code")]
        [Displayed("Code")]
        public string? Code { get; set; }

        [Filtered("Nom")]
        [Displayed("Nom")]
        public string? Name { get; set; }

        public int? Amount { get; set; }

        [IgnoreSql]
        [TextFormat("{0:N2}")]
        [Filtered("Prix")]
        [Displayed("Prix")]
        public double ComputedAmount { get => (Amount ?? 0) / 100.0; }


        public static List<SqlViewable>? Views(params NpgsqlParameter[] sqlParam)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT * FROM Product;";

            return DbManager.Db.Views<ProductView>(cmd).Cast<SqlViewable>().ToList();
        }
    }
}
