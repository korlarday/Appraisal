namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingOptionOptionalInAppraisalSections : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AppraisalTemplates", "TemplateRatingId", "dbo.TemplateRatings");
            DropForeignKey("dbo.TemplateRatings", "AppraisalTemplate_Id", "dbo.AppraisalTemplates");
            DropIndex("dbo.AppraisalTemplates", new[] { "TemplateRatingId" });
            DropIndex("dbo.TemplateRatings", new[] { "AppraisalTemplate_Id" });
            DropColumn("dbo.TemplateRatings", "AppraisalTemplateId");
            RenameColumn(table: "dbo.TemplateRatings", name: "AppraisalTemplate_Id", newName: "AppraisalTemplateId");
            AddColumn("dbo.AppraisalTemplateSections", "Optional", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppraisalTemplateSections", "SetupId", c => c.Int(nullable: false));
            AddColumn("dbo.AppraisalTemplateSections", "DerivedSectionSetupId", c => c.Int());
            AlterColumn("dbo.TemplateRatings", "AppraisalTemplateId", c => c.Int(nullable: false));
            CreateIndex("dbo.TemplateRatings", "AppraisalTemplateId");
            AddForeignKey("dbo.TemplateRatings", "AppraisalTemplateId", "dbo.AppraisalTemplates", "Id", cascadeDelete: true);
            DropColumn("dbo.AppraisalTemplates", "TemplateRatingId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppraisalTemplates", "TemplateRatingId", c => c.Int(nullable: false));
            DropForeignKey("dbo.TemplateRatings", "AppraisalTemplateId", "dbo.AppraisalTemplates");
            DropIndex("dbo.TemplateRatings", new[] { "AppraisalTemplateId" });
            AlterColumn("dbo.TemplateRatings", "AppraisalTemplateId", c => c.Int());
            DropColumn("dbo.AppraisalTemplateSections", "DerivedSectionSetupId");
            DropColumn("dbo.AppraisalTemplateSections", "SetupId");
            DropColumn("dbo.AppraisalTemplateSections", "Optional");
            RenameColumn(table: "dbo.TemplateRatings", name: "AppraisalTemplateId", newName: "AppraisalTemplate_Id");
            AddColumn("dbo.TemplateRatings", "AppraisalTemplateId", c => c.Int(nullable: false));
            CreateIndex("dbo.TemplateRatings", "AppraisalTemplate_Id");
            CreateIndex("dbo.AppraisalTemplates", "TemplateRatingId");
            AddForeignKey("dbo.TemplateRatings", "AppraisalTemplate_Id", "dbo.AppraisalTemplates", "Id");
            AddForeignKey("dbo.AppraisalTemplates", "TemplateRatingId", "dbo.TemplateRatings", "Id", cascadeDelete: true);
        }
    }
}
