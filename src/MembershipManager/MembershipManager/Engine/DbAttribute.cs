using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipManager.Engine
{
    internal class DbAttribute : Attribute
    {
        public string Name { get; private set; }
        public DbAttribute(string attributeName)
        {
            this.Name = attributeName;
        }
    }

    internal class DbRelation : Attribute
    {
        public string Name { get; private set; }
        public DbRelation(string Name) { 
            this.Name = Name;
        }

    }
}
