using Npgsql;
using System.Configuration;
using System.Reflection;
using System.Windows;


namespace MembershipManager.Engine
{
    /// <summary>
    /// This class is used to interact with the database
    /// </summary>
    public class DbManager
    {
        #region Db interraction

        /// <summary>
        /// This method is used to send a command to the database
        /// </summary>
        /// <param name="cmd">The command to send</param>

        public void Send(NpgsqlCommand cmd)
        {
            cmd.Connection = new NpgsqlConnection(GetConnectionString());
            CheckDbValidity(cmd);
            OpenConnection(cmd.Connection);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            CloseConnection(cmd.Connection);
        }

        public List<List<object>> InsertReturnigIds(NpgsqlCommand cmd)
        {
            cmd.Connection = new NpgsqlConnection(GetConnectionString());
            CheckDbValidity(cmd);
            OpenConnection(cmd.Connection);

            List<List<object>> results = [];
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    List<object> Ids = [];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Ids.Add(reader[i]);
                    }
                    results.Add(Ids);
                }
            }

            CloseConnection(cmd.Connection);
            return results;
        }

        /// <summary>
        /// This method converts a tuple into an object and uses the property names to retrieve the values in the tuple.
        /// A property can be ignored by adding the attribute IgnoreSql
        public List<T> Views<T>(NpgsqlCommand cmd) where T : class
        {
            cmd.Connection = new NpgsqlConnection(GetConnectionString());

            CheckDbValidity(cmd);

            Type type = typeof(T);
            OpenConnection(cmd.Connection);
            cmd.Prepare();
            NpgsqlDataReader reader = cmd.ExecuteReader();

            List<object> results = [];
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    object? obj = Activator.CreateInstance(type);
                    if (obj is null) throw new ArgumentNullException($"Impossible to create an instance of {type.FullName}");
                    foreach (PropertyInfo p in type.GetProperties())
                    {
                        if (p.GetCustomAttribute<IgnoreSql>() != null) continue;
                        object? valueRead = reader[p.Name];
                        if (valueRead is DBNull) valueRead = null;
                        p.SetValue(obj, valueRead);
                    }
                    results.Add(obj);
                }
            }
            return results.Cast<T>().ToList();
        }


        /// <summary>
        /// This method converts a tuple into an object using DbAttribute and DbRelation attributes to retrieve the values in the tuple.
        /// A property can be ignored whitout adding Attribute
        /// </summary>
        /// <typeparam name="T">The ISql Type </typeparam>
        /// <param name="cmd">The commande </param>
        /// <returns>List of ISql</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public List<T> Recieve<T>(NpgsqlCommand cmd) where T : class
        {
            cmd.Connection = new NpgsqlConnection(GetConnectionString());

            CheckDbValidity(cmd);

            Type type = typeof(T);
            OpenConnection(cmd.Connection);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            List<T> results = [];

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    object? obj = ConverTupleToObject(reader, type) ?? throw new ArgumentNullException($"Impossible to convert sql tuple in {type.FullName}");
                    results.Add((T)obj);
                }
            }
            CloseConnection(cmd.Connection);

            return results;
        }

        /// <summary>
        /// This method converts a tuple into an object using DbAttribute and DbRelation attributes to retrieve the values in the tuple.
        /// To call this method with inheritance, the type must have the DbInherit attribute and need a copy constructor with the parent type as parameter
        /// </summary>
        /// <param name="reader"> The reader to get sql tuple </param>
        /// <param name="type">The type to get from sql tuple </param>
        /// <returns></returns>

        private static object? ConverTupleToObject(NpgsqlDataReader reader, Type type)
        {

            DbInherit? inheritType = type.GetCustomAttributes<DbInherit>().FirstOrDefault();
            bool inherit = inheritType is not null;
            object? newObject;

            //Check if type inherit from another type
            if (inherit)
            {
                //Get primary keys of object
                List<object> pks = [];
                foreach (string pkName in ISql.GetPrimaryKeyNames(inheritType.InheritType))
                {
                    if (pkName is null) continue;
                    object? pk = reader[pkName];
                    if (pk is null) continue;
                    pks.Add(pk);
                }
                object[] args = pks.ToArray();
                //Get parent object
                object? parent = ISql.Select(inheritType.InheritType, args);

                //Create child with parent
                newObject = Activator.CreateInstance(type, parent);
            }
            else
            {
                //Create new object
                newObject = Activator.CreateInstance(type);
            }

            if (newObject is null) return null;

            foreach (PropertyInfo property in type.GetProperties())
            {
                // Ignore properties if from base class and inherit is true
                if (property.DeclaringType != type && inherit)
                    continue;


                IEnumerable<DbNameable> attributes = property.GetCustomAttributes<DbNameable>();

                //Get property value from sql tuple
                if (attributes.Any(a => a is DbAttribute))
                {
                    DbAttribute att = (DbAttribute)attributes.First(a => a is DbAttribute);
                    object valueRead = reader[att.Name];
                    if (valueRead.GetType() != typeof(DBNull))
                        property.SetValue(newObject, valueRead);
                }

                //Get relation value from sql tuple
                else if (attributes.Any(a => a is DbRelation))
                {
                    DbRelation rel = (DbRelation)attributes.First(a => a is DbRelation);
                    object foreignKey = reader[rel.Name];
                    Type relationType = property.PropertyType;
                    object[] args = { foreignKey };
                    ISql? newRelation = ISql.Select(relationType, args);
                    property.SetValue(newObject, newRelation);
                }
            }

            return newObject;
        }

        /// <summary>
        /// This method is used to check if the command is valid
        /// </summary>
        private static void CheckDbValidity(NpgsqlCommand cmd)
        {
            if (cmd.Connection is null) throw new ArgumentNullException("Connection is null");
            //TODO revoir les tests car plusieurs connexions peuvent être ouvertes en raison des relations

            //if (Db is null) throw new ArgumentNullException("Db is null");
            //if (cmd.Connection != Db._conn) throw new ArgumentException("Connection is not the same");
        }
        #endregion

        #region Db connection management

        /// <summary>
        /// This method is used to get the connection string from the app.config file
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString(bool useSchema = true)
        {

            System.Collections.Specialized.NameValueCollection AppSetting = ConfigurationManager.AppSettings;
            if (AppSetting.Count == 0)throw new ArgumentNullException("Application Setting is null");
            string? host = AppSetting["Host"];
            string? port = AppSetting["Port"];
            string? database = AppSetting["Database"];
            string? user = AppSetting["User"];
            string? password = AppSetting["Password"];

            //Connection string builder
            Npgsql.NpgsqlDataSourceBuilder builder = new();
            builder.ConnectionStringBuilder.Username = user;
            builder.ConnectionStringBuilder.Password = password;
            builder.ConnectionStringBuilder.Host = host;
            builder.ConnectionStringBuilder.Port = int.Parse(port);
            builder.ConnectionStringBuilder.Database = database;
            

            if (useSchema && bool.TryParse(ConfigurationManager.AppSettings["UseSchema"], out bool use)
              && use)
            {
                string? schema = ConfigurationManager.AppSettings["Schema"];
                if (schema is null) throw new ArgumentNullException("Schema is null");
                builder.ConnectionStringBuilder.SearchPath = schema;
            }

            return builder.ConnectionString;

        }

        /// <summary>
        /// This method is used to open a connection to the database
        /// </summary>
        /// <param name="connection"></param>
        private static void OpenConnection(NpgsqlConnection connection)
        {
            try
            {
                CloseConnection(connection);
                connection.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// This method is used to close a connection to the database
        /// </summary>
        private static void CloseConnection(NpgsqlConnection connection)
        {
            connection.Close();
        }

        public bool CheckSchema()
        {
            bool schemaExists = false;
            NpgsqlCommand cmd = new();
            cmd.Connection = new NpgsqlConnection(GetConnectionString(false));
            cmd.CommandText = @"SELECT EXISTS (
                                    SELECT 1
                                    FROM information_schema.Schemata
                                    WHERE schema_name = 'membershipmanager'
                                ) as schema_exists;";
            OpenConnection(cmd.Connection);
            schemaExists = cmd.ExecuteScalar() as bool? ?? false;
            CloseConnection(cmd.Connection);
            return schemaExists;


        }

        #endregion

        #region Singleton
        public static DbManager Db { get; private set; } = new DbManager();
        #endregion
    }
}
