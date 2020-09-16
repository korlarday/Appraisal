namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDateCreatedToAppraisalStaff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppraisalStaffs", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppraisalStaffs", "DateCreated");
        }
    }
}
