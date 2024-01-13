using MembershipManager.DataModel.People;
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
        public void Update();

        public abstract static ISql? Select(params object[] pk);

        public bool Validate();

        #endregion

        #region  Default Methods


        #endregion

        #region  Static Methods

        public static ISql? Select(Type type, object[] pk) 
        {
            // Recherche de la méthode statique
            MethodInfo? methodInfo = type.GetMethod("Select");

            // Appel de la méthode statique
            object? obj = methodInfo?.Invoke(null,new object[] { pk }) ;

            if (obj is not null && obj is ISql isql) return isql;

            return null;
        }

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

        public static List<T> GetAll<T>() where T : class
        {
            Type type = typeof(T);
            NpgsqlCommand cmd = new();

            DbTableName? tableNameAttribute = type.GetCustomAttribute<DbTableName>();
            if (tableNameAttribute == null) throw new MissingMemberException();

            cmd.CommandText = $"SELECT * FROM {tableNameAttribute.Name}";
    
            return DbManager.Db.Receive<T>(cmd);
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
        public static NpgsqlCommand InsertQuery<T>(object obj)
        {
            Type type = typeof(T);
            NpgsqlCommand cmd = new NpgsqlCommand();
            string tableName = type.GetCustomAttribute<DbTableName>()?.Name ?? throw new MissingMemberException();
            StringBuilder sbAtt = new($"INSERT INTO {tableName} (");
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
                    object? value = p.GetValue(obj);
                    if(value is null) continue;

                    //Add property name to query
                    sbAtt.Append(attributes.First().Name).Append(", ");
                    sbVal.Append($"@value{i}, ");

                    //Add property value to query
                    if (attributes.Any(a => a is DbAttribute))
                    {
                        DbAttribute att = (DbAttribute)attributes.First(a => a is DbAttribute);
                        
                        NpgsqlParameter param = new NpgsqlParameter($"@value{i}", value);
                        cmd.Parameters.Add(param);
                    }

                    else if (attributes.Any(a => a is DbRelation))
                    {
                        DbRelation rel = (DbRelation)attributes.First(a => a is DbRelation);
                        if (value is null) continue;
                        ISql? sql = (ISql?)value ?? throw new Exception("ISql is null");
                        NpgsqlParameter param = new NpgsqlParameter($"@value{i}", GetDbAttributeByName(sql, rel.Name));
                        cmd.Parameters.Add(param);
                    }

                    ++i;
                }
            }
            //remove last comma
            sbAtt.Remove(sbAtt.Length - 2, 2);
            sbVal.Remove(sbVal.Length - 2, 2);

            sbAtt.Append(")");
            sbVal.Append(")");
            cmd.CommandText = sbAtt.Append(" VALUES ").Append(sbVal).ToString();

            return cmd;
        }

        public static NpgsqlCommand UpdateQuery<T>(object obj)
        {
            Type type = typeof(T);
            NpgsqlCommand cmd = new NpgsqlCommand();
            string tableName = type.GetCustomAttribute<DbTableName>()?.Name ?? throw new MissingMemberException();
            StringBuilder sbAtt = new($"UPDATE {tableName} SET ");
            int i = 0;
          
            foreach (PropertyInfo p in type.GetProperties())
            {
                IEnumerable<DbConstraint> constraints = p.GetCustomAttributes<DbConstraint>();

                // Ignore properties if from base class and not primary key
                if (p.DeclaringType != type || constraints.Any(x => x is DbPrimaryKey))
                    continue;

                DbNameable? attribute = p.GetCustomAttribute<DbNameable>();

                if (attribute is not null)
                {
                    //Add property name to query
                    sbAtt.Append($"{attribute.Name} = @value{i}").Append(", ");
    

                    //Add property value to query
                    if (attribute is DbAttribute)
                    {
                        object? value = p.GetValue(obj) ?? DBNull.Value;
                        NpgsqlParameter param = new NpgsqlParameter($"@value{i}", value);
                        cmd.Parameters.Add(param);
                    }

                    else if (attribute is DbRelation)
                    {
                        var value = p.GetValue(obj);
                        if (value is null) continue;
                        ISql? sql = (ISql?)value ?? throw new Exception("ISql is null");
                        object? val = GetDbAttributeByName(sql, attribute.Name) ?? DBNull.Value;
                        NpgsqlParameter param = new NpgsqlParameter($"@value{i}",val);
                        cmd.Parameters.Add(param);
                    }

                    ++i;
                }
            }

            //remove last comma
            sbAtt.Remove(sbAtt.Length - 2, 2);

            sbAtt.Append(" WHERE ");
            List<string> primaryKeysName = GetPrimaryKeysName(type);
            List<object?> primaryKeysValue = GetPrimaryKeyValue(obj);
            for (int j = 0; j < primaryKeysName.Count(); j++ )
            {
                sbAtt.Append($"{primaryKeysName[j]} = @value{i}").Append(" AND ");
                NpgsqlParameter param = new NpgsqlParameter($"@value{i}", primaryKeysValue[j]);
                cmd.Parameters.Add(param);
                ++i;
            }

            //remove last AND
            sbAtt.Remove(sbAtt.Length - 5, 5);

            cmd.CommandText = sbAtt.ToString();
                        
            return cmd;
        }

        private static List<object?> GetPrimaryKeyValue(object obj)
        {
            Type type = obj.GetType();
            List<object?> pkValues = new ();
            foreach (PropertyInfo p in type.GetProperties())
            {
                DbPrimaryKey? attribute = p.GetCustomAttribute<DbPrimaryKey>();
                if (attribute is not null)  pkValues.Add(p.GetValue(obj));
            }
            return pkValues;
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
        public static List<string> GetPrimaryKeysName(Type type)
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
