namespace AprraisalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedingDefaultRatings : DbMigration
    {
        public override void Up()
        {
            Sql("SET IDENTITY_INSERT DefaultRatings ON");
            Sql(@"INSERT INTO DefaultRatings (Id, Score, Rating, Description) VALUES 
                (1, 1, 'Unsatisfactory', 'Overall performance is poor and lower to the required standards of performance required for the job.')");
            Sql(@"INSERT INTO DefaultRatings (Id, Score, Rating, Description) VALUES 
                (2, 2, 'Below Average', 'Overall performance is below average and does not meet the required standards of performance for the job. Serious effort is needed to improve performance.')");
            Sql(@"INSERT INTO DefaultRatings (Id, Score, Rating, Description) VALUES 
                (3, 3, 'Satisfactory', 'Overall performances satisfactory and meets the requirements of the job.')");
            Sql(@"INSERT INTO DefaultRatings (Id, Score, Rating, Description) VALUES 
                (4, 4, 'Very Good', 'Overall performances are very good and are above average most of the time; periodically exceeds expectations.')");
            Sql(@"INSERT INTO DefaultRatings (Id, Score, Rating, Description) VALUES 
                (5, 5, 'Outstanding', 'Overall performances are outstanding and consistently superior to standards required for the job. A model of a perfect employee.')");
        }
        
        public override void Down()
        {
            Sql(@"DELETE FROM DefaultRatings WHERE Id IN (1, 2, 3, 4, 5)");
        }
    }
}
