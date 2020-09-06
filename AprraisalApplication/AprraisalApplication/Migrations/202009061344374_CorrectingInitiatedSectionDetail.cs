namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectingInitiatedSectionDetail : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SectionDetailResults", "InitiatedSectionDetailId", "dbo.InitiatedSectionDetails");
            DropIndex("dbo.SectionDetailResults", new[] { "InitiatedSectionDetailId" });
            AddColumn("dbo.SectionDetailResults", "SectionDetailId", c => c.Int(nullable: false));
            DropColumn("dbo.SectionDetailResults", "InitiatedSectionDetailId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SectionDetailResults", "InitiatedSectionDetailId", c => c.Int(nullable: false));
            DropColumn("dbo.SectionDetailResults", "SectionDetailId");
            CreateIndex("dbo.SectionDetailResults", "InitiatedSectionDetailId");
            AddForeignKey("dbo.SectionDetailResults", "InitiatedSectionDetailId", "dbo.InitiatedSectionDetails", "Id", cascadeDelete: true);
        }
    }
}
