using MembershipManager.DataModel;
using MembershipManager.DataModel.Financial;
using MembershipManager.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipManager.Resources
{
    [DbTableName("setting")]
    public class Settings : ISql
    {
        public static Settings Values { get; internal set; }


        [DbPrimaryKey(NpgsqlTypes.NpgsqlDbType.Integer)]
        [DbAttribute("id")]
        public int? Id { get; set; }

        [DbAttribute("payment_terms")]
        public int? PaymentTerms { get;set; }

        public static void Delete(params object[] pk)
        {
            throw new NotImplementedException();
        }

        public static ISql? Select(params object[] pk)
        {
            Settings? s = ISql.Get<Settings>();
            return s;
        }

        public void Insert()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public bool Validate()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
