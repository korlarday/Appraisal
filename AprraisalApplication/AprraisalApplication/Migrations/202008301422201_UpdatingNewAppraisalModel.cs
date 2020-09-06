namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatingNewAppraisalModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AppraisalStaffs", "NewAppraisal_Id", "dbo.NewAppraisals");
            DropIndex("dbo.AppraisalStaffs", new[] { "NewAppraisal_Id" });
            RenameColumn(table: "dbo.AppraisalStaffs", name: "NewAppraisal_Id", newName: "NewAppraisalId");
            AddColumn("dbo.NewAppraisals", "DateInitiated", c => c.DateTime(nullable: false));
            AddColumn("dbo.NewAppraisals", "AppraisalTitle", c => c.String());
            AddColumn("dbo.NewAppraisals", "Slug", c => c.String());
            AlterColumn("dbo.AppraisalStaffs", "NewAppraisalId", c => c.Int(nullable: false));
            CreateIndex("dbo.AppraisalStaffs", "NewAppraisalId");
            AddForeignKey("dbo.AppraisalStaffs", "NewAppraisalId", "dbo.NewAppraisals", "Id", cascadeDelete: true);
            DropColumn("dbo.AppraisalStaffs", "NewAppraisalStaff");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppraisalStaffs", "NewAppraisalStaff", c => c.Int(nullable: false));
            DropForeignKey("dbo.AppraisalStaffs", "NewAppraisalId", "dbo.NewAppraisals");
            DropIndex("dbo.AppraisalStaffs", new[] { "NewAppraisalId" });
            AlterColumn("dbo.AppraisalStaffs", "NewAppraisalId", c => c.Int());
            DropColumn("dbo.NewAppraisals", "Slug");
            DropColumn("dbo.NewAppraisals", "AppraisalTitle");
            DropColumn("dbo.NewAppraisals", "DateInitiated");
            RenameColumn(table: "dbo.AppraisalStaffs", name: "NewAppraisalId", newName: "NewAppraisal_Id");
            CreateIndex("dbo.AppraisalStaffs", "NewAppraisal_Id");
            AddForeignKey("dbo.AppraisalStaffs", "NewAppraisal_Id", "dbo.NewAppraisals", "Id");
        }
    }
}
