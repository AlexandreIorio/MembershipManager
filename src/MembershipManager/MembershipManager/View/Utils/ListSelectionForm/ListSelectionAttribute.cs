using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipManager.View.Utils.ListSelectionForm
{


    internal class Filtered(string friendlyName, bool isDefault = false) : Attribute
    {
        public bool IsDefault { get; set; } = isDefault;
        public string FriendlyName { get; set; } = friendlyName;
       
    }


    internal class Displayed(string headerName) : Attribute
    {
        public string HeaderName { get; set; } = headerName;
    }
}
