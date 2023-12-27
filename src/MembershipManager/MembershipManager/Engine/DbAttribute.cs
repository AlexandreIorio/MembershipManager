using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipManager.Engine
{
    internal class DbNameable : Attribute
    {
        public string Name { get; private set; }
        public DbNameable(string attributeName)
        {
            this.Name = attributeName;
        }
    }

    internal class DbConstraint  : Attribute {}
    internal class DbAttribute (string attributeName) : DbNameable (attributeName) {}
    internal class DbRelation (string attributeName) : DbNameable(attributeName) {}
    internal class DbPrimaryKey : DbConstraint{}
}
