using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace alert_roster.web.Models
{
    public class User
    {
        public int ID { get; set; }

        [Required]
        public String Name { get; set; }

        [EmailAddress, Display(Name = "Email Address")]
        public String EmailAddress { get; set; }

        [Display(Name = "Enable Email Notifications")]
        public Boolean EmailEnabled { get; set; }

        [Phone, Display(Name = "Cell Phone Number")]
        public String PhoneNumber { get; set; }

        [Display(Name = "Enable SMS Notifications")]
        public Boolean SMSEnabled { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
    }
}