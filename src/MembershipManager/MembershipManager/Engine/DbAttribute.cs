using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpgsqlTypes;

namespace MembershipManager.Engine
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class DbTableName(string tableName) : Attribute
    {
        public string Name { get; private set; } = tableName;
    }

    [AttributeUsage(AttributeTargets.Property)]
    internal class DbNameable(string attributeName) : Attribute
    {
        public string Name { get; private set; } = attributeName;
    }

    [AttributeUsage(AttributeTargets.Property)]
    internal class DbConstraint  : Attribute {}
    internal class DbAttribute (string attributeName) : DbNameable (attributeName) {}
    internal class DbRelation (string attributeName, string relAttribteName) : DbNameable(attributeName) {
        public string RelAttributeName { get;} = relAttribteName;
    }
    internal class DbPrimaryKey(NpgsqlDbType pkType, int size = 0) : DbConstraint
    {
        public NpgsqlDbType PkType { get; private set; } = pkType;
        public int Size { get; private set; } = size;
    }
}
