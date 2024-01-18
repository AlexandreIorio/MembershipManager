using MembershipManager.Engine;

namespace MembershipManager.DataModel.Financial
{
    [DbTableName("bill")]
    [DbInherit(typeof(Paiement))]
    internal class Bill : Paiement
    {
        [DbAttribute("issue_date")]
        public DateTime IssueDate { get; set; }

        //public Bill() : base()
        //{
        //    this.IssueDate = DateTime.Now + ;
        //}

        //public Bill(Paiement person) : base(person)
        //{
        //    this.SubscriptionDate = DateTime.Now;
        //}
    }
}
