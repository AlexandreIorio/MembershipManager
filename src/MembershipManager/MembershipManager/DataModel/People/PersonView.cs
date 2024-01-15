using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using MembershipManager.Engine;
using MembershipManager.View.People.Person;
using MembershipManager.View.Utils;
using MembershipManager.View.Utils.ListSelectionForm;

namespace MembershipManager.DataModel.People
{
    public class PersonView : SqlViewable
    {
        [Displayed("N° Avs")]
        [Filtered("N° Avs")]
        public string? no_avs { get; set; }

        [Sorted]
        [Filtered("Prénom")]
        [Displayed("Prénom")]
        public string? first_name { get; set; }

        [Filtered("Nom")]
        [Displayed("Nom")]
        public string? last_name { get; set; }

        [Displayed("Ville")]
        public string? city_name { get; set; }

        [Displayed("Canton")]
        public string? canton_name { get; set; }

        [IgnoreSql]
        [Filtered("Tout", true)]
        public string ? FullName { get => $"{first_name} {last_name}"; }

        #region Events
        public static void EditPerson(string noAvs)
        {
            Person? person = (Person?)Person.Select(noAvs);
            if (person is null) return;
            PersonDetailWindow personDetailWindow = new PersonDetailWindow(person);
            personDetailWindow.ShowDialog();
        }

        public static void NewPerson()
        {
            PersonDetailWindow personDetailWindow = new PersonDetailWindow();
            personDetailWindow.ShowDialog();
        }

        #endregion
    }
}
