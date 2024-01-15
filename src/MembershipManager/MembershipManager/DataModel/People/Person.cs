﻿using MembershipManager.Engine;

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
    public class Person : ISql, INotifyPropertyChanged, Ilistable
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
            if (City is null)
            {
                message.AppendLine("La ville est obligatoire");
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

        public static List<SqlViewable> Views
        {
            get
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.CommandText = @"SELECT first_name, last_name, no_avs, address, city.city_name as city_name, canton.canton_name as canton_name
                                FROM person
                                    INNER JOIN city ON city_id = city.id
                                    INNER JOIN canton ON canton_abbreviation = canton.abbreviation;";

                return DbManager.Db.Views<PersonView>(cmd).Cast<SqlViewable>().ToList();
            }
        }

    }
}