namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingOptionalSectionFieldToSectionResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SectionResults", "Optional", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SectionResults", "Optional");
        }
    }
}
