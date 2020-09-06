namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectionSectionResultsModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SectionDetailResults", "Title1", c => c.String());
            AddColumn("dbo.SectionDetailResults", "Title2", c => c.String());
            AddColumn("dbo.SectionDetailResults", "Number", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SectionDetailResults", "Number");
            DropColumn("dbo.SectionDetailResults", "Title2");
            DropColumn("dbo.SectionDetailResults", "Title1");
        }
    }
}
