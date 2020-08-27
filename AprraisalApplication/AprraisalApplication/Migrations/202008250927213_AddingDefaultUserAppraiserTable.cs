namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDefaultUserAppraiserTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DefaultUserAppraisers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppraiseeId = c.String(maxLength: 128),
                        AppraiserId = c.String(),
                        ToTeamLeader = c.Boolean(nullable: false),
                        SubmissionToHOD = c.Boolean(nullable: false),
                        SubmissionToHR = c.Boolean(nullable: false),
                        SubmissionToMD = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AppraiseeId)
                .Index(t => t.AppraiseeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DefaultUserAppraisers", "AppraiseeId", "dbo.AspNetUsers");
            DropIndex("dbo.DefaultUserAppraisers", new[] { "AppraiseeId" });
            DropTable("dbo.DefaultUserAppraisers");
        }
    }
}
