namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingHrCommentToComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppraiseeComments", "HrComment", c => c.String());
            AddColumn("dbo.AppraiseeComments", "HrCommentDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AppraiseeComments", "HrEmployeeId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppraiseeComments", "HrEmployeeId");
            DropColumn("dbo.AppraiseeComments", "HrCommentDate");
            DropColumn("dbo.AppraiseeComments", "HrComment");
        }
    }
}
