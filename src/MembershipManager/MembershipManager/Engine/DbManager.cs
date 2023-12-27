using Npgsql;
using System.Configuration;
using System.Reflection;


namespace MembershipManager.Engine
{
    public class DbManager
    {
        #region Singleton access
        public static DbManager? Db
        {
            get
            {
                _instance ??= new DbManager();
                return _instance;
            }
        }

        private DbManager()
        {

        }

        private static DbManager? _instance;
        #endregion

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
            cmd.Connection = new NpgsqlConnection(GetConnectionString()); ;

            CheckDbValidity(cmd);

            Type type = typeof(T);
            OpenConnection(cmd.Connection);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            List<T> results = [];

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    object? obj = Convert(reader, type) ?? throw new ArgumentNullException($"Command call throw exception {cmd.CommandText}");
                    results.Add((T)obj);
                }
            }
            CloseConnection(cmd.Connection);

            return results;
        }

        private static object? Convert(NpgsqlDataReader reader, Type type)
        {
            object? newObject = Activator.CreateInstance(type);

            if (newObject is null) return null;

            foreach (var property in type.GetProperties())
            {
                var attributes = property.GetCustomAttributes<Attribute>();
                if (attributes.FirstOrDefault() is DbAttribute att)
                {
                    var valueRead = reader[att.Name];
                    if (valueRead.GetType() != typeof(DBNull))
                        property.SetValue(newObject, valueRead);
                }
                else if (attributes.FirstOrDefault() is DbRelation rel)
                { 
                    var foreignKey = reader[rel.Name];
                    Type relationType = property.PropertyType;
                    object[] args = { foreignKey };
                    ISql? newRelation = (ISql?)Activator.CreateInstance(relationType, args);
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
    }
}
