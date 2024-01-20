using MembershipManager.DataModel.Financial;
using MembershipManager.DataModel.People;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
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
    /// Logique d'interaction pour BillDashBoard.xaml
    /// </summary>
    public partial class BillDashBoard : Window
    {
        public List<BillView>? Bills { get; set; }
        public MemberAccount Account { get; private set; }
        public BillDashBoard()
        {
            InitializeComponent();
            RefreshTransactions();
        }

        private void RefreshTransactions()
        {
            Bills = Bill.Views()?.Cast<BillView>().ToList();
            BillsDataGrid.ItemsSource = Bills;
        }
    }
}
