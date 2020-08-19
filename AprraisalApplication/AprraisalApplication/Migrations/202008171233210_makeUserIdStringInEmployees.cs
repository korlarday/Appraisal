namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makeUserIdStringInEmployees : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Employees", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Employees", "ApplicationUserId");
            RenameColumn(table: "dbo.Employees", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            AlterColumn("dbo.Employees", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Employees", "ApplicationUserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Employees", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Employees", "ApplicationUserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Employees", name: "ApplicationUserId", newName: "ApplicationUser_Id");
            AddColumn("dbo.Employees", "ApplicationUserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "ApplicationUser_Id");
        }
    }
}
