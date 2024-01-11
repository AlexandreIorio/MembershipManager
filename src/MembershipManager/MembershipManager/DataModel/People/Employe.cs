using MembershipManager.DataModel.Company;
using MembershipManager.Engine;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MembershipManager.DataModel.People
{
    public class Employe : Person, ISql
    {

        [DbNameable("salary")]
        public int Salary { get; set; }

        [DbNameable("rate")]
        public int Rate { get; set; }

        [DbRelation("franchise_id", "id")]
        Franchise? Franchise { get; set; }

        public new void Insert()
        {
            Person p = new Person(this);
            p.Insert();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = $"INSERT INTO person {ISql.ComputeQuery(typeof(Employe))}";
            ISql.ComputeCommandeWithValues(cmd, this);
            DbManager.Db?.Send(cmd);
        }

        public new static ISql? Select(params object[] pk)
        {

            if (pk.Length != 1) throw new ArgumentException();
            Employe? p = ISql.Get<Employe>(pk[0]);
            if (p == null) throw new KeyNotFoundException();

            return p;

        }

    }
}
