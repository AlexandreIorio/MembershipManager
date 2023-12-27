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
            Member m = new Member("7569854624538");
        }
    }
}