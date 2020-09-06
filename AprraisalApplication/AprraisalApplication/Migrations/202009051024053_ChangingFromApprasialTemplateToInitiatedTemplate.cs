namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingFromApprasialTemplateToInitiatedTemplate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appraisees", "AppraisalTemplateId", "dbo.AppraisalTemplates");
            DropIndex("dbo.Appraisees", new[] { "AppraisalTemplateId" });
            AddColumn("dbo.Appraisees", "InitiatedAppraisalTemplateId", c => c.Int(nullable: false));
            CreateIndex("dbo.Appraisees", "InitiatedAppraisalTemplateId");
            AddForeignKey("dbo.Appraisees", "InitiatedAppraisalTemplateId", "dbo.InitiatedAppraisalTemplates", "Id", cascadeDelete: true);
            DropColumn("dbo.Appraisees", "AppraisalTemplateId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appraisees", "AppraisalTemplateId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Appraisees", "InitiatedAppraisalTemplateId", "dbo.InitiatedAppraisalTemplates");
            DropIndex("dbo.Appraisees", new[] { "InitiatedAppraisalTemplateId" });
            DropColumn("dbo.Appraisees", "InitiatedAppraisalTemplateId");
            CreateIndex("dbo.Appraisees", "AppraisalTemplateId");
            AddForeignKey("dbo.Appraisees", "AppraisalTemplateId", "dbo.AppraisalTemplates", "Id", cascadeDelete: true);
        }
    }
}
