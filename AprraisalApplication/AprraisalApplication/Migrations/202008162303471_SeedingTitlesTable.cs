namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedingTitlesTable : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO Titles (Id, Name, [IsDeleted]) VALUES (1, 'MISS', 0)");
            Sql(@"INSERT INTO Titles (Id, Name, [IsDeleted]) VALUES (2, 'MR', 0)");
            Sql(@"INSERT INTO Titles (Id, Name, [IsDeleted]) VALUES (3, 'MIS', 0)");
            Sql(@"INSERT INTO Titles (Id, Name, [IsDeleted]) VALUES (4, 'MS', 0)");
            Sql(@"INSERT INTO Titles (Id, Name, [IsDeleted]) VALUES (5, 'MRS', 0)");
        }
        
        public override void Down()
        {
            Sql(@"DELETE FROM Titles WHERE Id IN (1, 2, 3, 4, 5)");
        }
    }
}
