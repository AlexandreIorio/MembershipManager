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

        public Member(string noAvs) : base(noAvs)
        {
            Member? m = ISql.Get<Member>(noAvs);
            if (m == null) throw new KeyNotFoundException();
            this.Structure = m.Structure;
            this.SubscriptionDate = m.SubscriptionDate;
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

    }
}
