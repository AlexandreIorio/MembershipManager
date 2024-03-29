﻿using MembershipManager.View.People.Person;
using System.Windows;

namespace MembershipManager.View.People.Member
{
    /// <summary>
    /// Logique d'interaction pour MemberDetailWindows.xaml
    /// </summary>
    public partial class MemberDetailWindows : Window
    {
        private bool _IsEditMode = false;
        private MemberDetail _memberDetail;
        private PersonDetail _personDetail;
        private DataModel.People.Member Member { get; set; }

        public MemberDetailWindows(MembershipManager.DataModel.People.Member m)
        {
            InitializeComponent();
            //Define if we are in edit mode or not
            _IsEditMode = m.NoAvs is not null;

            // Passing m as reference to both PersonDetail and MemberDetail
            _personDetail = new PersonDetail(m);
            _memberDetail = new MemberDetail(m);

            //Set the frame to the windows
            PersonFrame.Navigate(_personDetail);
            MemberFrame.Navigate(_memberDetail);

            Member = m;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

            if (_IsEditMode)
            {
                Member.Update();
            }
            else
            {
                Member.Insert();
            }
            Close();
        }
    }
}
