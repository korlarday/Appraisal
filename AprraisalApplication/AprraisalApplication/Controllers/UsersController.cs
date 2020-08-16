using AprraisalApplication.Models;
using AprraisalApplication.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprraisalApplication.Controllers
{
    public class UsersController : Controller
    {
        public readonly ApplicationDbContext db;
        public readonly UnitOfWork _unitOfWork;
        public UsersController()
        {
            db = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(db);
        }
        
        [ActionName("user-profile")]
        public ActionResult UserProfile()
        {
            return View();
        }
    }
}