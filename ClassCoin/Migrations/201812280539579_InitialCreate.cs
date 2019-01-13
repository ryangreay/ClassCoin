namespace ClassCoin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classroom",
                c => new
                    {
                        ClassroomID = c.Int(nullable: false, identity: true),
                        ClassCode = c.String(),
                        ClassName = c.String(),
                        ClassSubject = c.String(),
                        Store_StoreID = c.Int(),
                        Instructor_InstructorID = c.Int(),
                    })
                .PrimaryKey(t => t.ClassroomID)
                .ForeignKey("dbo.Store", t => t.Store_StoreID)
                .ForeignKey("dbo.Instructor", t => t.Instructor_InstructorID)
                .Index(t => t.Store_StoreID)
                .Index(t => t.Instructor_InstructorID);
            
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        GroupID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Classroom_ClassroomID = c.Int(),
                    })
                .PrimaryKey(t => t.GroupID)
                .ForeignKey("dbo.Classroom", t => t.Classroom_ClassroomID)
                .Index(t => t.Classroom_ClassroomID);
            
            CreateTable(
                "dbo.Student",
                c => new
                    {
                        StudentID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ProfileImageURL = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        Funds = c.Double(nullable: false),
                        Group_GroupID = c.Int(),
                        Classroom_ClassroomID = c.Int(),
                    })
                .PrimaryKey(t => t.StudentID)
                .ForeignKey("dbo.Group", t => t.Group_GroupID)
                .ForeignKey("dbo.Classroom", t => t.Classroom_ClassroomID)
                .Index(t => t.Group_GroupID)
                .Index(t => t.Classroom_ClassroomID);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        NotificationID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Pending = c.Boolean(nullable: false),
                        Student_StudentID = c.Int(),
                        Instructor_InstructorID = c.Int(),
                    })
                .PrimaryKey(t => t.NotificationID)
                .ForeignKey("dbo.Student", t => t.Student_StudentID)
                .ForeignKey("dbo.Instructor", t => t.Instructor_InstructorID)
                .Index(t => t.Student_StudentID)
                .Index(t => t.Instructor_InstructorID);
            
            CreateTable(
                "dbo.Reward",
                c => new
                    {
                        RewardID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Price = c.Double(nullable: false),
                        Student_StudentID = c.Int(),
                        Store_StoreID = c.Int(),
                    })
                .PrimaryKey(t => t.RewardID)
                .ForeignKey("dbo.Student", t => t.Student_StudentID)
                .ForeignKey("dbo.Store", t => t.Store_StoreID)
                .Index(t => t.Student_StudentID)
                .Index(t => t.Store_StoreID);
            
            CreateTable(
                "dbo.Store",
                c => new
                    {
                        StoreID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.StoreID);
            
            CreateTable(
                "dbo.Instructor",
                c => new
                    {
                        InstructorID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ProfileImageURL = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Institution = c.String(),
                    })
                .PrimaryKey(t => t.InstructorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notification", "Instructor_InstructorID", "dbo.Instructor");
            DropForeignKey("dbo.Classroom", "Instructor_InstructorID", "dbo.Instructor");
            DropForeignKey("dbo.Student", "Classroom_ClassroomID", "dbo.Classroom");
            DropForeignKey("dbo.Classroom", "Store_StoreID", "dbo.Store");
            DropForeignKey("dbo.Reward", "Store_StoreID", "dbo.Store");
            DropForeignKey("dbo.Group", "Classroom_ClassroomID", "dbo.Classroom");
            DropForeignKey("dbo.Student", "Group_GroupID", "dbo.Group");
            DropForeignKey("dbo.Reward", "Student_StudentID", "dbo.Student");
            DropForeignKey("dbo.Notification", "Student_StudentID", "dbo.Student");
            DropIndex("dbo.Reward", new[] { "Store_StoreID" });
            DropIndex("dbo.Reward", new[] { "Student_StudentID" });
            DropIndex("dbo.Notification", new[] { "Instructor_InstructorID" });
            DropIndex("dbo.Notification", new[] { "Student_StudentID" });
            DropIndex("dbo.Student", new[] { "Classroom_ClassroomID" });
            DropIndex("dbo.Student", new[] { "Group_GroupID" });
            DropIndex("dbo.Group", new[] { "Classroom_ClassroomID" });
            DropIndex("dbo.Classroom", new[] { "Instructor_InstructorID" });
            DropIndex("dbo.Classroom", new[] { "Store_StoreID" });
            DropTable("dbo.Instructor");
            DropTable("dbo.Store");
            DropTable("dbo.Reward");
            DropTable("dbo.Notification");
            DropTable("dbo.Student");
            DropTable("dbo.Group");
            DropTable("dbo.Classroom");
        }
    }
}
