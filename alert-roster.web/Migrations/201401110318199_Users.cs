namespace alert_roster.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Users : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    EmailAddress = c.String(),
                    EmailEnabled = c.Boolean()
                })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
