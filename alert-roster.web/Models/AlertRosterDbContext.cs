using alert_roster.web.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace alert_roster.web.Models
{
    public class AlertRosterDbContext : DbContext
    {
        public AlertRosterDbContext()
            : base("MainDb")
        {

        }

        public DbSet<Message> Messages { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AlertRosterDbContext, Configuration>());
        }
    }
}