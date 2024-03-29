﻿using MembershipManager.Engine;
using System.Text;
using System.Windows;

namespace MembershipManager.Resources
{
    [DbTableName("settings")]
    public class Settings : ISql
    {
        public static Settings Values { get; internal set; }


        [DbPrimaryKey(NpgsqlTypes.NpgsqlDbType.Integer)]
        [DbAttribute("id")]
        public int? Id { get; set; }

        [DbAttribute("payment_terms")]
        public int? PaymentTerms { get; set; }

        [DbAttribute("payment_cash")]
        public bool? PaymentCash { get; set; }

        public static void Delete(params object[] pk)
        {
            throw new NotImplementedException();
        }

        public static ISql? Select(params object[] pk)
        {
            Settings? s = ISql.Get<Settings>(pk);
            return s;
        }

        public void Insert()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            if (Validate()) DbManager.Db?.Send(ISql.UpdateQuery<Settings>(this));
        }

        public bool Validate()
        {
            StringBuilder message = new();
            bool valid = true;
            if (PaymentTerms is null)
            {
                message.AppendLine("Le délai de paiement est obligatoire");
                valid = false;
            }
            if (Id is null)
            {
                message.AppendLine("L'id est obligatoire");
                valid = false;
            }

            if (!valid)
            {
                MessageBox.Show(message.ToString(), "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return valid;

        }

    }
}
