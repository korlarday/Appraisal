namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatingBdsTrackerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BdsPerformanceTrackers", "FundingAchievedPercentage", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BdsPerformanceTrackers", "FundingAchievedPercentage");
        }
    }
}
