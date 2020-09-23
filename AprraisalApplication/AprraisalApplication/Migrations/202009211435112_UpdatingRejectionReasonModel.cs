namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatingRejectionReasonModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppraiseeRejections", "RejectTo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppraiseeRejections", "RejectTo");
        }
    }
}
