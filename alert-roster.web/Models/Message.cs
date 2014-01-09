using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace alert_roster.web.Models
{
    public class Message
    {
        public int ID { get; set; }

        public DateTime PostedDate { get; set; }

        public String Content { get; set; }

        public Message()
        {
            this.PostedDate = DateTime.UtcNow;
        }
    }
}