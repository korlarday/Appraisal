namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingAppraiseeProgress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppraiseeProgresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppraiseeSubmit = c.Boolean(nullable: false),
                        SupervisorSubmit = c.Boolean(nullable: false),
                        SupervisorReject = c.Boolean(nullable: false),
                        HODSubmit = c.Boolean(nullable: false),
                        HODReject = c.Boolean(nullable: false),
                        HRSubmit = c.Boolean(nullable: false),
                        HRReject = c.Boolean(nullable: false),
                        MDAcknowledgement = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppraiseeSectionDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppraiseeId = c.Int(nullable: false),
                        AppraisalTemplateSectionId = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Title1 = c.String(),
                        Title2 = c.String(),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Appraisees", "AppraiseeProgressId", c => c.Int(nullable: false));
            CreateIndex("dbo.Appraisees", "AppraiseeProgressId");
            AddForeignKey("dbo.Appraisees", "AppraiseeProgressId", "dbo.AppraiseeProgresses", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appraisees", "AppraiseeProgressId", "dbo.AppraiseeProgresses");
            DropIndex("dbo.Appraisees", new[] { "AppraiseeProgressId" });
            DropColumn("dbo.Appraisees", "AppraiseeProgressId");
            DropTable("dbo.AppraiseeSectionDetails");
            DropTable("dbo.AppraiseeProgresses");
        }
    }
}
