namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDefaultRatings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DefaultRatings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Score = c.Int(nullable: false),
                        Rating = c.String(nullable: false, maxLength: 255),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DefaultRatings");
        }
    }
}
