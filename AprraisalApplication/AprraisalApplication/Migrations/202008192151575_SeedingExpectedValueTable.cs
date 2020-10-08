namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedingExpectedValueTable : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO ExpectedValues (Id, Name) VALUES (1, 'Number Value')");
            Sql(@"INSERT INTO ExpectedValues (Id, Name) VALUES (2, 'Monetary Value')");
            Sql(@"INSERT INTO ExpectedValues (Id, Name) VALUES (3, 'No Value')");
        }
        
        public override void Down()
        {
            Sql(@"DELETE FROM ExpectedValues WHERE Id IN (1, 2, 3)");
        }
    }
}
