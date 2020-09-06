namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectingResultBreakdownModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemBreakdownResults", "Title", c => c.String());
            AddColumn("dbo.ItemBreakdownResults", "ExpectedValueId", c => c.Byte(nullable: false));
            AddColumn("dbo.ItemBreakdownResults", "Value", c => c.String());
            CreateIndex("dbo.ItemBreakdownResults", "ExpectedValueId");
            AddForeignKey("dbo.ItemBreakdownResults", "ExpectedValueId", "dbo.ExpectedValues", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemBreakdownResults", "ExpectedValueId", "dbo.ExpectedValues");
            DropIndex("dbo.ItemBreakdownResults", new[] { "ExpectedValueId" });
            DropColumn("dbo.ItemBreakdownResults", "Value");
            DropColumn("dbo.ItemBreakdownResults", "ExpectedValueId");
            DropColumn("dbo.ItemBreakdownResults", "Title");
        }
    }
}
