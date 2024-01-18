using MembershipManager.Engine;
using MembershipManager.View.Financial;
using MembershipManager.View.People.Member;
using MembershipManager.View.Utils;
using MembershipManager.View.Utils.ListSelectionForm;
using System.Globalization;
using System.Windows.Controls;

namespace MembershipManager.DataModel.People
{
    public class MemberView : PersonView
    {
        [TextFormat("{0:d}")]
        [Displayed("Date d'inscription")]
        public DateTime subscription_date { get; set; }

        [Displayed("Structure")]
        public string? structure_name { get; set; }

        #region Events
        public static void EditMember(string? noAvs)
        {
            ArgumentNullException.ThrowIfNull(noAvs);
            Member? member = (Member?)Member.Select(noAvs);
            if (member is null) return;
            MemberDetailWindows memberDetailWindow = new(member);
            memberDetailWindow.ShowDialog();
        }

        public static void NewMember()
        {
            MemberDetailWindows memberDetailWindow = new(new Member());
            memberDetailWindow.ShowDialog();
        }

        public static ContextMenu ContextMenu(ListSelection viewer)
        {
            ContextMenu contextMenu = new();
            MenuItem edit = new()
            {
                Header = "Modifier"
            };
            edit.Click += (sender, e) =>
            {
                MemberView? member = GetContextMenuSelectedObject((MenuItem)sender);
                if (member is null) return;

                EditMember(member.no_avs);
                viewer.UpdateList(Member.Views().Cast<MemberView>());
            };
            contextMenu.Items.Add(edit);

            MenuItem delete = new MenuItem();
            delete.Header = "Supprimer";
            delete.Click += (sender, e) =>
            {
                MemberView? member = GetContextMenuSelectedObject((MenuItem)sender);
                if (member is null) return;
                ISql.Delete(typeof(Member), member.no_avs);
                viewer.UpdateList(Member.Views().Cast<MemberView>());
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
