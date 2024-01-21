using MembershipManager.View.Utils.ListSelectionForm;
using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MembershipManager.View.Utils
{
    /// <summary>
    /// This class is used to display a list of Ilistable item and select one of them.
    /// </summary>
    public partial class ListSelectionPage : Page
    {
        private Type _type { get; set; }
        private IEnumerable _items { get; set; }

        private bool Ascending = false;

        public ListSelectionPage(IEnumerable list)
        {
            InitializeComponent();
            TextBoxSearch.Focus();

            list = list ?? throw new ArgumentNullException(nameof(list));

            _type = list.GetType().GetGenericArguments()[0];

            _items = list;
            TextBoxSearch.TextChanged += TextBox_TextChanged;
            InitializeList();
            InitializeFilter();
            FilterList();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterList();
        }

        private void FilterList()
        {
            ComboBoxItem item = (ComboBoxItem)ComboBoxFilters.SelectedItem;
            List.ItemsSource = _items;
            if (item is null) return;

            PropertyInfo? sortedProperty = _type.GetProperties().FirstOrDefault(x => x.GetCustomAttribute<Sorted>() != null);
            if (sortedProperty is not null)
            {
                SortListByProperty(sortedProperty);
            }

            List<object> list = List.ItemsSource.Cast<object>().ToList();
            List.ItemsSource = list.Where(x =>
            {

                string? tag = item.Tag?.ToString();
                if (tag is null) return false;

                PropertyInfo? prop = x.GetType().GetProperty(tag);
                if (prop is null) return false;

                string? value = prop.GetValue(x)?.ToString();
                if (value is null) return false;

                return value.Contains(TextBoxSearch.Text, StringComparison.CurrentCultureIgnoreCase);
            }

            );



            if (List.Items.Count == 1)
            {
                List.SelectedIndex = 0;
            }
        }

        private void InitializeList()
        {
            GridView gv = new();
            foreach (PropertyInfo p in _type.GetProperties())
            {
                if (p.GetCustomAttribute<Displayed>() is Displayed displayedAttribute)
                {
                    //Get TextFormat attribute if exists
                    string? format = p.GetCustomAttribute<TextFormat>()?.Format;

                    GridViewColumn column = new()
                    {
                        DisplayMemberBinding = new Binding(p.Name)
                    };
                    if (format is not null) column.DisplayMemberBinding.StringFormat = format;

                    GridViewColumnHeader header = new();
                    header.Click += List_Click;
                    header.Content = displayedAttribute.HeaderName;
                    header.Tag = p;
                    column.Header = header;
                    gv.Columns.Add(column);
                }
            }
            List.View = gv;

        }

        private void InitializeFilter()
        {
            ComboBoxItem item;
            foreach (PropertyInfo p in _type.GetProperties())
            {
                if (p.GetCustomAttribute<Filtered>() is Filtered filtered)
                {
                    item = new ComboBoxItem
                    {
                        Content = filtered.FriendlyName,
                        Tag = p.Name
                    };
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

        private void List_Click(object sender, RoutedEventArgs e)
        {
            PropertyInfo? propertyToSort = ((GridViewColumnHeader)sender).Tag as PropertyInfo;
            if (propertyToSort is null) return;
            SortListByProperty(propertyToSort);
        }

        private void SortListByProperty(PropertyInfo property)
        {
            if (Ascending)
            {
                List.ItemsSource = List.ItemsSource.Cast<object>().OrderByDescending(x => property.GetValue(x));
            }
            else
            {
                List.ItemsSource = List.ItemsSource.Cast<object>().OrderBy(x => property.GetValue(x));
            }
            Ascending = !Ascending;
        }

        public void UpdateList(IEnumerable list)
        {
            _items = list;
            FilterList();
        }

    }





}
