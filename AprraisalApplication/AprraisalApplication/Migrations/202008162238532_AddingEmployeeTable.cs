namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingEmployeeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeQualifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        QualificationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Qualifications", t => t.QualificationId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.QualificationId);
            
            CreateTable(
                "dbo.Qualifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.Int(nullable: false),
                        Firstname = c.String(nullable: false, maxLength: 255),
                        Lastname = c.String(nullable: false, maxLength: 255),
                        Othername = c.String(maxLength: 255),
                        StateId = c.Int(nullable: false),
                        GenderId = c.Byte(nullable: false),
                        BranchId = c.Int(nullable: false),
                        JobTitleId = c.Int(nullable: false),
                        DateOfEmployment = c.DateTime(nullable: false),
                        DateOfLastPromotion = c.DateTime(),
                        TitleId = c.Byte(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Branches", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("dbo.Genders", t => t.GenderId, cascadeDelete: true)
                .ForeignKey("dbo.JobTitles", t => t.JobTitleId, cascadeDelete: true)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: false)
                .ForeignKey("dbo.Titles", t => t.TitleId, cascadeDelete: true)
                .Index(t => t.StateId)
                .Index(t => t.GenderId)
                .Index(t => t.BranchId)
                .Index(t => t.JobTitleId)
                .Index(t => t.TitleId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JobTitles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Titles",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "EmployeeId", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "TitleId", "dbo.Titles");
            DropForeignKey("dbo.Employees", "StateId", "dbo.States");
            DropForeignKey("dbo.Employees", "JobTitleId", "dbo.JobTitles");
            DropForeignKey("dbo.Employees", "GenderId", "dbo.Genders");
            DropForeignKey("dbo.EmployeeQualifications", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Employees", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EmployeeQualifications", "QualificationId", "dbo.Qualifications");
            DropIndex("dbo.Employees", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Employees", new[] { "TitleId" });
            DropIndex("dbo.Employees", new[] { "JobTitleId" });
            DropIndex("dbo.Employees", new[] { "BranchId" });
            DropIndex("dbo.Employees", new[] { "GenderId" });
            DropIndex("dbo.Employees", new[] { "StateId" });
            DropIndex("dbo.EmployeeQualifications", new[] { "QualificationId" });
            DropIndex("dbo.EmployeeQualifications", new[] { "EmployeeId" });
            DropColumn("dbo.AspNetUsers", "EmployeeId");
            DropTable("dbo.Titles");
            DropTable("dbo.JobTitles");
            DropTable("dbo.Genders");
            DropTable("dbo.Employees");
            DropTable("dbo.Qualifications");
            DropTable("dbo.EmployeeQualifications");
        }
    }
}
