using MembershipManager.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipManager.DataModel.Company
{
    public class Structure
    {
        [DbAttribute("name")]
        public string Name { get; set; }

        [DbAttribute("head_office_address")]
        public string? HeadOfficeAddress { get; set; }

        [DbRelation("city_id")]
        City? City { get; set; }
    }
}
