using MembershipManager.DataModel.People;
using MembershipManager.Engine;
using MembershipManager.View.Utils.ListSelectionForm;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MembershipManager.DataModel.Financial
{
    internal class Paiement : ISql, INotifyPropertyChanged, Ilistable
    {
        [DbPrimaryKey(NpgsqlDbType.Char, 13)]
        [DbAttribute("id")]
        public int? Id { get; set; }

        [DbAttribute("amount")]
        public int? Amount { get; set; }

        [DbAttribute("date")]
        public DateTime? Date { get; set; }

        [DbRelation("account_id")]
        public Account? Account { get; set; }


        public Paiement() { }
        public Paiement(Paiement paiement)
        {
            this.Id = paiement.Id;
            this.Amount = paiement.Amount;
            this.Date = paiement.Date;
            this.Account = paiement.Account;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public static ISql? Select(params object[] pk)
        {
            if (pk.Length != 1) throw new ArgumentException();
            Paiement? p = ISql.Get<Paiement>(pk[0]);
            return p == null ? throw new KeyNotFoundException() : (ISql)p;
        }

        public void Insert()
        {
            if (Validate()) DbManager.Db?.Send(ISql.InsertQuery<Paiement>(this));
        }

        public void Update()
        {
            if (Validate()) DbManager.Db?.Send(ISql.UpdateQuery<Paiement>(this));
        }

        public bool Validate()
        {
            StringBuilder message = new StringBuilder();
            bool valid = true;
            if (Account is null)
            {
                message.AppendLine("Un lien vers un compte est obligatoire");
                valid = false;
            }
            if (Amount == 0)
            {
                message.AppendLine("La valeur d'un paiement ne peut pas être 0");
                valid = false;
            }
            if (!Date.HasValue)
            {
                message.AppendLine("La date doit être spécifiée");
                valid = false;
            }

            if (!valid)
            {
                MessageBox.Show(message.ToString(),
                    "Erreur",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);

            }
            return valid;
        }

        public override string ToString()
        {
            return $"{Amount} {Date}";
        }

        public static List<SqlViewable>? Views(params NpgsqlParameter[] sqlParam)
        {
            NpgsqlCommand cmd = new()
            {
                CommandText = @"SELECT no_avs, first_name, last_name, address, city.name as city_name, canton.name as canton_name
                                FROM person
                                    INNER JOIN city ON city_id = city.id
                                    INNER JOIN canton ON canton_abbreviation = canton.abbreviation;"
            };

            return DbManager.Db.Views<PaiementView>(cmd).Cast<SqlViewable>().ToList();
        }
    }
}
