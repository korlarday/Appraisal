namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingIsScoreToSectionDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SectionDetailBreakdowns", "IsScore", c => c.Boolean(nullable: false));
            AddColumn("dbo.InitiatedSectionDetailBreakdowns", "IsScore", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InitiatedSectionDetailBreakdowns", "IsScore");
            DropColumn("dbo.SectionDetailBreakdowns", "IsScore");
        }
    }
}
