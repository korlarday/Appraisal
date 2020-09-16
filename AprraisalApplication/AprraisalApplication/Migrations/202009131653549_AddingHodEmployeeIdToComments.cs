namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingHodEmployeeIdToComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppraiseeComments", "HodEmployeeId", c => c.Int());
            AddColumn("dbo.AppraiseeComments", "MdEmployeeId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppraiseeComments", "MdEmployeeId");
            DropColumn("dbo.AppraiseeComments", "HodEmployeeId");
        }
    }
}
