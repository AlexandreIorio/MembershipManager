﻿using System.Windows;
using System.Windows.Controls;

namespace MembershipManager.View.Utils.UserControls
{
    /// <summary>
    /// Logique d'interaction pour EntryH.xaml
    /// </summary>
    public partial class Entry : UserControl
    {

        public event EventHandler TextChanged;

        public TextBox TextBox { get => Tb; }
        public HorizontalAlignment TextBoxAlignment { get => Tb.HorizontalAlignment; set => Tb.HorizontalAlignment = value; }

        public Label Label { get => Lab; }

        public Orientation Orientation { get => Sp.Orientation; set => Sp.Orientation = value; }

        public string Text { get => Tb.Text; set => Tb.Text = value; }
        public string Title { set => Lab.Content = value; }

        public double LabelWidth { get => Lab.Width; set => Lab.Width = value; }
        public double TextBoxWidth { get => Tb.Width; set => Tb.Width = value; }
        public HorizontalAlignment LabelAlignment { get => Lab.HorizontalAlignment; set => Lab.HorizontalAlignment = value; }
        public Entry()
        {
            InitializeComponent();
            //default values
            Orientation = Orientation.Horizontal;
            TextBox.TextChanged += (sender, e) => { TextChanged?.Invoke(sender, e); };
        }


    }
}
