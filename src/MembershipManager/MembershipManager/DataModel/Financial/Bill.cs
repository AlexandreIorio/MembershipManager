using MembershipManager.Engine;
using System.Text;
using System.Windows;

namespace MembershipManager.DataModel.Financial
{
    [DbTableName("bill")]
    [DbInherit(typeof(Paiement))]
    public class Bill : Paiement, ISql
    {
        [DbAttribute("issue_date")]
        public DateTime IssueDate { get; set; }

        [DbAttribute("payed")]
        public bool Payed { get; set; }

        public void Generate()
        {
            if (Validate()) DbManager.Db?.Send(ISql.InsertQuery<Bill>(this));
        }

        public new static ISql? Select(params object[] pk)
        {
            if (pk.Length != 1) throw new ArgumentException();
            Paiement? p = ISql.Get<Paiement>(pk[0]);
            return p == null ? throw new KeyNotFoundException() : (ISql)p;
        }

        public new void Insert()
        {
            if (Validate()) DbManager.Db?.Send(ISql.InsertQuery<Paiement>(this));
        }

        public new void Update()
        {
            if (Validate()) DbManager.Db?.Send(ISql.UpdateQuery<Paiement>(this));
        }

        public new bool Validate()
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
        public static new void Delete(params object[] pk)
        {
            ISql.Erase<Paiement>(pk);
        }
    }
}
