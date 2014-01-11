namespace alert_roster.web.Migrations
{
    using alert_roster.web.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<alert_roster.web.Models.AlertRosterDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
            ContextKey = "alert_roster.web.Models.AlertRosterDbContext";
        }

        protected override void Seed(alert_roster.web.Models.AlertRosterDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //context.Messages.AddOrUpdate(
            //    m => m.Content,
            //    new Message { PostedDate = DateTime.UtcNow, Content = "This is the initial, test message posting" }
            //    );
        }
    }
}
