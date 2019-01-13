namespace ClassCoin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbUpdate2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Firstname = c.String(),
                        Lastname = c.String(),
                        ProfileImageURL = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        SecurityStamp = c.String(),
                        UserConfirmed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            AddColumn("dbo.Notification", "User_UserID", c => c.Int());
            CreateIndex("dbo.Notification", "User_UserID");
            AddForeignKey("dbo.Notification", "User_UserID", "dbo.User", "UserID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notification", "User_UserID", "dbo.User");
            DropIndex("dbo.Notification", new[] { "User_UserID" });
            DropColumn("dbo.Notification", "User_UserID");
            DropTable("dbo.User");
        }
    }
}
