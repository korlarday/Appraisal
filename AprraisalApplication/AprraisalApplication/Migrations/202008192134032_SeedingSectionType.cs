namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedingSectionType : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO SectionTypes (Id, Name) VALUES (1, 'Task performed by employee and their ratings')");
            Sql(@"INSERT INTO SectionTypes (Id, Name) VALUES (2, 'Duties assigned to employee and their ratings')");
            Sql(@"INSERT INTO SectionTypes (Id, Name) VALUES (3, 'Quatitative indices')");
            Sql(@"INSERT INTO SectionTypes (Id, Name) VALUES (4, 'Qualitative indices')");
        }
        
        public override void Down()
        {
            Sql(@"DELETE FROM SectionTypes WHERE Id IN (1, 2, 3, 4)");
        }
    }
}
