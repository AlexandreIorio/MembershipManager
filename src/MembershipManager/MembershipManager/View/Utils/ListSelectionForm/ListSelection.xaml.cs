using MembershipManager.View.Utils.ListSelectionForm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace MembershipManager.View.Utils
{
    /// <summary>
    /// Logique d'interaction pour ListSelection.xaml
    /// </summary>
    public partial class ListSelection : Window
    {
        public ListBox List { get => Lst; }
        public Button ButtonCancel { get => BtnCancel; }
        public Button ButtonSelect { get => BtnSelect; }

        private Type _type { get; set; }
        private List<object> _selectedItems { get; set; } = new List<object>();
        private IEnumerable _items { get; set; }

        public ListSelection(IEnumerable list)
        {
            InitializeComponent();
            list = list ?? throw new ArgumentNullException(nameof(list));

            _type = list.GetType().GetGenericArguments()[0];

            InitializeList();
            InitializeFilter();
            _items = list;
            Lst.ItemsSource = list;
            EntrySearch.TextBox.TextChanged += TextBox_TextChanged;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterList();
        }

        private void FilterList()
        {
            ComboBoxItem item = (ComboBoxItem)ComboBoxFilters.SelectedItem;
            Lst.ItemsSource = _items;
            List<object> list = Lst.ItemsSource.Cast<object>().ToList();
            Lst.ItemsSource = list.Where(x =>{

                string? tag = item.Tag.ToString();
                if (tag is null) return false;

                PropertyInfo? prop = x.GetType().GetProperty(tag);
                if (prop is null) return false;

                string? value = prop.GetValue(x)?.ToString();
                if (value is null) return false;

                return value.Contains(EntrySearch.TextBox.Text, StringComparison.CurrentCultureIgnoreCase);
            }

            );
        }

        private void InitializeList()
        {
            GridView gv = new GridView();
            foreach (PropertyInfo p in _type.GetProperties())
            {
                if (p.GetCustomAttribute<Displayed>() is Displayed displayedAttribute)
                {
                    GridViewColumn column = new GridViewColumn();
                    column.Header = displayedAttribute.HeaderName;
                    column.DisplayMemberBinding = new Binding(p.Name);
                    gv.Columns.Add(column);
                }
            }
            Lst.View = gv;
        }

        private void InitializeFilter()
        {
            ComboBoxItem item;
            foreach (PropertyInfo p in _type.GetProperties())
            {
                if (p.GetCustomAttribute<Filtered>() is Filtered filtered)
                {
                    item = new ComboBoxItem();
                    item.Content = filtered.FriendlyName;
                    item.Tag = p.Name;
                    if (filtered.IsDefault)
                    {
                        ComboBoxFilters.Items.Insert(0, item);
                        ComboBoxFilters.SelectedItem = item;
                    }
                    else
                    {
                        ComboBoxFilters.Items.Add(item);
                    }                    
                }
            }
        }

        private void Lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Lst_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {

        }
    }





}
