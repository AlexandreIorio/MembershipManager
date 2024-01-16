using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipManager.DataModel.Financial
{
    public interface ITransaction
    {
        enum TransactionType
        {
            DEBIT = 1, CREDIT = -1
        }
        Account? Account { get; set; }
        DateTime? Date { get; set; }
        string? Description { get; set; }
        int? Amount { get; set; }
        TransactionType? Type { get; set; }
        
        public int GetAmount()
        {
            if (Amount is null || Type is null) return 0;
            return (int)Amount * (int)Type;
        }
    }
}
