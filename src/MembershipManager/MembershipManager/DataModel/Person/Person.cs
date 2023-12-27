using MembershipManager.Engine;
using MembershipManager.DataModel;
using System.Diagnostics.Metrics;
using Npgsql;
using NpgsqlTypes;
using System.Reflection;
using System.Text;
using System.Reflection.Metadata.Ecma335;
using System.IO.Packaging;
using System.Dynamic;
using System.Collections.Generic;


namespace MembershipManager.DataModel.Person
{
    [DbTableName("person")]
    public class Person : ISql
    {
        [DbPrimaryKey(NpgsqlDbType.Char, 13)]
        [DbAttribute("no_avs")]
        public string? NoAvs { get; set; }

        [DbAttribute("first_name")]
        public string? FirstName { get; set; }

        [DbAttribute("last_name")]
        public string? LastName { get; set; }

        [DbAttribute("address")]
        public string? Address { get; set; }

        [DbRelation("city_id")]
        public City? City { get; set; }

        [DbAttribute("phone")]
        public string? Phone { get; set; }

        [DbAttribute("mobile")]
        public string? MobilePhone { get; set;}

        [DbAttribute("email")]
        public string? Email { get; set; }

        public Person() { }

        public Person(string noAvs)
        {
            Person? p = (Person?)Get(noAvs);
            if (p == null) throw new KeyNotFoundException();
            NoAvs = p.NoAvs;
            FirstName = p.FirstName;
            LastName = p.LastName;
            Address = p.Address;
            City = p.City;
        }

        public void Insert()
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = $"INSERT INTO person {ISql.ComputeQuery(typeof(Person))}";
            ISql.ComputeCommandeWithValues(cmd, this);
            DbManager.Db?.Send(cmd);
        }
                
        public ISql? Get(params object[] pk)
        {
            NpgsqlCommand cmd = new();

            DbTableName? tableNameAttribute = GetType().GetCustomAttribute<DbTableName>();
            if (tableNameAttribute == null) throw new MissingMemberException();

            cmd.CommandText = $"SELECT * FROM {tableNameAttribute.Name} WHERE {ISql.ComputeWhereClause(GetType())}";

            for (int i = 0; i < pk.Length; i++)
            {
                NpgsqlParameter param = new NpgsqlParameter($"@value{i + 1}", NpgsqlDbType.Char, 13) { Value = pk[i] };
                cmd.Parameters.Add(param);
            }

            return DbManager.Db?.Receive<Person>(cmd).FirstOrDefault();
        }

    }
}
