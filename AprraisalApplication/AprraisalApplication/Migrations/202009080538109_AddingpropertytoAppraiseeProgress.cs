namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingpropertytoAppraiseeProgress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppraiseeProgresses", "FeedbackFromAppraisee", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppraiseeProgresses", "FeedbackFromAppraisee");
        }
    }
}
