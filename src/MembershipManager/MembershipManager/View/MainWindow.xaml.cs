using System.Windows;
using MembershipManager.DataModel;
using MembershipManager.DataModel.Person;
using MembershipManager.Engine;
using Npgsql;

namespace MembershipManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Person p = new Person();
            p = Person.GetPerson("7563775467937");

        }
    }
}