namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeintToStringExpectedValue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ExpectedValues", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ExpectedValues", "Name", c => c.Int(nullable: false));
        }
    }
}
