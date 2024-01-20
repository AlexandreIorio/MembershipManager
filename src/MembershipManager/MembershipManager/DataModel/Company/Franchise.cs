using MembershipManager.Engine;
using NpgsqlTypes;
using System.Text;
using System.Windows;

namespace MembershipManager.DataModel.Company
{
    [DbTableName("franchise")]
    public class Franchise : ISql
    {
        public static Franchise? CurrentFranchise { get; set; } = (Franchise)Select(1); //TODO: Remove this value when the login is implemented

        [DbPrimaryKey(NpgsqlDbType.Integer)]
        [DbAttribute("id")]
        public int Id { get; set; }

        [DbAttribute("structure_name")]
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
            StringBuilder message = new();
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

        }

        public void Insert()
        {
            throw new NotImplementedException();
        }

        public static void Delete(params object[] pk)
        {
            throw new NotImplementedException();
        }
    }
}
