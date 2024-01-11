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

namespace MembershipManager.View.Utils.UserControls
{
    /// <summary>
    /// Logique d'interaction pour EntryH.xaml
    /// </summary>
    public partial class Entry : UserControl
    {
        public TextBox TextBox { get => Tb; }
        public HorizontalAlignment TextBoxAlignment { get => Tb.HorizontalAlignment; set => Tb.HorizontalAlignment = value; }

        public Label Label { get => Lab; }

        public Orientation Orientation { get => Sp.Orientation; set => Sp.Orientation = value; }

        public string Text { get => Tb.Text; set => Tb.Text = value; }
        public string Title { get => Lab.Content.ToString(); set => Lab.Content = value; }

        public double LabelWidth { get => Lab.Width; set => Lab.Width = value; }
        public double TextBoxWidth { get => Tb.Width; set => Tb.Width = value; }
        public HorizontalAlignment LabelAlignment { get => Lab.HorizontalAlignment; set => Lab.HorizontalAlignment = value; }
        public Entry()
        {
            InitializeComponent();
            //default values
            Orientation = Orientation.Horizontal;
        }
    }
}
