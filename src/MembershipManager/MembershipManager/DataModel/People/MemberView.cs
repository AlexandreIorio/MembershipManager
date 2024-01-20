using MembershipManager.Engine;
using MembershipManager.View.Financial;
using MembershipManager.View.People.Member;
using MembershipManager.View.Utils;
using MembershipManager.View.Utils.ListSelectionForm;
using System.Windows;
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

        public static ContextMenu ContextMenu(ListSelectionPage viewer)
        {
            ContextMenu contextMenu = new();

            contextMenu.Items.Add(CreateButtonNewMember(viewer));
            contextMenu.Items.Add(CreateButtonEditMember(viewer));
            contextMenu.Items.Add(CreateButtonDeleteMember(viewer));

            return contextMenu;
        }

        private static MenuItem CreateButtonDeleteMember(ListSelectionPage viewer)
        {
            MenuItem delete = new MenuItem();
            delete.Header = "Supprimer";
            delete.Click += (sender, e) =>
            {
                MemberView? member = GetContextMenuSelectedObject((MenuItem)sender);
                if (member is null) return;
                if (!CanDeleteMember(member.no_avs)) return;
                ISql.Delete(typeof(Member), member.no_avs);
                viewer.UpdateList(Member.Views().Cast<MemberView>());
            };
            return delete;
        }

        private static MenuItem CreateButtonEditMember(ListSelectionPage viewer)
        {
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
            return edit;
        }

        private static MenuItem CreateButtonNewMember(ListSelectionPage viewer)
        {
            MenuItem newItem = new()
            {
                Header = "Nouveau membre"
            };
            newItem.Click += (sender, e) =>
            {
                NewMember();
                viewer.UpdateList(Member.Views().Cast<MemberView>());
            };
            return newItem;
        }

        private static MemberView? GetContextMenuSelectedObject(MenuItem menuItem)
        {
            // Get element from menu item
            if (menuItem == null) return null;
            if (menuItem.Parent is not ContextMenu contextMenu) return null;

            if (contextMenu.PlacementTarget is not ListView list) return null;

            if (list.SelectedItem is not MemberView member) return null;

            if (member.no_avs is null) return null;
            return member;
        }

        private static bool CanDeleteMember(string? noAvs)
        {
            ArgumentNullException.ThrowIfNull(noAvs);
            Member? member = (Member?)Member.Select(noAvs);
            if (member is null) return false;
            if (member.Account?.Balance != 0)
            {
                MessageBox.Show("Impossible de supprimer un membre avec un solde différent de 0");
                return false;
            }

            return true;
        }
    }
}

