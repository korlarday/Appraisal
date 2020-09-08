namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingAppraiseeComment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppraiseeComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TrainingNeeds = c.String(),
                        AppraiseeComment = c.String(),
                        AppraiseeCommentDate = c.DateTime(nullable: false),
                        AppraiserComment = c.String(),
                        AppraiserCommentDate = c.DateTime(nullable: false),
                        HodComment = c.String(),
                        HodCommentDate = c.DateTime(nullable: false),
                        MdComment = c.String(),
                        MdCommentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Appraisees", "AppraiseeCommentsId", c => c.Int(nullable: false));
            CreateIndex("dbo.Appraisees", "AppraiseeCommentsId");
            AddForeignKey("dbo.Appraisees", "AppraiseeCommentsId", "dbo.AppraiseeComments", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appraisees", "AppraiseeCommentsId", "dbo.AppraiseeComments");
            DropIndex("dbo.Appraisees", new[] { "AppraiseeCommentsId" });
            DropColumn("dbo.Appraisees", "AppraiseeCommentsId");
            DropTable("dbo.AppraiseeComments");
        }
    }
}
