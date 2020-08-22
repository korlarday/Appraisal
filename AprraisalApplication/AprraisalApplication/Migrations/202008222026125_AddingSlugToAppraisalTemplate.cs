namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingSlugToAppraisalTemplate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppraisalTemplates", "Slug", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppraisalTemplates", "Slug");
        }
    }
}
