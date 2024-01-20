using MembershipManager.DataModel.Buyable;
using System.Windows;

namespace MembershipManager.View.Buyable
{
    /// <summary>
    /// Interaction logic for EntryDetailWindow.xaml
    /// </summary>
    public partial class EntryDetailWindow : Window
    {
        private bool _IsEditMode = false;
        private EntryDetail _entryDetail;

        public EntryDetailWindow(Entry entry)
        {
            InitializeComponent();
            _entryDetail = new EntryDetail(entry);

            _IsEditMode = entry.Id is not null;
            if (_IsEditMode) _entryDetail.TextBoxAmount.Focus();
            MainFrame.Navigate(_entryDetail);
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

            Entry entry = _entryDetail.Entry;
            if (_IsEditMode)
            {
                entry.Update();
            }
            else
            {
                entry.Insert();
            }
            Close();
        }
    }
}
