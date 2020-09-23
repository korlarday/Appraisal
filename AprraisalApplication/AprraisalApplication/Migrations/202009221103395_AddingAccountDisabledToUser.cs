namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingAccountDisabledToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "AccountDisabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "AccountDisabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AccountDisabled");
            DropColumn("dbo.Employees", "AccountDisabled");
        }
    }
}
