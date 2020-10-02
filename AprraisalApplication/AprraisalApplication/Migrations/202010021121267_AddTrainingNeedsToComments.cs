namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrainingNeedsToComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppraiseeComments", "AppraiseeTrainingNeeds", c => c.String());
            AddColumn("dbo.AppraiseeComments", "AppraiserTrainingNeeds", c => c.String());
            DropColumn("dbo.AppraiseeComments", "TrainingNeeds");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppraiseeComments", "TrainingNeeds", c => c.String());
            DropColumn("dbo.AppraiseeComments", "AppraiserTrainingNeeds");
            DropColumn("dbo.AppraiseeComments", "AppraiseeTrainingNeeds");
        }
    }
}
