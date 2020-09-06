namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingInitiatedAppraisalTemplate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SectionResults", "AppraisalTemplateSectionId", "dbo.AppraisalTemplateSections");
            DropForeignKey("dbo.SectionDetailResults", "AppraisalSectionDetailId", "dbo.AppraisalSectionDetails");
            DropIndex("dbo.SectionResults", new[] { "AppraisalTemplateSectionId" });
            DropIndex("dbo.SectionDetailResults", new[] { "AppraisalSectionDetailId" });
            CreateTable(
                "dbo.InitiatedTemplateSections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InitiatedAppraisalTemplateId = c.Int(nullable: false),
                        SectionTypeId = c.Byte(nullable: false),
                        SectionTitle = c.String(),
                        SectionInstructions = c.String(),
                        FirstColumnHeader = c.String(),
                        SecondColumnHeader = c.String(),
                        TotalMarkObtainable = c.Int(nullable: false),
                        TotalPercentageObtainable = c.Int(nullable: false),
                        Optional = c.Boolean(nullable: false),
                        SetupId = c.Int(nullable: false),
                        DerivedSectionSetupId = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        BreakdownValueBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SectionTypes", t => t.SectionTypeId, cascadeDelete: true)
                .ForeignKey("dbo.InitiatedAppraisalTemplates", t => t.InitiatedAppraisalTemplateId, cascadeDelete: true)
                .Index(t => t.InitiatedAppraisalTemplateId)
                .Index(t => t.SectionTypeId);
            
            CreateTable(
                "dbo.InitiatedSectionDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InitiatedTemplateSectionId = c.Int(nullable: false),
                        Title1 = c.String(),
                        Title2 = c.String(),
                        MaxScore = c.Int(nullable: false),
                        HasBreakDown = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InitiatedTemplateSections", t => t.InitiatedTemplateSectionId, cascadeDelete: true)
                .Index(t => t.InitiatedTemplateSectionId);
            
            CreateTable(
                "dbo.InitiatedSectionDetailBreakdowns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppraisalSectionDetailId = c.Int(nullable: false),
                        Title = c.String(),
                        MaxScore = c.Int(nullable: false),
                        ExpectedValueId = c.Byte(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        InitiatedSectionDetail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExpectedValues", t => t.ExpectedValueId, cascadeDelete: true)
                .ForeignKey("dbo.InitiatedSectionDetails", t => t.InitiatedSectionDetail_Id)
                .Index(t => t.ExpectedValueId)
                .Index(t => t.InitiatedSectionDetail_Id);
            
            CreateTable(
                "dbo.InitiatedAppraisalTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NewAppraiserId = c.Int(nullable: false),
                        AppraisalTemplateId = c.Int(nullable: false),
                        NumberOfSections = c.Int(nullable: false),
                        TemplateName = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Slug = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TemplateRatings", "InitiatedAppraisalTemplate_Id", c => c.Int());
            AddColumn("dbo.TemplateSummaryRatings", "InitiatedAppraisalTemplate_Id", c => c.Int());
            AddColumn("dbo.SectionResults", "InitiatedTemplateSectionId", c => c.Int(nullable: false));
            AddColumn("dbo.SectionDetailResults", "InitiatedSectionDetailId", c => c.Int(nullable: false));
            AddColumn("dbo.ItemBreakdownResults", "InitiatedSectionDetailBreakdownId", c => c.Int(nullable: false));
            CreateIndex("dbo.TemplateRatings", "InitiatedAppraisalTemplate_Id");
            CreateIndex("dbo.TemplateSummaryRatings", "InitiatedAppraisalTemplate_Id");
            CreateIndex("dbo.SectionResults", "InitiatedTemplateSectionId");
            CreateIndex("dbo.SectionDetailResults", "InitiatedSectionDetailId");
            CreateIndex("dbo.ItemBreakdownResults", "InitiatedSectionDetailBreakdownId");
            AddForeignKey("dbo.SectionResults", "InitiatedTemplateSectionId", "dbo.InitiatedTemplateSections", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SectionDetailResults", "InitiatedSectionDetailId", "dbo.InitiatedSectionDetails", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ItemBreakdownResults", "InitiatedSectionDetailBreakdownId", "dbo.InitiatedSectionDetailBreakdowns", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TemplateRatings", "InitiatedAppraisalTemplate_Id", "dbo.InitiatedAppraisalTemplates", "Id");
            AddForeignKey("dbo.TemplateSummaryRatings", "InitiatedAppraisalTemplate_Id", "dbo.InitiatedAppraisalTemplates", "Id");
            DropColumn("dbo.SectionResults", "AppraisalTemplateSectionId");
            DropColumn("dbo.SectionDetailResults", "AppraisalSectionDetailId");
            DropColumn("dbo.ItemBreakdownResults", "ItemBreakDown_BreakdownId");
            DropColumn("dbo.ItemBreakdownResults", "ItemBreakDown_ItemText");
            DropColumn("dbo.ItemBreakdownResults", "ItemBreakDown_ValueType");
            DropColumn("dbo.ItemBreakdownResults", "ItemBreakDownId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ItemBreakdownResults", "ItemBreakDownId", c => c.Int(nullable: false));
            AddColumn("dbo.ItemBreakdownResults", "ItemBreakDown_ValueType", c => c.String());
            AddColumn("dbo.ItemBreakdownResults", "ItemBreakDown_ItemText", c => c.String());
            AddColumn("dbo.ItemBreakdownResults", "ItemBreakDown_BreakdownId", c => c.Int());
            AddColumn("dbo.SectionDetailResults", "AppraisalSectionDetailId", c => c.Int(nullable: false));
            AddColumn("dbo.SectionResults", "AppraisalTemplateSectionId", c => c.Int(nullable: false));
            DropForeignKey("dbo.TemplateSummaryRatings", "InitiatedAppraisalTemplate_Id", "dbo.InitiatedAppraisalTemplates");
            DropForeignKey("dbo.TemplateRatings", "InitiatedAppraisalTemplate_Id", "dbo.InitiatedAppraisalTemplates");
            DropForeignKey("dbo.InitiatedTemplateSections", "InitiatedAppraisalTemplateId", "dbo.InitiatedAppraisalTemplates");
            DropForeignKey("dbo.ItemBreakdownResults", "InitiatedSectionDetailBreakdownId", "dbo.InitiatedSectionDetailBreakdowns");
            DropForeignKey("dbo.SectionDetailResults", "InitiatedSectionDetailId", "dbo.InitiatedSectionDetails");
            DropForeignKey("dbo.SectionResults", "InitiatedTemplateSectionId", "dbo.InitiatedTemplateSections");
            DropForeignKey("dbo.InitiatedTemplateSections", "SectionTypeId", "dbo.SectionTypes");
            DropForeignKey("dbo.InitiatedSectionDetails", "InitiatedTemplateSectionId", "dbo.InitiatedTemplateSections");
            DropForeignKey("dbo.InitiatedSectionDetailBreakdowns", "InitiatedSectionDetail_Id", "dbo.InitiatedSectionDetails");
            DropForeignKey("dbo.InitiatedSectionDetailBreakdowns", "ExpectedValueId", "dbo.ExpectedValues");
            DropIndex("dbo.ItemBreakdownResults", new[] { "InitiatedSectionDetailBreakdownId" });
            DropIndex("dbo.SectionDetailResults", new[] { "InitiatedSectionDetailId" });
            DropIndex("dbo.InitiatedSectionDetailBreakdowns", new[] { "InitiatedSectionDetail_Id" });
            DropIndex("dbo.InitiatedSectionDetailBreakdowns", new[] { "ExpectedValueId" });
            DropIndex("dbo.InitiatedSectionDetails", new[] { "InitiatedTemplateSectionId" });
            DropIndex("dbo.InitiatedTemplateSections", new[] { "SectionTypeId" });
            DropIndex("dbo.InitiatedTemplateSections", new[] { "InitiatedAppraisalTemplateId" });
            DropIndex("dbo.SectionResults", new[] { "InitiatedTemplateSectionId" });
            DropIndex("dbo.TemplateSummaryRatings", new[] { "InitiatedAppraisalTemplate_Id" });
            DropIndex("dbo.TemplateRatings", new[] { "InitiatedAppraisalTemplate_Id" });
            DropColumn("dbo.ItemBreakdownResults", "InitiatedSectionDetailBreakdownId");
            DropColumn("dbo.SectionDetailResults", "InitiatedSectionDetailId");
            DropColumn("dbo.SectionResults", "InitiatedTemplateSectionId");
            DropColumn("dbo.TemplateSummaryRatings", "InitiatedAppraisalTemplate_Id");
            DropColumn("dbo.TemplateRatings", "InitiatedAppraisalTemplate_Id");
            DropTable("dbo.InitiatedAppraisalTemplates");
            DropTable("dbo.InitiatedSectionDetailBreakdowns");
            DropTable("dbo.InitiatedSectionDetails");
            DropTable("dbo.InitiatedTemplateSections");
            CreateIndex("dbo.SectionDetailResults", "AppraisalSectionDetailId");
            CreateIndex("dbo.SectionResults", "AppraisalTemplateSectionId");
            AddForeignKey("dbo.SectionDetailResults", "AppraisalSectionDetailId", "dbo.AppraisalSectionDetails", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SectionResults", "AppraisalTemplateSectionId", "dbo.AppraisalTemplateSections", "Id", cascadeDelete: true);
        }
    }
}
