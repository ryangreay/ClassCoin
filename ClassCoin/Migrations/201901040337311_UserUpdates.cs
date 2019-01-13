namespace ClassCoin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student", "Username", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Student", "Username");
        }
    }
}
