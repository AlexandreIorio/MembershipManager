using MembershipManager.Engine;
using NpgsqlTypes;
using System.Text;
using System.Windows;

namespace MembershipManager.DataModel.Company
{
    /// <summary>
    /// Class which represent a structure. A structure can hold multiple franchise.
    /// </summary>
    [DbTableName("structure")]
    public class Structure : ISql
    {
        /// <summary>
        /// The name of the structure
        /// <summary/>
        [DbPrimaryKey(NpgsqlDbType.Varchar, 50)]
        [DbAttribute("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Complete Head office address.
        /// </summary>
        [DbAttribute("head_office_address")]
        public string? HeadOfficeAddress { get; set; }

        /// <summary>
        /// City where the structure is located.
        /// </summary>
        [DbRelation("city_id")]
        public City? City { get; set; }
        public static Structure CurrentStructure { get; internal set; }

        /// <summary>
        /// basic constructor
        /// </summary>
        public Structure() { }

        /// <summary>
        /// <inheritdoc/>
        /// <summary/>
        public static ISql? Select(params object[] pk)
        {

            if (pk.Length != 1) throw new ArgumentException();
            Structure? s = ISql.Get<Structure>(pk[0]);
            if (s == null) throw new KeyNotFoundException();

            return s;

        }

        /// <summary>
        /// <inheritdoc/>
        /// <summary/>
        bool ISql.Validate()
        {
            StringBuilder message = new();
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

        #region not implemented yet
        public void Insert()
        {
            DbManager.Db?.Send(ISql.InsertQuery<Structure>(this));
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public static void Delete(params object[] pk)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
