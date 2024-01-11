using MembershipManager.DataModel.People;
using Npgsql;
using System.Configuration;
using System.Linq;
using System.Reflection;


namespace MembershipManager.Engine
{
    public class DbManager
    {
        #region Db interraction

        public void Send(NpgsqlCommand cmd)
        {
            cmd.Connection = new NpgsqlConnection(GetConnectionString());
            CheckDbValidity(cmd);
            OpenConnection(cmd.Connection);
            cmd.ExecuteNonQuery();
            CloseConnection(cmd.Connection);
        }

        public List<T> Receive<T>(NpgsqlCommand cmd) where T : class
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
                    object? obj = ConverTupleToObject(reader, type) ?? throw new ArgumentNullException($"Command call throw exception {cmd.CommandText}");
                    results.Add((T)obj);
                }
            }
            CloseConnection(cmd.Connection);

            return results;
        }

        private static object? ConverTupleToObject(NpgsqlDataReader reader, Type type)
        {

            DbInherit? inheritType = type.GetCustomAttributes<DbInherit>().FirstOrDefault();
            object? newObject;
            if (inheritType is not null)
            {
                //Get primary keys
                List<string> pks = new(); 
                foreach (string pkName in ISql.GetPrimaryKeysName(inheritType.InheritType))
                {
                    if (pkName is null) continue;
                    string? pk = reader[pkName].ToString();
                    if (pk is null) continue;
                    pks.Add(pk);
                }
                object[] args = pks.ToArray();
                //Get parent object
                object? parent = ISql.Select(inheritType.InheritType, args);

                //Change type to parent type
                newObject = Activator.CreateInstance(type,parent);
            }
            else
            {
                //Create new object
                newObject = Activator.CreateInstance(type);
            }

            if (newObject is null) return null;



            foreach (var property in type.GetProperties())
            {
                // Ignore properties if from base class
                if (property.DeclaringType != type)
                    continue;

                IEnumerable<Attribute> attributes = property.GetCustomAttributes<Attribute>();
                if (attributes.Any(a => a is DbAttribute))
                {
                    DbAttribute att = (DbAttribute)attributes.First(a => a is DbAttribute);
                    var valueRead = reader[att.Name];
                    if (valueRead.GetType() != typeof(DBNull))
                        property.SetValue(newObject, valueRead);
                }
                else if (attributes.Any(a => a is DbRelation))
                {
                    DbRelation rel = (DbRelation)attributes.First(a => a is DbRelation);
                    var foreignKey = reader[rel.Name];
                    Type relationType = property.PropertyType;
                    object[] args = { foreignKey };
                    ISql? newRelation = ISql.Select(relationType, args);
                    property.SetValue(newObject, newRelation);
                }
            }

            return newObject;
        }

        private static void CheckDbValidity(NpgsqlCommand cmd)
        {
            if (cmd.Connection is null) throw new ArgumentNullException("Connection is null");
            //TODO revoir les tests car plusieurs connexions peuvent être ouvertes en raison des relations

            //if (Db is null) throw new ArgumentNullException("Db is null");
            //if (cmd.Connection != Db._conn) throw new ArgumentException("Connection is not the same");
        }
        #endregion

        #region Db connection management

        private static string GetConnectionString()
        {

            var AppSetting = ConfigurationManager.AppSettings;
            string? host = AppSetting["Host"];
            string? port = AppSetting["Port"];
            string? database = AppSetting["Database"];
            string? user = AppSetting["User"];
            string? password = AppSetting["Password"];

            return $"User ID={user}; Password={password}; Host={host}; Port={port};  Database={database};";

        }

        private static void OpenConnection(NpgsqlConnection connection)
        {
            CloseConnection(connection);
            connection.Open();
            using var command = new NpgsqlCommand("SET search_path TO membershipmanager", connection);
            command.ExecuteNonQuery();
        }

        private static void CloseConnection(NpgsqlConnection connection)
        {
            connection.Close();
        }
        #endregion

        #region Singleton
        public static DbManager Db { get; private set; } = new DbManager();
        #endregion
    }
}
