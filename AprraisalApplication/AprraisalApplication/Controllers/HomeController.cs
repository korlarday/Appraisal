using AprraisalApplication.Models;
using AprraisalApplication.Models.Attributes;
using AprraisalApplication.Models.ViewModels;
using AprraisalApplication.Persistence;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprraisalApplication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public readonly ApplicationDbContext db;
        public readonly UnitOfWork _unitOfWork;
        public HomeController()
        {
            db = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(db);
        }
        public ActionResult Index()
        {
            DashboardVM model = new DashboardVM
            {
                NumberOfEmployees = _unitOfWork.Office.GetAllEmployees().Count(),
                CompletedAppraisals = _unitOfWork.Office.GetAllCompletedAppraisals(),
                OngoingAppraisals = _unitOfWork.Office.GetAllOngoingAppraisals()
            };
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult UserPartial()
        {
            string userId = User.Identity.GetUserId();

            UserPartialVM model = new UserPartialVM
            {
                User = _unitOfWork.Account.GetUserById(userId)
            };

            return PartialView(model);
        }
    }
}