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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MembershipManager.Utils.UserControls
{
    /// <summary>
    /// Logique d'interaction pour EntryH.xaml
    /// </summary>
    public partial class EntryH : UserControl
    {
        public TextBox TextBox { get => Tb; }
        public string Title { get => Lab.Content.ToString(); set => Lab.Content = value; }

        public double LabelWidth { get => Lab.Width;  set => Lab.Width = value; }
        public double TextBoxWidth { get => Tb.Width;  set => Tb.Width = value; }
        public HorizontalAlignment LabelAlignment { get => Lab.HorizontalAlignment; set => Lab.HorizontalAlignment = value; }
        public EntryH()
        {
            InitializeComponent();
            //default values
            LabelWidth = 80;
            TextBoxWidth = 150;
        }
    }
}
