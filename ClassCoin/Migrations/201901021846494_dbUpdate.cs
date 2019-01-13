namespace ClassCoin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notification", "Student_StudentID", "dbo.Student");
            DropForeignKey("dbo.Classroom", "Instructor_InstructorID", "dbo.Instructor");
            DropForeignKey("dbo.Notification", "Instructor_InstructorID", "dbo.Instructor");
            DropIndex("dbo.Classroom", new[] { "Instructor_InstructorID" });
            DropIndex("dbo.Notification", new[] { "Student_StudentID" });
            DropIndex("dbo.Notification", new[] { "Instructor_InstructorID" });
            AddColumn("dbo.Group", "GroupName", c => c.String());
            AddColumn("dbo.Student", "UserID", c => c.Int(nullable: false));
            AddColumn("dbo.Instructor", "UserID", c => c.Int(nullable: false));
            AddColumn("dbo.Instructor", "Classroom_ClassroomID", c => c.Int());
            CreateIndex("dbo.Instructor", "Classroom_ClassroomID");
            AddForeignKey("dbo.Instructor", "Classroom_ClassroomID", "dbo.Classroom", "ClassroomID");
            DropColumn("dbo.Classroom", "Instructor_InstructorID");
            DropColumn("dbo.Group", "Name");
            DropColumn("dbo.Student", "Name");
            DropColumn("dbo.Student", "ProfileImageURL");
            DropColumn("dbo.Student", "UserName");
            DropColumn("dbo.Student", "Password");
            DropColumn("dbo.Notification", "Student_StudentID");
            DropColumn("dbo.Notification", "Instructor_InstructorID");
            DropColumn("dbo.Instructor", "Name");
            DropColumn("dbo.Instructor", "ProfileImageURL");
            DropColumn("dbo.Instructor", "Email");
            DropColumn("dbo.Instructor", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Instructor", "Password", c => c.String());
            AddColumn("dbo.Instructor", "Email", c => c.String());
            AddColumn("dbo.Instructor", "ProfileImageURL", c => c.String());
            AddColumn("dbo.Instructor", "Name", c => c.String());
            AddColumn("dbo.Notification", "Instructor_InstructorID", c => c.Int());
            AddColumn("dbo.Notification", "Student_StudentID", c => c.Int());
            AddColumn("dbo.Student", "Password", c => c.String());
            AddColumn("dbo.Student", "UserName", c => c.String());
            AddColumn("dbo.Student", "ProfileImageURL", c => c.String());
            AddColumn("dbo.Student", "Name", c => c.String());
            AddColumn("dbo.Group", "Name", c => c.String());
            AddColumn("dbo.Classroom", "Instructor_InstructorID", c => c.Int());
            DropForeignKey("dbo.Instructor", "Classroom_ClassroomID", "dbo.Classroom");
            DropIndex("dbo.Instructor", new[] { "Classroom_ClassroomID" });
            DropColumn("dbo.Instructor", "Classroom_ClassroomID");
            DropColumn("dbo.Instructor", "UserID");
            DropColumn("dbo.Student", "UserID");
            DropColumn("dbo.Group", "GroupName");
            CreateIndex("dbo.Notification", "Instructor_InstructorID");
            CreateIndex("dbo.Notification", "Student_StudentID");
            CreateIndex("dbo.Classroom", "Instructor_InstructorID");
            AddForeignKey("dbo.Notification", "Instructor_InstructorID", "dbo.Instructor", "InstructorID");
            AddForeignKey("dbo.Classroom", "Instructor_InstructorID", "dbo.Instructor", "InstructorID");
            AddForeignKey("dbo.Notification", "Student_StudentID", "dbo.Student", "StudentID");
        }
    }
}
