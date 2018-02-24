using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlertRoster.Web.Models
{
    public class Member
    {
        [Key]
        public int Id { get; private set; }

        // Email Address?

        [StringLength(20)]
        public String PhoneNumber { get; private set; }

        [Required, StringLength(20)]
        public String DisplayName { get; private set; }

        public virtual ICollection<MemberGroup> Groups { get; private set; }

        private Member()
        {
            // Parameter-less ctor for EF
        }

        public Member(String phoneNumber, String displayName)
        {
            this.PhoneNumber = phoneNumber;
            this.DisplayName = displayName;
            this.Groups = new List<MemberGroup>();
        }
    }
}