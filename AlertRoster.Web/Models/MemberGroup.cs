namespace AlertRoster.Web.Models
{
    public class MemberGroup
    {
        public int MemberId { get; private set; }

        public virtual Member Member { get; private set; }

        public int GroupId { get; private set; }

        public virtual Group Group { get; private set; }

        private MemberGroup()
        {
            // Parameter-less ctor for EF
        }

        public MemberGroup(int memberId, int groupId)
        {
            this.MemberId = memberId;
            this.GroupId = groupId;
        }
    }
}