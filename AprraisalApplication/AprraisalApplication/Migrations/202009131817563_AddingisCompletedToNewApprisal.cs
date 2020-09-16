namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingisCompletedToNewApprisal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewAppraisals", "IsCompleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NewAppraisals", "IsCompleted");
        }
    }
}
