namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatingTemplateModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppraisalTemplates", "IncludeBdsTracker", c => c.Boolean(nullable: false));
            AddColumn("dbo.InitiatedAppraisalTemplates", "IncludeBdsTracker", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InitiatedAppraisalTemplates", "IncludeBdsTracker");
            DropColumn("dbo.AppraisalTemplates", "IncludeBdsTracker");
        }
    }
}
