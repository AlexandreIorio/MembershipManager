using MembershipManager.View.People.Person;
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

namespace MembershipManager.View.People.Member
{
    /// <summary>
    /// Logique d'interaction pour MemberDetailWindows.xaml
    /// </summary>
    public partial class MemberDetailWindows : Window
    {
        private bool _IsEditMode = false;
        private MemberDetail _memberDetail;
        private PersonDetail _PersonDetail;
        public MemberDetailWindows(MembershipManager.DataModel.People.Member? m = null)
        {
            InitializeComponent();
            _PersonDetail = new PersonDetail(m);
            _memberDetail = new MemberDetail(m);
            PersonFrame.Navigate(_PersonDetail);
            MemberFrame.Navigate(_memberDetail);
            _IsEditMode = m is not null;
        }
    }
}
