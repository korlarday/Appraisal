namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSupervisorAskforfeedback : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppraiseeProgresses", "SupervisorAskForFeedback", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppraiseeProgresses", "SupervisorAskForFeedback");
        }
    }
}
