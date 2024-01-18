using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipManager.DataModel.Buyable
{
    public class Product
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? Amount { get; set; }

        public Product(Product product) {
            Code = product.Code;
            Name = product.Name;
            Amount = product.Amount;
        }

    }
}
