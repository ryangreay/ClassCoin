namespace ClassCoin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserUpdates2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Username", c => c.String());
            DropColumn("dbo.Student", "Username");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Student", "Username", c => c.String());
            DropColumn("dbo.User", "Username");
        }
    }
}
