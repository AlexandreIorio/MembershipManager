using MembershipManager.Engine;
using MembershipManager.DataModel.Company;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace MembershipManager.DataModel.Person
{
    [DbTableName("member")]
    public class Member : Person
    {
        [DbRelation("structure_name", "name")]
        public Structure Structure { get; set; }

        [DbAttribute("subscription_date")]
        public DateTime SubscriptionDate { get; set; }

        public Member() : base()
        {
            this.SubscriptionDate = DateTime.Now;
        }



        public new void Insert()
        {
            Person p = new Person();
            p.NoAvs = this.NoAvs;
            p.FirstName = this.FirstName;
            p.LastName = this.LastName;
            p.Address = this.Address;
            p.City = this.City;
            p.Phone = this.Phone;
            p.MobilePhone = this.MobilePhone;
            p.Email = this.Email;
            p.Insert();
            NpgsqlCommand cmdMember = new NpgsqlCommand();
            cmdMember.CommandText = $"INSERT INTO member {ISql.ComputeQuery(this.GetType())}";
            ISql.ComputeCommandeWithValues(cmdMember, this);
            DbManager.Db?.Send(cmdMember);
        }

        public new void Select(params object[] pk)
        {

            if (pk.Length != 1) throw new ArgumentException();
            Member? m = ISql.Get<Member>(pk[0]);
            if (m == null) throw new KeyNotFoundException();

            base.Select(pk[0]);
            //Member class
            this.Structure = m.Structure;
            this.SubscriptionDate = m.SubscriptionDate;
        }

    }
}
