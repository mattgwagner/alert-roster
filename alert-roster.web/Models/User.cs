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

        [EmailAddress]
        public String EmailAddress { get; set; }

        public Boolean EmailEnabled { get; set; }
    }
}