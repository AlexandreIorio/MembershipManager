using MembershipManager.Engine;

using System.Diagnostics.Metrics;
using Npgsql;
using NpgsqlTypes;
using System.Reflection;
using System.Text;
using System.Reflection.Metadata.Ecma335;
using System.IO.Packaging;
using System.Dynamic;
using System.Collections.Generic;
using System.Windows;


namespace MembershipManager.DataModel.People
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

        [DbRelation("city_id", "id")]
        public City? City { get; set; }

        [DbAttribute("phone")]
        public string? Phone { get; set; }

        [DbAttribute("mobile")]
        public string? Mobile { get; set; }

        [DbAttribute("email")]
        public string? Email { get; set; }

        public Person() { }
        public Person(Person person)
        {
            this.NoAvs = person.NoAvs;
            this.FirstName = person.FirstName;
            this.LastName = person.LastName;
            this.Address = person.Address;
            this.City = person.City;
            this.Phone = person.Phone;
            this.Mobile = person.Mobile;
            this.Email = person.Email;
        }

        public void Insert()
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = $"INSERT INTO person {ISql.InsertQuery(typeof(Person))}";
            ISql.ComputeCommandeWithValues(cmd, this);
            DbManager.Db?.Send(cmd);
        }

        public static ISql? Select(params object[] pk)
        {

            if (pk.Length != 1) throw new ArgumentException();
            Person? p = ISql.Get<Person>(pk[0]);
            if (p == null) throw new KeyNotFoundException();

            return p;

        }

        public bool Validate()
        {
            StringBuilder message = new StringBuilder();
            bool valid = true;
            if (string.IsNullOrEmpty(NoAvs))
            {
                message.AppendLine("Le numéro AVS est obligatoire");
                valid = false;
            }
            if (string.IsNullOrEmpty(FirstName))
            {
                message.AppendLine("Le prénom est obligatoire");
                valid = false;
            }
            if (string.IsNullOrEmpty(LastName))
            {
                message.AppendLine("Le nom est obligatoire");
                valid = false;
            }
            if (!valid)
            {
                MessageBox.Show(message.ToString(),
                    "Erreur",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);

            }
            return valid;
        }

        public void Update()
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = $"UPDATE person SET {ISql.InsertQuery(typeof(Person))} WHERE 'no_avs' = @where;";
            ISql.ComputeCommandeWithValues(cmd, this, true);
            NpgsqlParameter param = new NpgsqlParameter($"@where", NoAvs);
            cmd.Parameters.Add(param);
            DbManager.Db?.Send(cmd);
        }
    }
}
