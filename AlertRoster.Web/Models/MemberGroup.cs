namespace AlertRoster.Web.Models
{
    public class MemberGroup
    {
        public int MemberId { get; private set; }

        public virtual Member Member { get; private set; }

        public int GroupId { get; private set; }

        public virtual Group Group { get; private set; }

        public GroupRole Role { get; set; } = GroupRole.Member;

        private MemberGroup()
        {
            // Parameter-less ctor for EF
        }

        public MemberGroup(int memberId, int groupId)
        {
            this.MemberId = memberId;
            this.GroupId = groupId;
        }

        public enum GroupRole : byte
        {
            Member = 0,

            Administrator = 1
        }
    }
}