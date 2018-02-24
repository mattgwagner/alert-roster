using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public virtual ICollection<Message> Messages { get; private set; }

        // TODO Who are admins?

        // TODO Reply-mode : Reply-All, Reply-to-Admin, Reject?

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
    }
}