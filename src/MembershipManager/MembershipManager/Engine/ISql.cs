using Npgsql;
using System.Reflection;
using System.Text;

namespace MembershipManager.Engine
{
    public interface ISql
    {
        #region Abstract Methods
        /// <summary>
        /// method to insert a new row in the database
        /// </summary>
        public void Insert();

        /// <summary>
        /// method to update a row in the database
        /// </summary> 
        public void Update();

        /// <summary>
        /// method to delete a row in the database
        /// </summary>
        public abstract static void Delete(params object[] pk);

        /// <summary>
        /// method to select a row in the database
        /// </summary> 
        /// <param name="pk"/> is the primary keys of the row to select</param>
        public abstract static ISql? Select(params object[] pk);

        /// <summary>
        /// This method validate the object before inserting or updating it in the database
        /// </summary>
        public bool Validate();
        #endregion


        #region  Static Methods
        /// <summary>
        /// This method returns the object corresponding to the primary keys
        /// </summary>
        /// <param name="type">The type of ISql object to get</param>
        /// <param name="pk">The primary keys of the object to get </param>
        /// <returns>The Isql selected object</returns>
        public static ISql? Select(Type type, object[] pk)
        {
            // Recherche de la méthode statique
            MethodInfo? methodInfo = type.GetMethod("Select");

            // Appel de la méthode statique
            object? obj = methodInfo?.Invoke(null, new object[] { pk });

            if (obj is not null && obj is ISql isql) return isql;

            return null;
        }

        public static void Delete(Type type, params object[] pk)
        {
            // Recherche de la méthode statique
            MethodInfo? methodInfo = type.GetMethod("Delete");

            // Appel de la méthode statique
            methodInfo?.Invoke(null, new object[] { pk });
        }

        /// <summary>
        /// this method returns the object corresponding to the primary keys
        /// </summary>
        /// <param name="pk">The primary keys of the object to get </param>"
        /// <returns>The Isql selected object</returns>
        public static T? Get<T>(params object[] pk) where T : class
        {
            //get the type of the object
            Type type = typeof(T);

            //get the table name of the object
            DbTableName? tableNameAttribute = type.GetCustomAttribute<DbTableName>();
            if (tableNameAttribute == null) throw new MissingMemberException();

            //create amd compute the command
            NpgsqlCommand cmd = new()
            {
                CommandText = $"SELECT * FROM {tableNameAttribute.Name} {ComputeWhereClause(type)}"
            };
            int i = 0;
            foreach (PropertyInfo p in type.GetProperties())
            {
                if (i == pk.Length) break;
                DbPrimaryKey? dbPrimaryKey = p.GetCustomAttribute<DbPrimaryKey>();
                if (dbPrimaryKey is null) continue;
                NpgsqlParameter param = new($"@value{i}", dbPrimaryKey.PkType, dbPrimaryKey.Size) { Value = pk[i] };
                cmd.Parameters.Add(param);
                i++;
            }
            //return the first element of the list
            return DbManager.Db?.Recieve<T>(cmd).FirstOrDefault();
        }

        public static void Erase<T>(params object[] pk)
        {
            //get the type of the object
            Type type = typeof(T);

            //get the table name of the object
            DbTableName? tableNameAttribute = type.GetCustomAttribute<DbTableName>();
            if (tableNameAttribute == null) throw new MissingMemberException();

            //create amd compute the command
            NpgsqlCommand cmd = new()
            {
                CommandText = $"DELETE FROM {tableNameAttribute.Name} {ComputeWhereClause(type)}"
            };
            int i = 0;
            foreach (PropertyInfo p in type.GetProperties())
            {
                if (i == pk.Length) break;
                DbPrimaryKey? dbPrimaryKey = p.GetCustomAttribute<DbPrimaryKey>();
                if (dbPrimaryKey is null) continue;
                NpgsqlParameter param = new($"@value{i}", dbPrimaryKey.PkType, dbPrimaryKey.Size) { Value = pk[i] };
                cmd.Parameters.Add(param);
                i++;
            }
            //return the first element of the list
            DbManager.Db?.Send(cmd);
        }

        /// <summary>
        /// This method returns all the objects of the type T
        /// </summary>
        /// <returns>List of T</returns>
        public static List<T> GetAll<T>() where T : class
        {
            //get the type of the object
            Type type = typeof(T);

            //get the table name of the object
            DbTableName? tableNameAttribute = type.GetCustomAttribute<DbTableName>();
            if (tableNameAttribute == null) throw new MissingMemberException();

            //create the command
            NpgsqlCommand cmd = new()
            {
                CommandText = $"SELECT * FROM {tableNameAttribute.Name}"
            };

            //return the whole list
            return DbManager.Db.Recieve<T>(cmd);
        }

        /// <summary>
        /// This method is used to create a query to insert an object in the database
        /// </summary> 
        /// <param name="obj">The object to insert</param>"
        public static NpgsqlCommand InsertQuery<T>(object obj)
        {
            //  get the type of the object
            Type type = typeof(T);

            bool inherit = type.GetCustomAttribute<DbInherit>() is not null;

            // Create the commandText
            string tableName = type.GetCustomAttribute<DbTableName>()?.Name ?? throw new MissingMemberException();
            StringBuilder sbAtt = new($"INSERT INTO {tableName} (");
            StringBuilder sbVal = new("(");
            int i = 0;

            NpgsqlCommand cmd = new();
            foreach (PropertyInfo p in type.GetProperties())
            {
                IEnumerable<DbConstraint> constraints = p.GetCustomAttributes<DbConstraint>();

                // Ignore properties if from base class if inherit and not primary key
                if (p.DeclaringType != type && inherit && !constraints.Any(x => x is DbPrimaryKey))
                    continue;

                IEnumerable<DbNameable> attributes = p.GetCustomAttributes<DbNameable>();

                if (attributes.Count() > 0)
                {
                    object? value = p.GetValue(obj);
                    if (value is null) continue;

                    //Add property name to query
                    sbAtt.Append(attributes.First().Name).Append(", ");
                    sbVal.Append($"@value{i}, ");

                    //Add property value to query
                    if (attributes.Any(a => a is DbAttribute))
                    {
                        DbAttribute att = (DbAttribute)attributes.First(a => a is DbAttribute);

                        NpgsqlParameter param = new($"@value{i}", value);
                        cmd.Parameters.Add(param);
                    }

                    else if (attributes.Any(a => a is DbRelation))
                    {
                        // get the relation attribute of the property
                        DbRelation rel = (DbRelation)attributes.First(a => a is DbRelation);
                        ISql? sqlObject = (ISql?)value ?? throw new Exception("ISql is null");
                        NpgsqlParameter param = new($"@value{i}", GetDbAttributeByName(sqlObject, rel.Name));
                        cmd.Parameters.Add(param);
                    }

                    ++i;
                }
            }
            //remove last comma
            sbAtt.Remove(sbAtt.Length - 2, 2);
            sbVal.Remove(sbVal.Length - 2, 2);

            // Add the last parenthesis
            sbAtt.Append(")");
            sbVal.Append(")");

            // Add the commandText to the command
            cmd.CommandText = sbAtt.Append(" VALUES ").Append(sbVal).ToString();

            return cmd;
        }

        /// <summary>
        /// This method is used to create a query to update an object in the database
        /// </summary>
        /// <param name="obj">The object to update</param>"
        public static NpgsqlCommand UpdateQuery<T>(object obj)
        {
            //  get the type of the object
            Type type = typeof(T);

            bool inherit = type.GetCustomAttribute<DbInherit>() is not null;

            // Create the commandText
            string tableName = type.GetCustomAttribute<DbTableName>()?.Name ?? throw new MissingMemberException();
            StringBuilder sbAtt = new($"UPDATE {tableName} SET ");
            int i = 0;

            NpgsqlCommand cmd = new();
            foreach (PropertyInfo p in type.GetProperties())
            {
                IEnumerable<DbConstraint> constraints = p.GetCustomAttributes<DbConstraint>();

                // Ignore properties if from base class and not primary key
                if ((p.DeclaringType != type && inherit) || constraints.Any(x => x is DbPrimaryKey))
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
                        NpgsqlParameter param = new($"@value{i}", value);
                        cmd.Parameters.Add(param);
                    }

                    else if (attribute is DbRelation)
                    {
                        // get the relation attribute of the property
                        object? value = p.GetValue(obj);
                        if (value is null) continue;
                        ISql? sql = (ISql?)value ?? throw new Exception("ISql is null");
                        object? val = GetDbAttributeByName(sql, attribute.Name) ?? DBNull.Value;
                        NpgsqlParameter param = new($"@value{i}", val);
                        cmd.Parameters.Add(param);
                    }

                    ++i;
                }
            }

            //remove last comma
            sbAtt.Remove(sbAtt.Length - 2, 2);

            sbAtt.Append(" WHERE ");
            List<string> primaryKeysName = GetPrimaryKeyNames(type);
            List<object?> primaryKeysValue = GetPrimaryKeyValues(obj);
            for (int j = 0; j < primaryKeysName.Count(); j++)
            {
                sbAtt.Append($"{primaryKeysName[j]} = @value{i}").Append(" AND ");
                NpgsqlParameter param = new($"@value{i}", primaryKeysValue[j]);
                cmd.Parameters.Add(param);
                ++i;
            }

            //remove last AND
            sbAtt.Remove(sbAtt.Length - 5, 5);

            cmd.CommandText = sbAtt.ToString();

            return cmd;
        }

        /// <summary>
        /// This methode is used to get the primary keys value of an object
        /// </summary>
        /// <param name="obj">The object to get the primary keys value</param>
        /// <returns>A list of primary keys value</returns>
        private static List<object?> GetPrimaryKeyValues(object obj)
        {
            Type type = obj.GetType();
            List<object?> pkValues = [];
            foreach (PropertyInfo p in type.GetProperties())
            {
                DbPrimaryKey? attribute = p.GetCustomAttribute<DbPrimaryKey>();
                if (attribute is not null) pkValues.Add(p.GetValue(obj));
            }
            return pkValues;
        }

        /// <summary>
        /// This method is used to get the primary keys name of a type
        /// </summary>
        /// <param name="type"> The type to get primary key names</param>
        /// <returns>A list of primary key names</returns>
        public static List<string> GetPrimaryKeyNames(Type type)
        {
            List<string> dbPrimaryKeys = [];

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

        /// <summary>
        /// Get the value of a property of an object using the DbAttribute or DbRelation attribute
        /// </summary>  
        /// <param name="obj">The object to get the value</param>
        /// <param name="attributeName">The name of the attribute to get the value of property</param>
        private static object? GetDbAttributeByName(object obj, string attributeName)
        {
            foreach (PropertyInfo p in obj.GetType().GetProperties())
            {
                DbNameable? attribute = p.GetCustomAttribute<DbNameable>();
                if (attribute is not null && attribute.Name.Equals(attributeName)) continue;
                return p.GetValue(obj);
            }
            return null;
        }

        /// <summary>
        /// This method is used to compute the where clause of a query
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string ComputeWhereClause(Type type)
        {
            List<string> dbPrimaryKeys = GetPrimaryKeyNames(type);
            StringBuilder sb = new("WHERE ");

            for (int i = 0; i < dbPrimaryKeys.Count; i++)
            {
                sb.Append($"{dbPrimaryKeys[i]} = @value{i}");
                if (i < dbPrimaryKeys.Count - 1) sb.Append(" AND ");
            }

            return sb.ToString();
        }

        #endregion
    }


}
