namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefaultAppraiserPrimaryKeyToEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "DefaultUserAppraiserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "DefaultUserAppraiserId");
            AddForeignKey("dbo.Employees", "DefaultUserAppraiserId", "dbo.DefaultUserAppraisers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "DefaultUserAppraiserId", "dbo.DefaultUserAppraisers");
            DropIndex("dbo.Employees", new[] { "DefaultUserAppraiserId" });
            DropColumn("dbo.Employees", "DefaultUserAppraiserId");
        }
    }
}
