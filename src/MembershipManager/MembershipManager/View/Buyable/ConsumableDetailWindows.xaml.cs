using MembershipManager.DataModel.Buyable;
using MembershipManager.View.Utils;
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

namespace MembershipManager.View.Buyable
{
    /// <summary>
    /// Logique d'interaction pour ConsumableDetailWindows.xaml
    /// </summary>
    public partial class ConsumableDetailWindows : Window
    {
        public ConsumableDetailWindows()
        {
            InitializeComponent();
            ListSelectionPage listSelection = new ListSelectionPage(ProductView.Views().Cast<ProductView>());
            FrameList.Navigate(listSelection);
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
