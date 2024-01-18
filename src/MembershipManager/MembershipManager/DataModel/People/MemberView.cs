using MembershipManager.View.Financial;
using MembershipManager.View.People.Member;
using MembershipManager.View.People.Person;
using MembershipManager.View.Utils.ListSelectionForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MembershipManager.DataModel.People
{
    public class MemberView : PersonView
    {
        [Displayed("Date d'inscription")]
        public DateTime subscription_date { get; set; }

        [Displayed("Structure")]
        public string? structure_name { get; set; }

        #region Events
        public static void EditMember(string? noAvs)
        {
            if (noAvs is null) throw new ArgumentNullException(nameof(noAvs));
            Member? member = (Member?)Member.Select(noAvs);
            if (member is null) return;
            MemberDetailWindows memberDetailWindow = new MemberDetailWindows(member);
            memberDetailWindow.ShowDialog();
        }

        public static void NewMember()
        {
            MemberDetailWindows memberDetailWindow = new MemberDetailWindows(new Member());
            memberDetailWindow.ShowDialog();
        }

        public static ContextMenu ContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem edit = new MenuItem();
            edit.Header = "Modifier";
            edit.Click += (sender, e) =>
            {
                MemberView? member = GetContextMenuSelectedObject((MenuItem)sender);
                if (member is null) return;

                MemberView.EditMember(member.no_avs);
            };
            contextMenu.Items.Add(edit);

            MenuItem delete = new MenuItem();
            delete.Header = "Supprimer";
            delete.Click += (sender, e) =>
            {
                string? noAvs = (sender as MemberView)?.no_avs;
                if (noAvs is null) return;
                //Member.Delete();
            };

            contextMenu.Items.Add(delete);

            MenuItem account = new MenuItem();
            account.Header = "Compte";
            account.Click += (sender, e) =>
            {
                MemberView? memberView = GetContextMenuSelectedObject((MenuItem)sender);
                if (memberView is null) return;
                Member? member = (Member?)Member.Select(memberView?.no_avs);

                if (member is null) return;
                AccountDetailWindows accountDetailWindow = new AccountDetailWindows(member);
                accountDetailWindow.ShowDialog();
            };
            contextMenu.Items.Add(account);


            return contextMenu;
        }

        private static MemberView? GetContextMenuSelectedObject(MenuItem menuItem)
        {
            // Get element from menu item
            if (menuItem == null) return null;
            ContextMenu? contextMenu = menuItem.Parent as ContextMenu;
            if (contextMenu == null) return null;

            ListView? list = contextMenu.PlacementTarget as ListView;
            if (list == null) return null;

            MemberView? member = list.SelectedItem as MemberView;
            if (member == null) return null;

            if (member.no_avs is null) return null;
            return member;
        }

        #endregion
    }


}
