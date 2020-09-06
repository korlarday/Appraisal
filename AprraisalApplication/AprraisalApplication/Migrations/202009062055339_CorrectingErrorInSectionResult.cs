namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectingErrorInSectionResult : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SectionResults", "Appraisee_Id", "dbo.Appraisees");
            DropIndex("dbo.SectionResults", new[] { "Appraisee_Id" });
            RenameColumn(table: "dbo.SectionResults", name: "Appraisee_Id", newName: "AppraiseeId");
            AlterColumn("dbo.SectionResults", "AppraiseeId", c => c.Int(nullable: false));
            CreateIndex("dbo.SectionResults", "AppraiseeId");
            AddForeignKey("dbo.SectionResults", "AppraiseeId", "dbo.Appraisees", "Id", cascadeDelete: false);
            DropColumn("dbo.SectionResults", "AppraieeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SectionResults", "AppraieeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.SectionResults", "AppraiseeId", "dbo.Appraisees");
            DropIndex("dbo.SectionResults", new[] { "AppraiseeId" });
            AlterColumn("dbo.SectionResults", "AppraiseeId", c => c.Int());
            RenameColumn(table: "dbo.SectionResults", name: "AppraiseeId", newName: "Appraisee_Id");
            CreateIndex("dbo.SectionResults", "Appraisee_Id");
            AddForeignKey("dbo.SectionResults", "Appraisee_Id", "dbo.Appraisees", "Id");
        }
    }
}
