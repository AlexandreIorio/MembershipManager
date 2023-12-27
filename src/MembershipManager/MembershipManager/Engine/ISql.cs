using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MembershipManager.Engine
{
    public interface ISql
    {
        #region Abstract Methods
        public ISql? Get(object pk);
        public void Insert();
        public object? GetPrimaryKey() {
            foreach (PropertyInfo p in this.GetType().GetProperties())
            {
                IEnumerable<DbPrimaryKey> attributes = p.GetCustomAttributes<DbPrimaryKey>();
                if (attributes.Count() > 0)
                {
                    return p.GetValue(this);
                }
            }
            return null;
        }
        #endregion

        #region  Static Methods
        public static List<Type> InheritedTypes
        {
            get
            {
                List<Type> inheritedTypes = new();
                MethodBase? currentClass = MethodBase.GetCurrentMethod() ?? throw new Exception("MethodBase.GetCurrentMethod() is null");

                Type? currentType = currentClass.DeclaringType;

                while (currentType != null)
                {
                    inheritedTypes.Add(currentType);
                    currentType = currentType.BaseType;
                }

                return inheritedTypes;
            }
        }
        public static string ComputeQuery(Type type)
        {
            StringBuilder sbAtt = new("(");
            StringBuilder sbVal = new("(");
            int i = 0;
            foreach (PropertyInfo p in type.GetProperties())
            {

                IEnumerable<DbNameable> attributes = p.GetCustomAttributes<DbNameable>();

                if (attributes.Count() > 0)
                {
                    sbAtt.Append(attributes.First().Name).Append(", ");
                    sbVal.Append($"@value{i++}, ");
                }
            }
            //remove last comma
            sbAtt.Remove(sbAtt.Length - 2, 2);
            sbVal.Remove(sbVal.Length - 2, 2);


            sbAtt.Append(")");
            sbVal.Append(");");

            return sbAtt.ToString() + " VALUES " + sbVal.ToString();
        }
        public static void ComputeCommandeWithValues(NpgsqlCommand cmd, object obj)
        {
            int i = 0;
            foreach (PropertyInfo p in obj.GetType().GetProperties())
            {
                // Ignore properties if from base class
                if (p.DeclaringType != obj.GetType())
                    continue;
                IEnumerable<DbNameable> attributes = p.GetCustomAttributes<DbNameable>();
                DbNameable? attribute = attributes.FirstOrDefault();
                if (attribute is null) continue;

                else if (attribute is DbAttribute att)
                {
                    var value = p.GetValue(obj);
                    NpgsqlParameter param = new NpgsqlParameter($"@value{i++}", value);
                    cmd.Parameters.Add(param);
                }

                else if (attribute is DbRelation rel)
                {
                    var value = p.GetValue(obj);
                    if (value is null) continue;
                    ISql? sql = (ISql?)value;
                    NpgsqlParameter param = new NpgsqlParameter($"@value{i++}", sql?.GetPrimaryKey());
                    cmd.Parameters.Add(param);
                }
            }
        }
        #endregion
    }


}
