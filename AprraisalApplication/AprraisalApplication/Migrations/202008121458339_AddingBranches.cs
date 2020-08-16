namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingBranches : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StateId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: true)
                .Index(t => t.StateId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Branches", "StateId", "dbo.States");
            DropIndex("dbo.Branches", new[] { "StateId" });
            DropTable("dbo.Branches");
        }
    }
}
