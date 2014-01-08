using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace alert_roster.web.Models
{
    public class AlertRosterDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
    }
}