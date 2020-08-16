namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDeleteFlagToModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Branches", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.States", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Departments", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Departments", "IsDeleted");
            DropColumn("dbo.States", "IsDeleted");
            DropColumn("dbo.Branches", "IsDeleted");
        }
    }
}
