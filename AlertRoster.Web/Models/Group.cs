using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AlertRoster.Web.Models
{
    public class Group
    {
        [Key]
        public int Id { get; private set; }

        [Required, StringLength(25)]
        public String DisplayName { get; set; }

        [StringLength(25)]
        public String PhoneNumber { get; set; }

        public virtual ICollection<MemberGroup> Members { get; private set; }

        public virtual IEnumerable<Member> Admins => Members?.Where(_ => _.Role == MemberGroup.GroupRole.Administrator).Select(_ => _.Member);

        public virtual ICollection<Message> Messages { get; private set; }

        [Required]
        public ReplyMode Replies { get; set; } = ReplyMode.ReplyAll;

        private Group()
        {
            // Parameter-less ctor for EF
        }

        public Group(String displayName)
        {
            this.DisplayName = displayName;
            this.Members = new List<MemberGroup>();
            this.Messages = new List<Message>();
        }

        public enum ReplyMode : byte
        {
            ReplyAll = 0,

            ReplyToAdmins = 1,

            Reject = 2
        }
    }
}