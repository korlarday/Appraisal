namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGradeToCareerHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareerHistories", "GradeId", c => c.Int(nullable: false));
            CreateIndex("dbo.CareerHistories", "GradeId");
            AddForeignKey("dbo.CareerHistories", "GradeId", "dbo.Grades", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CareerHistories", "GradeId", "dbo.Grades");
            DropIndex("dbo.CareerHistories", new[] { "GradeId" });
            DropColumn("dbo.CareerHistories", "GradeId");
        }
    }
}
