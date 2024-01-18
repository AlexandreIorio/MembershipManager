using MembershipManager.Engine;
using NpgsqlTypes;
using System.Text;
using System.Windows;

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

        [DbRelation("city_id")]
        public City? City { get; set; }

        public Structure() { }


        public void Insert()
        {

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
            //NpgsqlCommand cmd = new NpgsqlCommand();
            //cmd.CommandText = $"UPDATE structure SET {ISql.InsertQuery(typeof(Structure))} WHERE 'name' = @where";
            //ISql.ComputeCommandeWithValues(cmd, this);
            //NpgsqlParameter param = new NpgsqlParameter($"@where", Name);
            //cmd.Parameters.Add(param);
            //DbManager.Db?.Send(cmd);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Structure s)
            {
                return s.Name == Name;
            }
            return false;
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
