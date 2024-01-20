using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace MembershipManager.View.Utils
{
    /// <summary>
    /// This class is used to display a list of Ilistable item and select one of them.
    /// </summary>
    public partial class ListSelection : Window
    {
        public Button ButtonCancel { get => BtnCancel; }
        public Button ButtonSelect { get => BtnSelect; }
        private Type _type { get; set; }
        private IEnumerable _items { get; set; }

        private bool Ascending = false;

        public ListView List { get => _listSelectionPage.List; }

        private ListSelectionPage _listSelectionPage { get; set; }

        public ListSelection(IEnumerable list)
        {
            InitializeComponent();
            if (list is null) throw new ArgumentNullException(nameof(list));

            _listSelectionPage = new ListSelectionPage(list);
            FrameList.Navigate(_listSelectionPage);
            _listSelectionPage.TextBoxSearch.Focus();

        }
        public void UpdateList(IEnumerable list)
        {
            _listSelectionPage.UpdateList(list);
        }
    }
}
