namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedingAppraisalTypes : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO AppraisalTypes (Id, Name) VALUES (1, 'Quarterly Appraisal')");
            Sql(@"INSERT INTO AppraisalTypes (Id, Name) VALUES (2, 'Annual Appraisal')");
        }
        
        public override void Down()
        {
            Sql(@"DELETE FROM AppraisalTypes WHERE Id IN (1, 2)");
        }
    }
}
