﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
