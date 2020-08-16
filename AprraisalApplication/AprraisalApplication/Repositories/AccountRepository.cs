using AprraisalApplication.Models;
using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Repositories
{
    public class AccountRepository
    {
        private readonly ApplicationDbContext db;

        public AccountRepository(ApplicationDbContext context)
        {
            db = context;
        }

        public ApplicationUser GetUserById(string userId)
        {
            return db.Users.Find(userId);
        }
    }
}