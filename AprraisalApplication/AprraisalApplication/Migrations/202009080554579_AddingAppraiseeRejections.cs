namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingAppraiseeRejections : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppraiseeRejections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppraiseeId = c.Int(nullable: false),
                        New = c.Boolean(nullable: false),
                        DateRejected = c.DateTime(nullable: false),
                        RejectionReason = c.String(),
                        RejectedById = c.Int(nullable: false),
                        RejectedByPosition = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.RejectedById, cascadeDelete: true)
                .ForeignKey("dbo.Appraisees", t => t.AppraiseeId, cascadeDelete: true)
                .Index(t => t.AppraiseeId)
                .Index(t => t.RejectedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppraiseeRejections", "AppraiseeId", "dbo.Appraisees");
            DropForeignKey("dbo.AppraiseeRejections", "RejectedById", "dbo.Employees");
            DropIndex("dbo.AppraiseeRejections", new[] { "RejectedById" });
            DropIndex("dbo.AppraiseeRejections", new[] { "AppraiseeId" });
            DropTable("dbo.AppraiseeRejections");
        }
    }
}
