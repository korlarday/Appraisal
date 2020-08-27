namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDepartmentAndUserAppraisalTemplate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppraisalDepartmentTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DepartmentId = c.Int(nullable: false),
                        AppraisalTemplateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppraisalTemplates", t => t.AppraisalTemplateId, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.DepartmentId)
                .Index(t => t.AppraisalTemplateId);
            
            CreateTable(
                "dbo.AppraisalUserTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        AppraisalTemplateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.AppraisalTemplates", t => t.AppraisalTemplateId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.AppraisalTemplateId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppraisalUserTemplates", "AppraisalTemplateId", "dbo.AppraisalTemplates");
            DropForeignKey("dbo.AppraisalUserTemplates", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppraisalDepartmentTemplates", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.AppraisalDepartmentTemplates", "AppraisalTemplateId", "dbo.AppraisalTemplates");
            DropIndex("dbo.AppraisalUserTemplates", new[] { "AppraisalTemplateId" });
            DropIndex("dbo.AppraisalUserTemplates", new[] { "ApplicationUserId" });
            DropIndex("dbo.AppraisalDepartmentTemplates", new[] { "AppraisalTemplateId" });
            DropIndex("dbo.AppraisalDepartmentTemplates", new[] { "DepartmentId" });
            DropTable("dbo.AppraisalUserTemplates");
            DropTable("dbo.AppraisalDepartmentTemplates");
        }
    }
}
