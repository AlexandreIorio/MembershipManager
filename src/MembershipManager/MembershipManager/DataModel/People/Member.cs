using MembershipManager.Engine;
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

namespace MembershipManager.DataModel.People
{
    [DbTableName("member")]
    [DbInherit(typeof(Person))]
    public class Member : Person
    {
        [DbRelation("structure_name", "name")]
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


    }
}
