﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipManager.DataModel.Financial
{
    internal interface ITransaction
    {
        Account? Account { get; set; }
        DateTime? Date { get; set; }
        string? Description { get; set; }
        int? Amount { get; set; }
    }
}
