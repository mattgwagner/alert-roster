namespace alert_roster.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserSMS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PhoneNumber", c => c.String());
            AddColumn("dbo.Users", "SMSEnabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "SMSEnabled");
            DropColumn("dbo.Users", "PhoneNumber");
        }
    }
}
