namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingIsDeletedFieldsTemplateModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppraisalSectionDetails", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.SectionDetailBreakdowns", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppraisalTemplateSections", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppraisalTemplateSections", "IsDeleted");
            DropColumn("dbo.SectionDetailBreakdowns", "IsDeleted");
            DropColumn("dbo.AppraisalSectionDetails", "IsDeleted");
        }
    }
}
