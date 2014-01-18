using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace alert_roster.web.Models
{
    public class Group
    {
        public int ID { get; set; }

        [Required]
        public String Name { get; set; }

        public virtual ICollection<User> Members { get; set; }
    }
}