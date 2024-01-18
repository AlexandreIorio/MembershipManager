using System.Windows;

namespace MembershipManager.View.People.Person
{
    /// <summary>
    /// Logique d'interaction pour PersonDetailWindow.xaml
    /// </summary>
    public partial class PersonDetailWindow : Window
    {
        private bool _IsEditMode = false;
        private PersonDetail _PersonDetail;
        public PersonDetailWindow(MembershipManager.DataModel.People.Person? p = null)
        {
            InitializeComponent();
            _PersonDetail = new PersonDetail(p);
            MainFrame.Navigate(_PersonDetail);
            _IsEditMode = p is not null;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            MembershipManager.DataModel.People.Person person = _PersonDetail.Person;
            if (_IsEditMode)
            {
                person.Update();
            }
            else
            {
                person.Insert();
            }
            Close();
        }
    }
}
