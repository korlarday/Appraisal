namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingOptionalSectionFilledToSectionResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SectionResults", "SectionFilled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SectionResults", "SectionFilled");
        }
    }
}