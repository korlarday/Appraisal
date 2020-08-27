namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingAppraisalProcessTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppraisalDepartments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NewAppraisalId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.NewAppraisals", t => t.NewAppraisalId, cascadeDelete: true)
                .Index(t => t.NewAppraisalId)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.AppraisalLocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NewAppraisalId = c.Int(nullable: false),
                        StateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: true)
                .ForeignKey("dbo.NewAppraisals", t => t.NewAppraisalId, cascadeDelete: true)
                .Index(t => t.NewAppraisalId)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.AppraisalStaffs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NewAppraisalStaff = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        IsCompleted = c.Boolean(nullable: false),
                        NewAppraisal_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.NewAppraisals", t => t.NewAppraisal_Id)
                .Index(t => t.EmployeeId)
                .Index(t => t.NewAppraisal_Id);
            
            CreateTable(
                "dbo.AppraisalTypes",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppraiseeCareerHistoryWithCompanies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        AppraiseeId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        GradeId = c.Int(nullable: false),
                        TrainingAttended = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Grades", t => t.GradeId, cascadeDelete: true)
                .ForeignKey("dbo.Appraisees", t => t.AppraiseeId, cascadeDelete: true)
                .Index(t => t.AppraiseeId)
                .Index(t => t.DepartmentId)
                .Index(t => t.GradeId);
            
            CreateTable(
                "dbo.AppraiseePersonalDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        GradeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Grades", t => t.GradeId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.GradeId);
            
            CreateTable(
                "dbo.Appraisees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NewAppraisalId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        AppraiseePersonalDataId = c.Int(nullable: false),
                        AppraiserPersonalDataId = c.Int(nullable: false),
                        AppraisalTemplateId = c.Int(nullable: false),
                        IsNew = c.Boolean(nullable: false),
                        IsCompleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppraisalTemplates", t => t.AppraisalTemplateId, cascadeDelete: true)
                .ForeignKey("dbo.AppraiseePersonalDatas", t => t.AppraiseePersonalDataId, cascadeDelete: false)
                .ForeignKey("dbo.AppraiserPersonalDatas", t => t.AppraiserPersonalDataId, cascadeDelete: false)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: false)
                .ForeignKey("dbo.NewAppraisals", t => t.NewAppraisalId, cascadeDelete: true)
                .Index(t => t.NewAppraisalId)
                .Index(t => t.EmployeeId)
                .Index(t => t.AppraiseePersonalDataId)
                .Index(t => t.AppraiserPersonalDataId)
                .Index(t => t.AppraisalTemplateId);
            
            CreateTable(
                "dbo.AppraiserPersonalDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppraiserId = c.Int(nullable: false),
                        JobTitleId = c.Int(nullable: false),
                        GradeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.AppraiserId, cascadeDelete: true)
                .ForeignKey("dbo.Grades", t => t.GradeId, cascadeDelete: true)
                .ForeignKey("dbo.JobTitles", t => t.JobTitleId, cascadeDelete: false)
                .Index(t => t.AppraiserId)
                .Index(t => t.JobTitleId)
                .Index(t => t.GradeId);
            
            CreateTable(
                "dbo.SectionResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppraieeId = c.Int(nullable: false),
                        AppraisalTemplateSectionId = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                        Appraisee_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppraisalTemplateSections", t => t.AppraisalTemplateSectionId, cascadeDelete: true)
                .ForeignKey("dbo.Appraisees", t => t.Appraisee_Id)
                .Index(t => t.AppraisalTemplateSectionId)
                .Index(t => t.Appraisee_Id);
            
            CreateTable(
                "dbo.SectionDetailResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SectionResultId = c.Int(nullable: false),
                        AppraisalSectionDetailId = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppraisalSectionDetails", t => t.AppraisalSectionDetailId, cascadeDelete: true)
                .ForeignKey("dbo.SectionResults", t => t.SectionResultId, cascadeDelete: false)
                .Index(t => t.SectionResultId)
                .Index(t => t.AppraisalSectionDetailId);
            
            CreateTable(
                "dbo.ItemBreakdownResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SectionDetailResultId = c.Int(nullable: false),
                        ItemBreakDown_BreakdownId = c.Int(),
                        ItemBreakDown_ItemText = c.String(),
                        ItemBreakDown_ValueType = c.String(),
                        ItemBreakDownId = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SectionDetailResults", t => t.SectionDetailResultId, cascadeDelete: true)
                .Index(t => t.SectionDetailResultId);
            
            CreateTable(
                "dbo.NewAppraisals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InitiatedById = c.String(maxLength: 128),
                        DueDate = c.DateTime(),
                        AppraisalTypeId = c.Byte(nullable: false),
                        AppraisalPeriodStartDate = c.DateTime(nullable: false),
                        AppraisalPeriodEndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppraisalTypes", t => t.AppraisalTypeId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.InitiatedById)
                .Index(t => t.InitiatedById)
                .Index(t => t.AppraisalTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NewAppraisals", "InitiatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appraisees", "NewAppraisalId", "dbo.NewAppraisals");
            DropForeignKey("dbo.NewAppraisals", "AppraisalTypeId", "dbo.AppraisalTypes");
            DropForeignKey("dbo.AppraisalStaffs", "NewAppraisal_Id", "dbo.NewAppraisals");
            DropForeignKey("dbo.AppraisalLocations", "NewAppraisalId", "dbo.NewAppraisals");
            DropForeignKey("dbo.AppraisalDepartments", "NewAppraisalId", "dbo.NewAppraisals");
            DropForeignKey("dbo.SectionResults", "Appraisee_Id", "dbo.Appraisees");
            DropForeignKey("dbo.SectionDetailResults", "SectionResultId", "dbo.SectionResults");
            DropForeignKey("dbo.ItemBreakdownResults", "SectionDetailResultId", "dbo.SectionDetailResults");
            DropForeignKey("dbo.SectionDetailResults", "AppraisalSectionDetailId", "dbo.AppraisalSectionDetails");
            DropForeignKey("dbo.SectionResults", "AppraisalTemplateSectionId", "dbo.AppraisalTemplateSections");
            DropForeignKey("dbo.Appraisees", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Appraisees", "AppraiserPersonalDataId", "dbo.AppraiserPersonalDatas");
            DropForeignKey("dbo.AppraiserPersonalDatas", "JobTitleId", "dbo.JobTitles");
            DropForeignKey("dbo.AppraiserPersonalDatas", "GradeId", "dbo.Grades");
            DropForeignKey("dbo.AppraiserPersonalDatas", "AppraiserId", "dbo.Employees");
            DropForeignKey("dbo.Appraisees", "AppraiseePersonalDataId", "dbo.AppraiseePersonalDatas");
            DropForeignKey("dbo.AppraiseeCareerHistoryWithCompanies", "AppraiseeId", "dbo.Appraisees");
            DropForeignKey("dbo.Appraisees", "AppraisalTemplateId", "dbo.AppraisalTemplates");
            DropForeignKey("dbo.AppraiseePersonalDatas", "GradeId", "dbo.Grades");
            DropForeignKey("dbo.AppraiseePersonalDatas", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.AppraiseeCareerHistoryWithCompanies", "GradeId", "dbo.Grades");
            DropForeignKey("dbo.AppraiseeCareerHistoryWithCompanies", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.AppraisalStaffs", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.AppraisalLocations", "StateId", "dbo.States");
            DropForeignKey("dbo.AppraisalDepartments", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.NewAppraisals", new[] { "AppraisalTypeId" });
            DropIndex("dbo.NewAppraisals", new[] { "InitiatedById" });
            DropIndex("dbo.ItemBreakdownResults", new[] { "SectionDetailResultId" });
            DropIndex("dbo.SectionDetailResults", new[] { "AppraisalSectionDetailId" });
            DropIndex("dbo.SectionDetailResults", new[] { "SectionResultId" });
            DropIndex("dbo.SectionResults", new[] { "Appraisee_Id" });
            DropIndex("dbo.SectionResults", new[] { "AppraisalTemplateSectionId" });
            DropIndex("dbo.AppraiserPersonalDatas", new[] { "GradeId" });
            DropIndex("dbo.AppraiserPersonalDatas", new[] { "JobTitleId" });
            DropIndex("dbo.AppraiserPersonalDatas", new[] { "AppraiserId" });
            DropIndex("dbo.Appraisees", new[] { "AppraisalTemplateId" });
            DropIndex("dbo.Appraisees", new[] { "AppraiserPersonalDataId" });
            DropIndex("dbo.Appraisees", new[] { "AppraiseePersonalDataId" });
            DropIndex("dbo.Appraisees", new[] { "EmployeeId" });
            DropIndex("dbo.Appraisees", new[] { "NewAppraisalId" });
            DropIndex("dbo.AppraiseePersonalDatas", new[] { "GradeId" });
            DropIndex("dbo.AppraiseePersonalDatas", new[] { "EmployeeId" });
            DropIndex("dbo.AppraiseeCareerHistoryWithCompanies", new[] { "GradeId" });
            DropIndex("dbo.AppraiseeCareerHistoryWithCompanies", new[] { "DepartmentId" });
            DropIndex("dbo.AppraiseeCareerHistoryWithCompanies", new[] { "AppraiseeId" });
            DropIndex("dbo.AppraisalStaffs", new[] { "NewAppraisal_Id" });
            DropIndex("dbo.AppraisalStaffs", new[] { "EmployeeId" });
            DropIndex("dbo.AppraisalLocations", new[] { "StateId" });
            DropIndex("dbo.AppraisalLocations", new[] { "NewAppraisalId" });
            DropIndex("dbo.AppraisalDepartments", new[] { "DepartmentId" });
            DropIndex("dbo.AppraisalDepartments", new[] { "NewAppraisalId" });
            DropTable("dbo.NewAppraisals");
            DropTable("dbo.ItemBreakdownResults");
            DropTable("dbo.SectionDetailResults");
            DropTable("dbo.SectionResults");
            DropTable("dbo.AppraiserPersonalDatas");
            DropTable("dbo.Appraisees");
            DropTable("dbo.AppraiseePersonalDatas");
            DropTable("dbo.AppraiseeCareerHistoryWithCompanies");
            DropTable("dbo.AppraisalTypes");
            DropTable("dbo.AppraisalStaffs");
            DropTable("dbo.AppraisalLocations");
            DropTable("dbo.AppraisalDepartments");
        }
    }
}
