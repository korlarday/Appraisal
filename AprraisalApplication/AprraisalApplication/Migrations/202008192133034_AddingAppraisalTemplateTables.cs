namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingAppraisalTemplateTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppraisalSectionDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppraisalTemplateSectionId = c.Int(nullable: false),
                        Title1 = c.String(),
                        Title2 = c.String(),
                        MaxScore = c.Int(nullable: false),
                        HasBreakDown = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppraisalTemplateSections", t => t.AppraisalTemplateSectionId, cascadeDelete: true)
                .Index(t => t.AppraisalTemplateSectionId);
            
            CreateTable(
                "dbo.SectionDetailBreakdowns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppraisalSectionDetailId = c.Int(nullable: false),
                        Title = c.String(),
                        MaxScore = c.Int(nullable: false),
                        ExpectedValueId = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExpectedValues", t => t.ExpectedValueId, cascadeDelete: true)
                .ForeignKey("dbo.AppraisalSectionDetails", t => t.AppraisalSectionDetailId, cascadeDelete: true)
                .Index(t => t.AppraisalSectionDetailId)
                .Index(t => t.ExpectedValueId);
            
            CreateTable(
                "dbo.ExpectedValues",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppraisalTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberOfSections = c.Int(nullable: false),
                        TemplateName = c.String(),
                        TemplateRatingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TemplateRatings", t => t.TemplateRatingId, cascadeDelete: true)
                .Index(t => t.TemplateRatingId);
            
            CreateTable(
                "dbo.AppraisalTemplateSections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppraisalTemplateId = c.Int(nullable: false),
                        SectionTypeId = c.Byte(nullable: false),
                        SectionTitle = c.String(),
                        SectionInstructions = c.String(),
                        FirstColumnHeader = c.String(),
                        SecondColumnHeader = c.String(),
                        TotalMarkObtainable = c.Int(nullable: false),
                        TotalPercentageObtainable = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SectionTypes", t => t.SectionTypeId, cascadeDelete: true)
                .ForeignKey("dbo.AppraisalTemplates", t => t.AppraisalTemplateId, cascadeDelete: true)
                .Index(t => t.AppraisalTemplateId)
                .Index(t => t.SectionTypeId);
            
            CreateTable(
                "dbo.SectionTypes",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TemplateRatings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppraisalTemplateId = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                        Rating = c.String(nullable: false, maxLength: 255),
                        Description = c.String(nullable: false),
                        AppraisalTemplate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppraisalTemplates", t => t.AppraisalTemplate_Id)
                .Index(t => t.AppraisalTemplate_Id);
            
            CreateTable(
                "dbo.TemplateSummaryRatings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppraisalTemplateId = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                        Rating = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppraisalTemplates", t => t.AppraisalTemplateId, cascadeDelete: true)
                .Index(t => t.AppraisalTemplateId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TemplateSummaryRatings", "AppraisalTemplateId", "dbo.AppraisalTemplates");
            DropForeignKey("dbo.TemplateRatings", "AppraisalTemplate_Id", "dbo.AppraisalTemplates");
            DropForeignKey("dbo.AppraisalTemplates", "TemplateRatingId", "dbo.TemplateRatings");
            DropForeignKey("dbo.AppraisalTemplateSections", "AppraisalTemplateId", "dbo.AppraisalTemplates");
            DropForeignKey("dbo.AppraisalTemplateSections", "SectionTypeId", "dbo.SectionTypes");
            DropForeignKey("dbo.AppraisalSectionDetails", "AppraisalTemplateSectionId", "dbo.AppraisalTemplateSections");
            DropForeignKey("dbo.SectionDetailBreakdowns", "AppraisalSectionDetailId", "dbo.AppraisalSectionDetails");
            DropForeignKey("dbo.SectionDetailBreakdowns", "ExpectedValueId", "dbo.ExpectedValues");
            DropIndex("dbo.TemplateSummaryRatings", new[] { "AppraisalTemplateId" });
            DropIndex("dbo.TemplateRatings", new[] { "AppraisalTemplate_Id" });
            DropIndex("dbo.AppraisalTemplateSections", new[] { "SectionTypeId" });
            DropIndex("dbo.AppraisalTemplateSections", new[] { "AppraisalTemplateId" });
            DropIndex("dbo.AppraisalTemplates", new[] { "TemplateRatingId" });
            DropIndex("dbo.SectionDetailBreakdowns", new[] { "ExpectedValueId" });
            DropIndex("dbo.SectionDetailBreakdowns", new[] { "AppraisalSectionDetailId" });
            DropIndex("dbo.AppraisalSectionDetails", new[] { "AppraisalTemplateSectionId" });
            DropTable("dbo.TemplateSummaryRatings");
            DropTable("dbo.TemplateRatings");
            DropTable("dbo.SectionTypes");
            DropTable("dbo.AppraisalTemplateSections");
            DropTable("dbo.AppraisalTemplates");
            DropTable("dbo.ExpectedValues");
            DropTable("dbo.SectionDetailBreakdowns");
            DropTable("dbo.AppraisalSectionDetails");
        }
    }
}
