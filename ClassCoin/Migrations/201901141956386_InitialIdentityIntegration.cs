namespace ClassCoin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialIdentityIntegration : DbMigration
    {
        public override void Up()
        {
            
            DropForeignKey("dbo.Notification", "User_UserID", "dbo.User");
            DropIndex("dbo.Notification", new[] { "User_UserID" });
            RenameColumn(table: "dbo.Notification", name: "User_UserID", newName: "User_Id");
            
            DropPrimaryKey("dbo.User");
            DropColumn("dbo.User", "UserID");
            CreateTable(
                "dbo.IdentityUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.IdentityRole", t => t.IdentityRole_Id)
                .Index(t => t.UserId)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.IdentityRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.User", "Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.User", "EmailConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.User", "PasswordHash", c => c.String());
            AddColumn("dbo.User", "PhoneNumber", c => c.String());
            AddColumn("dbo.User", "PhoneNumberConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.User", "TwoFactorEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.User", "LockoutEndDateUtc", c => c.DateTime());
            AddColumn("dbo.User", "LockoutEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.User", "AccessFailedCount", c => c.Int(nullable: false));
            AlterColumn("dbo.Student", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Instructor", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Notification", "User_Id", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.User", "Id");
            CreateIndex("dbo.Student", "UserId");
            CreateIndex("dbo.Notification", "User_Id");
            CreateIndex("dbo.Instructor", "UserId");
            AddForeignKey("dbo.Student", "UserId", "dbo.User", "Id");
            AddForeignKey("dbo.Instructor", "UserId", "dbo.User", "Id");
            AddForeignKey("dbo.Notification", "User_Id", "dbo.User", "Id");
            DropColumn("dbo.User", "Password");
            DropColumn("dbo.User", "UserConfirmed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "UserConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.User", "Password", c => c.String());
            AddColumn("dbo.User", "UserID", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Notification", "User_Id", "dbo.User");
            DropForeignKey("dbo.AspNetUserRoles", "IdentityRole_Id", "dbo.IdentityRole");
            DropForeignKey("dbo.Instructor", "UserId", "dbo.User");
            DropForeignKey("dbo.Student", "UserId", "dbo.User");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.User");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.User");
            DropForeignKey("dbo.IdentityUserClaim", "UserId", "dbo.User");
            DropIndex("dbo.Instructor", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Notification", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.IdentityUserClaim", new[] { "UserId" });
            DropIndex("dbo.Student", new[] { "UserId" });
            DropPrimaryKey("dbo.User");
            AlterColumn("dbo.Notification", "User_Id", c => c.Int());
            AlterColumn("dbo.Instructor", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Student", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.User", "AccessFailedCount");
            DropColumn("dbo.User", "LockoutEnabled");
            DropColumn("dbo.User", "LockoutEndDateUtc");
            DropColumn("dbo.User", "TwoFactorEnabled");
            DropColumn("dbo.User", "PhoneNumberConfirmed");
            DropColumn("dbo.User", "PhoneNumber");
            DropColumn("dbo.User", "PasswordHash");
            DropColumn("dbo.User", "EmailConfirmed");
            DropColumn("dbo.User", "Id");
            DropTable("dbo.IdentityRole");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.IdentityUserClaim");
            AddPrimaryKey("dbo.User", "UserID");
            RenameColumn(table: "dbo.Notification", name: "User_Id", newName: "User_UserID");
            CreateIndex("dbo.Notification", "User_UserID");
            AddForeignKey("dbo.Notification", "User_UserID", "dbo.User", "UserID");
        }
    }
}
