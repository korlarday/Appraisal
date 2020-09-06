namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingBreakdownAddedBy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppraisalTemplateSections", "BreakdownValueBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppraisalTemplateSections", "BreakdownValueBy");
        }
    }
}
