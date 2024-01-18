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

        [DbAttribute("salary")]
        public int Salary { get; set; }

        [DbAttribute("rate")]
        public int Rate { get; set; }

        [DbRelation("franchise_id")]
        Franchise? Franchise { get; set; }

        public new static ISql? Select(params object[] pk)
        {

            if (pk.Length != 1) throw new ArgumentException();
            Employe? p = ISql.Get<Employe>(pk[0]);
            if (p == null) throw new KeyNotFoundException();

            return p;

        }

    }
}
