namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCareerHistoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CareerHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        TrainingAttended = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: false)
                .Index(t => t.EmployeeId)
                .Index(t => t.DepartmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CareerHistories", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.CareerHistories", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.CareerHistories", new[] { "DepartmentId" });
            DropIndex("dbo.CareerHistories", new[] { "EmployeeId" });
            DropTable("dbo.CareerHistories");
        }
    }
}
