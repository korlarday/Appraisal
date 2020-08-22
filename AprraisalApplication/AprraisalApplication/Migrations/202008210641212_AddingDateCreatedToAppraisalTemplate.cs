namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDateCreatedToAppraisalTemplate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppraisalTemplates", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.AppraisalTemplates", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppraisalTemplates", "IsDeleted");
            DropColumn("dbo.AppraisalTemplates", "DateCreated");
        }
    }
}
