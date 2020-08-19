namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedingDefaultSummaryRatings : DbMigration
    {
        public override void Up()
        {
            Sql("SET IDENTITY_INSERT DefaultSummaryRatings ON");
            Sql(@"INSERT INTO DefaultSummaryRatings (Id, Score, Rating) VALUES (1, 0, 'Unsatisfactory')");
            Sql(@"INSERT INTO DefaultSummaryRatings (Id, Score, Rating) VALUES (2, 50, 'Average')");
            Sql(@"INSERT INTO DefaultSummaryRatings (Id, Score, Rating) VALUES (3, 60, 'Good')");
            Sql(@"INSERT INTO DefaultSummaryRatings (Id, Score, Rating) VALUES (4, 70, 'Very Good')");
            Sql(@"INSERT INTO DefaultSummaryRatings (Id, Score, Rating) VALUES (5, 80, 'Outstanding')");
        }
        
        public override void Down()
        {
            Sql(@"DELETE FROM DefaultSummaryRatings WHERE Id IN (1, 2, 3, 4, 5)");
        }
    }
}
