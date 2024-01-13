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
using System.ComponentModel;
using MembershipManager.View.Utils.ListSelectionForm;


namespace MembershipManager.DataModel.People
{
    [DbTableName("person")]
    public class Person : ISql, INotifyPropertyChanged
    {
        [Filtered("Numéro AVS")]
        [Displayed("Numéro AVS")]
        [DbPrimaryKey(NpgsqlDbType.Char, 13)]
        [DbAttribute("no_avs")]
        public string? NoAvs { get; set; }

        [Sorted]
        [Filtered("Prénom")]
        [Displayed("Prénom")]
        [DbAttribute("first_name")]
        public string? FirstName { get; set; }

        [Filtered("Nom")]
        [Displayed("Nom")]
        [DbAttribute("last_name")]
        public string? LastName { get; set; }

        [Filtered("Adresse")]
        [Displayed("Adresse")]
        [DbAttribute("address")]
        public string? Address { get; set; }

        [Displayed("Ville")]
        [DbRelation("city_id", "id")]
        public City? City { get; set; }

        [Displayed("Téléphone")]
        [DbAttribute("phone")]
        public string? Phone { get; set; }

        [Displayed("Mobile")]
        [DbAttribute("mobile")]
        public string? Mobile { get; set; }

        [Displayed("Email")]
        [DbAttribute("email")]
        public string? Email { get; set; }

        [Filtered("Tous", true)]
        public string? FullName { get => ToString(); }

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

        public event PropertyChangedEventHandler? PropertyChanged;
      

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
        if (Validate()) DbManager.Db?.Send(ISql.UpdateQuery<Person>(this));
        }

        public void Insert()
        {
            if (Validate()) DbManager.Db?.Send(ISql.InsertQuery<Person>(this));
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
