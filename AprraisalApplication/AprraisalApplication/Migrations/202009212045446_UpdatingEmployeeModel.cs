namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatingEmployeeModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "StaffCodeNo", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "StaffCodeNo");
        }
    }
}
