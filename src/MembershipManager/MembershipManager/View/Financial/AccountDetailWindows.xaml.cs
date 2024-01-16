using MembershipManager.DataModel.Financial;
using MembershipManager.DataModel.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MembershipManager.View.Financial
{
    /// <summary>
    /// Logique d'interaction pour AccountDetailWindows.xaml
    /// </summary>
    public partial class AccountDetailWindows : Window
    {
        public List<ITransaction> Transactions { get; set; } = new List<ITransaction>();
        public Member Member { get; private set; }

        public string MemberName
        {
            get
            {
                return Member.FirstName + " " + Member.LastName;
            }
        }   

        public int Balance
        {
            get
            {
                int balance = 200;
                foreach (ITransaction t in Transactions)
                {
                    balance += t.GetAmount();
                }
                return balance;
            }
        }

        public AccountDetailWindows(Member member)
        {
            Member = member;
            InitializeComponent();
            DataContext = this;
        }
    }
}
