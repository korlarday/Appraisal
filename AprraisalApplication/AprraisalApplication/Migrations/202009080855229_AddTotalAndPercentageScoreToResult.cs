namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTotalAndPercentageScoreToResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SectionResults", "PercentageScore", c => c.Double(nullable: false));
            AddColumn("dbo.SectionResults", "TotalScore", c => c.Double(nullable: false));
            DropColumn("dbo.SectionResults", "Score");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SectionResults", "Score", c => c.Double(nullable: false));
            DropColumn("dbo.SectionResults", "TotalScore");
            DropColumn("dbo.SectionResults", "PercentageScore");
        }
    }
}
