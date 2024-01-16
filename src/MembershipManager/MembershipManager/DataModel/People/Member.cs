﻿using MembershipManager.Engine;
using MembershipManager.DataModel.Company;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using MembershipManager.View.Utils.ListSelectionForm;

namespace MembershipManager.DataModel.People
{
    /// <summary>
    /// This class represents a member
    /// </summary>

    [DbTableName("member")]
    [DbInherit(typeof(Person))]
    public class Member : Person
    {
        [DbRelation("structure_name")]
        public Structure? Structure { get; set; }

        [DbAttribute("subscription_date")]
        public DateTime SubscriptionDate { get; set; }

        public Member() : base()
        {
            this.SubscriptionDate = DateTime.Now;
        }

        public Member(Person person) : base(person)
        {
            this.SubscriptionDate = DateTime.Now;
        }

        public new event PropertyChangedEventHandler? PropertyChanged;


        public static new ISql? Select(params object[] pk)
        {
            if (pk.Length != 1) throw new ArgumentException();
            Member? m = ISql.Get<Member>(pk[0]);
            if (m == null) throw new KeyNotFoundException();
            return m;
        }
        public new void Insert()
        {
            base.Insert();
            if (Validate()) DbManager.Db?.Send(ISql.InsertQuery<Member>(this));
        }

        public new void Update()
        {
            base.Update();
            if (Validate()) DbManager.Db?.Send(ISql.UpdateQuery<Member>(this));
        }

        public new bool Validate()
        {
            StringBuilder sb = new StringBuilder();
            bool valid = true;
            if (!base.Validate()) valid = false;
            if (Structure == null)
            {
                sb.AppendLine("La structure est obligatoire");
                valid = false;
            }
            if (valid == false)
            {
                MessageBox.Show(sb.ToString(),
                    "Erreur",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);

            }
            return valid;

        }

        public override string ToString()
        {
            return base.ToString();
        }

        public new static List<SqlViewable>? Views(params NpgsqlParameter[] sqlParam)
        {
           
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.CommandText = @"SELECT member.no_avs, first_name, last_name, address, subscription_date, structure.name as structure_name, city.name as city_name, canton.name as canton_name
                                    FROM member
                                        INNER JOIN structure ON structure_name = structure.name
                                        INNER JOIN person ON member.no_avs = person.no_avs 
                                        INNER JOIN city ON person.city_id = city.id
                                        INNER JOIN canton ON canton_abbreviation = canton.abbreviation;";

                return DbManager.Db.Views<MemberView>(cmd).Cast<SqlViewable>().ToList();
            
        }

    }
}
