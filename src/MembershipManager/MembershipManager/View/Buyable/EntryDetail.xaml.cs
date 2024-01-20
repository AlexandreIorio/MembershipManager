using MembershipManager.DataModel.Buyable;
using System.Windows.Controls;

namespace MembershipManager.View.Buyable
{
    /// <summary>
    /// Interaction logic for EntryDetail.xaml
    /// </summary>
    public partial class EntryDetail : Page
    {
        public Entry Entry { get; set; }

        public EntryDetail(Entry entry)
        {
            this.Entry = entry;
            InitializeComponent();
            this.DataContext = entry;
        }
    }
}
