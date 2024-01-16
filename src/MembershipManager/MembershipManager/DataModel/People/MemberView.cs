using MembershipManager.View.People.Member;
using MembershipManager.View.People.Person;
using MembershipManager.View.Utils.ListSelectionForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipManager.DataModel.People
{
    public class MemberView : PersonView
    {
        [Displayed("Date d'inscription")]
        public DateTime subscription_date { get; set; }

        [Displayed("Structure")]
        public string? structure_name { get; set; }

        #region Events
        public static void EditMember(string noAvs)
        {
            Member? member = (Member?)Member.Select(noAvs);
            if (member is null) return;
            MemberDetailWindows memberDetailWindow = new MemberDetailWindows(member);
            memberDetailWindow.ShowDialog();
        }

        public static void NewMember()
        {
            MemberDetailWindows memberDetailWindow = new MemberDetailWindows();
            memberDetailWindow.ShowDialog();
        }

        #endregion
    }


}
