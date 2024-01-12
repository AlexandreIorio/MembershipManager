﻿using MembershipManager.DataModel.People;
using MembershipManager.Engine;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace MembershipManager.DataModel.Company
{
    [DbTableName("structure")]
    public class Structure : ISql
    {
        [DbPrimaryKey(NpgsqlDbType.Varchar, 50)]
        [DbAttribute("name")]
        public string? Name { get; set; }

        [DbAttribute("head_office_address")]
        public string? HeadOfficeAddress { get; set; }

        [DbRelation("city_id", "id")]
        public City? City { get; set; }

        public Structure() { }


        public void Insert()
        {
            throw new NotImplementedException();
        }

        public static ISql? Select(params object[] pk)
        {

            if (pk.Length != 1) throw new ArgumentException();
            Structure? s = ISql.Get<Structure>(pk[0]);
            if (s == null) throw new KeyNotFoundException();

            return s;

        }

        bool ISql.Validate()
        {
            StringBuilder message = new StringBuilder();
            bool valid = true;
            if (string.IsNullOrEmpty(Name))
            {
                message.AppendLine("Le nom de la structure est obligatoire");
                valid = false;
            }
            if (string.IsNullOrEmpty(HeadOfficeAddress))
            {
                message.AppendLine("L'adresse du siège est obligatoire");
                valid = false;
            }
            if (City == null)
            {
                message.AppendLine("La ville du siège est obligatoire");
                valid = false;
            }
            if (!valid)
            {
                MessageBox.Show(message.ToString(), "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return valid;
        }

        public void Update()
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = $"UPDATE structure SET {ISql.InsertQuery(typeof(Structure))} WHERE 'name' = @where";
            ISql.ComputeCommandeWithValues(cmd, this);
            NpgsqlParameter param = new NpgsqlParameter($"@where", Name);
            cmd.Parameters.Add(param);
            DbManager.Db?.Send(cmd);
        }
    }
}
