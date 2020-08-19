namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingGradesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Employees", "GradeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "GradeId");
            AddForeignKey("dbo.Employees", "GradeId", "dbo.Grades", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "GradeId", "dbo.Grades");
            DropIndex("dbo.Employees", new[] { "GradeId" });
            DropColumn("dbo.Employees", "GradeId");
            DropTable("dbo.Grades");
        }
    }
}
