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
            _conn = new NpgsqlConnection(GetConnectionString());
        }

        private static DbManager? _instance;
        #endregion

        #region Db interraction
        public void Send(NpgsqlCommand cmd)
        {
            cmd.Connection = _conn;

            CheckDbValidity(cmd);

            Db?.OpenConnection();
            cmd.ExecuteNonQuery();
            Db?.CloseConnection();
        }

        public List<T> Receive<T>(NpgsqlCommand cmd) where T : class
        {
            cmd.Connection = _conn;

            CheckDbValidity(cmd);

            Type type = typeof(T);
            Db?.OpenConnection();
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
            Db?.CloseConnection();

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
                else if (attributes.FirstOrDefault() is DbRelation)
                {
                    Type relationType = property.PropertyType;
                    object? newRelation = Convert(reader, relationType);
                    property.SetValue(newObject, newRelation);
                }
            }

            return newObject;
        }

        private static void CheckDbValidity(NpgsqlCommand cmd)
        {
            if (cmd.Connection is null) throw new ArgumentNullException("Connection is null");
            if (Db is null) throw new ArgumentNullException("Db is null");
            if (cmd.Connection != Db._conn) throw new ArgumentException("Connection is not the same");
        }
        #endregion

        #region Db connection management
        private readonly NpgsqlConnection _conn;

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

        private void OpenConnection()
        {
            _conn.Open();
            using var command = new NpgsqlCommand("SET search_path TO membershipmanager", _conn);
            command.ExecuteNonQuery();
        }

        private void CloseConnection()
        {
            _conn.Close();
        }
        #endregion
    }
}
