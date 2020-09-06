namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectingPrimaryKeyOnInitiatedSectionBreakdown : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InitiatedSectionDetailBreakdowns", "InitiatedSectionDetail_Id", "dbo.InitiatedSectionDetails");
            DropIndex("dbo.InitiatedSectionDetailBreakdowns", new[] { "InitiatedSectionDetail_Id" });
            RenameColumn(table: "dbo.InitiatedSectionDetailBreakdowns", name: "InitiatedSectionDetail_Id", newName: "InitiatedSectionDetailId");
            AlterColumn("dbo.InitiatedSectionDetailBreakdowns", "InitiatedSectionDetailId", c => c.Int(nullable: false));
            CreateIndex("dbo.InitiatedSectionDetailBreakdowns", "InitiatedSectionDetailId");
            AddForeignKey("dbo.InitiatedSectionDetailBreakdowns", "InitiatedSectionDetailId", "dbo.InitiatedSectionDetails", "Id", cascadeDelete: false);
            DropColumn("dbo.InitiatedSectionDetailBreakdowns", "AppraisalSectionDetailId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InitiatedSectionDetailBreakdowns", "AppraisalSectionDetailId", c => c.Int(nullable: false));
            DropForeignKey("dbo.InitiatedSectionDetailBreakdowns", "InitiatedSectionDetailId", "dbo.InitiatedSectionDetails");
            DropIndex("dbo.InitiatedSectionDetailBreakdowns", new[] { "InitiatedSectionDetailId" });
            AlterColumn("dbo.InitiatedSectionDetailBreakdowns", "InitiatedSectionDetailId", c => c.Int());
            RenameColumn(table: "dbo.InitiatedSectionDetailBreakdowns", name: "InitiatedSectionDetailId", newName: "InitiatedSectionDetail_Id");
            CreateIndex("dbo.InitiatedSectionDetailBreakdowns", "InitiatedSectionDetail_Id");
            AddForeignKey("dbo.InitiatedSectionDetailBreakdowns", "InitiatedSectionDetail_Id", "dbo.InitiatedSectionDetails", "Id");
        }
    }
}
