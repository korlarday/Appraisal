using AprraisalApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Repositories
{
    public class AppraisalRepository
    {
        private readonly ApplicationDbContext db;

        public AppraisalRepository(ApplicationDbContext context)
        {
            db = context;
        }
    }
}