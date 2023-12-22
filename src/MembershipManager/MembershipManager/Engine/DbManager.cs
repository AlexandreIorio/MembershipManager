using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using Npgsql;

using System.Windows;
using System.Reflection;


namespace MembershipManager.Engine
{


    public class DbManager
    {
        private static readonly DbManager? _instance = new();

        public static DbManager? Db
        {
            get { return _instance; }
        }

        private DbManager()
        {
            _conn = new NpgsqlConnection(getConnectionString());
        }

        private NpgsqlConnection _conn;

        private string getConnectionString()
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
            using (var command = new NpgsqlCommand("SET search_path TO membershipmanager", _conn))
            {
                command.ExecuteNonQuery();
            }
        }

        private void CloseConnection()
        {
            _conn.Close();
        }

        public void send(NpgsqlCommand cmd)
        {
            Db.OpenConnection();
            cmd.Connection = _conn;
            cmd.ExecuteNonQuery();
            Db.CloseConnection();
        }

        public List<T> recieve<T>(NpgsqlCommand cmd) where T : class
        {

            Type type = typeof(T);
            cmd.Connection = _conn;
            Db.OpenConnection();
            NpgsqlDataReader reader = cmd.ExecuteReader();
            
            List<T> results = new List<T>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    object? obj = converter(reader, type) ?? throw new ArgumentNullException($"Command call throw exception {cmd.CommandText}");
                    results.Add((T)obj);
                }
            }
            Db.CloseConnection();
            return results;

        }

        private object? converter(NpgsqlDataReader reader, Type type)
        {
            object? myObject = Activator.CreateInstance(type);

            if (myObject == null) return null;

            foreach (PropertyInfo p in type.GetProperties())
            {
                var Attributs = p.GetCustomAttributes<Attribute>();
                if (Attributs.Count() == 0) continue;
                if (Attributs.First() is DbAttribute att)
                {
                    var theValue = reader[att.Name];
                    if (theValue.GetType() != typeof(DBNull))
                    {
                        p.SetValue(myObject, theValue);
                    }
                }

                else if (Attributs.First() is DbRelation rel)
                {
                    Type relationType = p.PropertyType;
                    object? myRelation = Activator.CreateInstance(relationType);
                    myRelation = converter(reader, relationType);
                    p.SetValue(myObject, myRelation);
                }
            }
            return myObject;

        }


    }
}
