using Npgsql;

namespace MembershipManager.View.Utils.ListSelectionForm
{
    public interface Ilistable
    {
        /// <summary>
        /// This property is used to store sql tuples of a sql query
        /// </summary>
        public abstract static List<SqlViewable>? Views(params NpgsqlParameter[] sqlParam);

    }
}
