namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingStates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.States",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 255),
                        Description = c.String(maxLength: 255),
                        ZoneCode = c.String(maxLength: 255),
                        TransId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.States");
        }
    }
}
