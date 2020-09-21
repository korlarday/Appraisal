namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingBDSTrackerToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BdsPerformanceTrackers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnnualTarget = c.Int(nullable: false),
                        ExpectedRSA = c.Int(nullable: false),
                        RSAAchieved = c.Int(nullable: false),
                        FundedPins = c.Int(nullable: false),
                        PreFundedPins = c.Int(nullable: false),
                        UnfundedAccounts = c.Int(nullable: false),
                        CashVolume = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RSAAchievedPercentage = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Appraisees", "BdsPerformanceTrackerId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appraisees", "BdsPerformanceTrackerId");
            DropTable("dbo.BdsPerformanceTrackers");
        }
    }
}
