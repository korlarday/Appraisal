namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingPassportAndSignatureToEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "PassportImage", c => c.Binary());
            AddColumn("dbo.Employees", "SignatureImage", c => c.Binary());
            AddColumn("dbo.Employees", "DepartmentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "DepartmentId");
            AddForeignKey("dbo.Employees", "DepartmentId", "dbo.Departments", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Employees", new[] { "DepartmentId" });
            DropColumn("dbo.Employees", "DepartmentId");
            DropColumn("dbo.Employees", "SignatureImage");
            DropColumn("dbo.Employees", "PassportImage");
        }
    }
}
