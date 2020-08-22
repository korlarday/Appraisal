namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDescriptionToAppraisalTemplate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppraisalTemplates", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppraisalTemplates", "Description");
        }
    }
}
