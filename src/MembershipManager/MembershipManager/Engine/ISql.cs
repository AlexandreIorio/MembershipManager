using MembershipManager.DataModel.Person;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace MembershipManager.Engine
{
    public interface ISql
    {
        #region Abstract Methods
        public void Insert();
        public ISql Select(params object[] pk)

        #endregion

        #region  Default Methods



        #endregion

        #region  Static Methods

        protected static T? Get<T>(params object[] pk) where T : class
        {
            Type type = typeof(T);
            NpgsqlCommand cmd = new();

            DbTableName? tableNameAttribute = type.GetCustomAttribute<DbTableName>();
            if (tableNameAttribute == null) throw new MissingMemberException();

            cmd.CommandText = $"SELECT * FROM {tableNameAttribute.Name} {ComputeWhereClause(type)}";
            int i = 0;
            foreach (PropertyInfo p in type.GetProperties())
            {
                DbPrimaryKey? dbPrimaryKey = p.GetCustomAttribute<DbPrimaryKey>();
                if (i == pk.Length) break;
                if (dbPrimaryKey is null) continue;
                NpgsqlParameter param = new NpgsqlParameter($"@value{i}",dbPrimaryKey.PkType, dbPrimaryKey.Size) { Value = pk[i] };
                cmd.Parameters.Add(param);
                i++;
            }
            return DbManager.Db?.Receive<T>(cmd).FirstOrDefault();
        }

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

                IEnumerable<DbConstraint> constraints = p.GetCustomAttributes<DbConstraint>();
                // Ignore properties if from base class and not primary key
                if (p.DeclaringType != type && !constraints.Any(x => x is DbPrimaryKey))
                    continue;

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
                IEnumerable<DbConstraint> constraints = p.GetCustomAttributes<DbConstraint>();
                // Ignore properties if from base class and not primary key
                if (p.DeclaringType != obj.GetType() && !constraints.Any(x => x is DbPrimaryKey))
                    continue;

                IEnumerable<DbNameable> attributes = p.GetCustomAttributes<DbNameable>();

                if (attributes.Count() < 0) continue;

                if (attributes.Any(a => a is DbAttribute))
                {
                    DbAttribute att = (DbAttribute)attributes.First(a => a is DbAttribute);
                    var value = p.GetValue(obj);
                    NpgsqlParameter param = new NpgsqlParameter($"@value{i++}", value);
                    cmd.Parameters.Add(param);
                }

                else if (attributes.Any(a => a is DbRelation))
                {
                    DbRelation rel = (DbRelation)attributes.First(a => a is DbRelation);
                    var value = p.GetValue(obj);
                    if (value is null) continue;
                    ISql? sql = (ISql?)value ?? throw new Exception("ISql is null");
                    NpgsqlParameter param = new NpgsqlParameter($"@value{i++}", GetDbAttributeByName(sql, rel.Name));
                    cmd.Parameters.Add(param);
                }
            }
        }

        private static object? GetDbAttributeByName(object objstring, string attributeName)
        {
            foreach (PropertyInfo p in objstring.GetType().GetProperties())
            {
                DbNameable? attribute = p.GetCustomAttribute<DbNameable>();
                if (attribute is not null && attribute.Name.Equals(attributeName)) continue;
                return p.GetValue(objstring);
            }
            return null;
        }

        private static string ComputeWhereClause(Type type)
        {
            List<string> dbPrimaryKeys = GetPrimaryKeysName(type);
            StringBuilder sb = new StringBuilder("WHERE ");

            for (int i = 0; i < dbPrimaryKeys.Count; i++)
            {
                sb.Append($"{dbPrimaryKeys[i]} = @value{i}");
                if (i < dbPrimaryKeys.Count - 1) sb.Append(" AND ");
            }

            return sb.ToString();
        }
        private static List<string> GetPrimaryKeysName(Type type)
        {
            List<string> dbPrimaryKeys = new List<string>();

            foreach (PropertyInfo p in type.GetProperties())
            {
                DbPrimaryKey? pkAttribute = p.GetCustomAttribute<DbPrimaryKey>();
                if (pkAttribute is not null)
                {
                    DbNameable? nameAttribute = p.GetCustomAttribute<DbNameable>();
                    if (nameAttribute is not null) dbPrimaryKeys.Add(nameAttribute.Name);
                }
            }
            return dbPrimaryKeys;
        }

        #endregion
    }


}
