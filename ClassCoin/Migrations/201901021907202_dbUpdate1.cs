namespace ClassCoin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbUpdate1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reward", "Student_StudentID", "dbo.Student");
            DropForeignKey("dbo.Group", "Classroom_ClassroomID", "dbo.Classroom");
            DropForeignKey("dbo.Instructor", "Classroom_ClassroomID", "dbo.Classroom");
            DropForeignKey("dbo.Student", "Classroom_ClassroomID", "dbo.Classroom");
            DropIndex("dbo.Group", new[] { "Classroom_ClassroomID" });
            DropIndex("dbo.Student", new[] { "Classroom_ClassroomID" });
            DropIndex("dbo.Reward", new[] { "Student_StudentID" });
            DropIndex("dbo.Instructor", new[] { "Classroom_ClassroomID" });
            CreateTable(
                "dbo.GroupClassroom",
                c => new
                    {
                        Group_GroupID = c.Int(nullable: false),
                        Classroom_ClassroomID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_GroupID, t.Classroom_ClassroomID })
                .ForeignKey("dbo.Group", t => t.Group_GroupID, cascadeDelete: true)
                .ForeignKey("dbo.Classroom", t => t.Classroom_ClassroomID, cascadeDelete: true)
                .Index(t => t.Group_GroupID)
                .Index(t => t.Classroom_ClassroomID);
            
            CreateTable(
                "dbo.StudentClassroom",
                c => new
                    {
                        Student_StudentID = c.Int(nullable: false),
                        Classroom_ClassroomID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_StudentID, t.Classroom_ClassroomID })
                .ForeignKey("dbo.Student", t => t.Student_StudentID, cascadeDelete: true)
                .ForeignKey("dbo.Classroom", t => t.Classroom_ClassroomID, cascadeDelete: true)
                .Index(t => t.Student_StudentID)
                .Index(t => t.Classroom_ClassroomID);
            
            CreateTable(
                "dbo.RewardStudent",
                c => new
                    {
                        Reward_RewardID = c.Int(nullable: false),
                        Student_StudentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reward_RewardID, t.Student_StudentID })
                .ForeignKey("dbo.Reward", t => t.Reward_RewardID, cascadeDelete: true)
                .ForeignKey("dbo.Student", t => t.Student_StudentID, cascadeDelete: true)
                .Index(t => t.Reward_RewardID)
                .Index(t => t.Student_StudentID);
            
            CreateTable(
                "dbo.InstructorClassroom",
                c => new
                    {
                        Instructor_InstructorID = c.Int(nullable: false),
                        Classroom_ClassroomID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Instructor_InstructorID, t.Classroom_ClassroomID })
                .ForeignKey("dbo.Instructor", t => t.Instructor_InstructorID, cascadeDelete: true)
                .ForeignKey("dbo.Classroom", t => t.Classroom_ClassroomID, cascadeDelete: true)
                .Index(t => t.Instructor_InstructorID)
                .Index(t => t.Classroom_ClassroomID);
            
            DropColumn("dbo.Group", "Classroom_ClassroomID");
            DropColumn("dbo.Student", "Classroom_ClassroomID");
            DropColumn("dbo.Reward", "Student_StudentID");
            DropColumn("dbo.Instructor", "Classroom_ClassroomID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Instructor", "Classroom_ClassroomID", c => c.Int());
            AddColumn("dbo.Reward", "Student_StudentID", c => c.Int());
            AddColumn("dbo.Student", "Classroom_ClassroomID", c => c.Int());
            AddColumn("dbo.Group", "Classroom_ClassroomID", c => c.Int());
            DropForeignKey("dbo.InstructorClassroom", "Classroom_ClassroomID", "dbo.Classroom");
            DropForeignKey("dbo.InstructorClassroom", "Instructor_InstructorID", "dbo.Instructor");
            DropForeignKey("dbo.RewardStudent", "Student_StudentID", "dbo.Student");
            DropForeignKey("dbo.RewardStudent", "Reward_RewardID", "dbo.Reward");
            DropForeignKey("dbo.StudentClassroom", "Classroom_ClassroomID", "dbo.Classroom");
            DropForeignKey("dbo.StudentClassroom", "Student_StudentID", "dbo.Student");
            DropForeignKey("dbo.GroupClassroom", "Classroom_ClassroomID", "dbo.Classroom");
            DropForeignKey("dbo.GroupClassroom", "Group_GroupID", "dbo.Group");
            DropIndex("dbo.InstructorClassroom", new[] { "Classroom_ClassroomID" });
            DropIndex("dbo.InstructorClassroom", new[] { "Instructor_InstructorID" });
            DropIndex("dbo.RewardStudent", new[] { "Student_StudentID" });
            DropIndex("dbo.RewardStudent", new[] { "Reward_RewardID" });
            DropIndex("dbo.StudentClassroom", new[] { "Classroom_ClassroomID" });
            DropIndex("dbo.StudentClassroom", new[] { "Student_StudentID" });
            DropIndex("dbo.GroupClassroom", new[] { "Classroom_ClassroomID" });
            DropIndex("dbo.GroupClassroom", new[] { "Group_GroupID" });
            DropTable("dbo.InstructorClassroom");
            DropTable("dbo.RewardStudent");
            DropTable("dbo.StudentClassroom");
            DropTable("dbo.GroupClassroom");
            CreateIndex("dbo.Instructor", "Classroom_ClassroomID");
            CreateIndex("dbo.Reward", "Student_StudentID");
            CreateIndex("dbo.Student", "Classroom_ClassroomID");
            CreateIndex("dbo.Group", "Classroom_ClassroomID");
            AddForeignKey("dbo.Student", "Classroom_ClassroomID", "dbo.Classroom", "ClassroomID");
            AddForeignKey("dbo.Instructor", "Classroom_ClassroomID", "dbo.Classroom", "ClassroomID");
            AddForeignKey("dbo.Group", "Classroom_ClassroomID", "dbo.Classroom", "ClassroomID");
            AddForeignKey("dbo.Reward", "Student_StudentID", "dbo.Student", "StudentID");
        }
    }
}
