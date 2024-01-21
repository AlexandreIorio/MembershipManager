using MembershipManager.DataModel.People;
using MembershipManager.Engine;
using NpgsqlTypes;
using System.Text;
using System.Windows;

namespace MembershipManager.DataModel.Company
{
    /// <summary>
    /// Class which represent a franchise. A franchise is hold by a Structure.
    /// </summary>
    [DbTableName("franchise")]
    public class Franchise : ISql
    {
        /// <summary>
        /// Static field to hold the current franchise. This field is used to know which franchise is currently used.
        /// </summary>
        public static Franchise? CurrentFranchise { get; set; } = (Franchise)Select(1); //TODO: Remove this value when the login is implemented

        /// <summary>
        /// The id of the franchise.
        /// </summary>
        [DbPrimaryKey(NpgsqlDbType.Integer)]
        [DbAttribute("id")]
        public int? Id { get; set; }

        /// <summary>
        /// The name of the franchise.
        /// </summary>
        [DbAttribute("structure_name")]
        public string? StructureName { get; set; }

        /// <summary>
        /// The address of the franchise.
        /// </summary>
        [DbAttribute("address")]
        public string? Address { get; set; }

        /// <summary>
        /// The city of the franchise.
        /// </summary>
        [DbRelation("city_id")]
        public City? City { get; set; }

        /// <summary>
        /// basic constructor
        /// </summary>
        public Franchise() { }

        /// <summary>
        /// <inheritdoc/>
        /// <summary/>
        public static ISql? Select(params object[] pk)
        {
            if (pk.Length != 1) throw new ArgumentException();
            Franchise? f = ISql.Get<Franchise>(pk[0]);
            return f == null ? throw new KeyNotFoundException() : (ISql)f;
        }

        /// <summary>
        /// <inheritdoc/>
        /// <summary/>
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

        /// <summary>
        /// <inheritdoc/>
        /// <summary/>
        public void Update()
        {

        }

        /// <summary>
        /// <inheritdoc/>
        /// <summary/>
        public void Insert()
        {
            DbManager.Db?.Send(ISql.InsertQuery<Franchise>(this));
        }

        /// <summary>
        /// <inheritdoc/>
        /// <summary/>
        public static void Delete(params object[] pk)
        {
            throw new NotImplementedException();
        }
    }
}
