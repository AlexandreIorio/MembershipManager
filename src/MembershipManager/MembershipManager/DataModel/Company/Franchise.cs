using MembershipManager.DataModel.People;
using MembershipManager.Engine;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MembershipManager.DataModel.Company
{
    [DbTableName("franchise")]
    public class Franchise : ISql
    {
        [DbPrimaryKey(NpgsqlDbType.Integer)]
        [DbAttribute("id")]
        public int Id { get; set; }

        [DbAttribute("strucutre_name")]
        public string? StructureName { get; set; }

        [DbAttribute("address")]
        public string? Address { get; set; }

        [DbRelation("city_id")]
        City? City { get; set; }

        public Franchise() { }

        public static ISql? Select(params object[] pk)
        {

            if (pk.Length != 1) throw new ArgumentException();
            Franchise? f = ISql.Get<Franchise>(pk[0]);
            if (f == null) throw new KeyNotFoundException();

            return f;

        }

        bool ISql.Validate()
        {
            StringBuilder message = new StringBuilder();
            bool valid = true;
            if (string.IsNullOrEmpty(StructureName))
            {
                message.AppendLine("Le nom de la structure est obligatoire");
                valid = false;
            }
            if (string.IsNullOrEmpty(Address))
            {
                message.AppendLine("L'adresse est obligatoire");
                valid = false;
            }
            if (City == null)
            {
                message.AppendLine("La ville est obligatoire");
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

            //NpgsqlCommand cmd = new NpgsqlCommand();
            //cmd.CommandText = $"UPDATE franchise SET {ISql.InsertQuery(typeof(Franchise))} WHERE 'id' = @where";
            //ISql.ComputeCommandeWithValues(cmd, this);
            //NpgsqlParameter param = new NpgsqlParameter($"@where", Id);
            //cmd.Parameters.Add(param);
            //DbManager.Db?.Send(cmd);

        }

        public void Insert()
        {
            throw new NotImplementedException();
        }
    }
}
