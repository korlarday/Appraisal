using AprraisalApplication.Models;
using AprraisalApplication.Models.Attributes;
using AprraisalApplication.Models.MigrationModels;
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
    [EmailConfirmation]
    [CompleteYourProfile]
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
            string userId = User.Identity.GetUserId();
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(userId);
            DashboardVM model = new DashboardVM
            {
                NumberOfEmployees = _unitOfWork.Office.GetAllEmployees().Count(),
                CompletedAppraisals = employee == null ? 0 : _unitOfWork.Office.GetAllCompletedAppraisals(),
                OngoingAppraisals = employee == null ? 0 : _unitOfWork.Office.GetAllOngoingAppraisals(),
                EmployeesInDept = employee == null ? 0 : _unitOfWork.Office.GetAllEmployeesInDepartment(employee.DepartmentId).Count(),
                MyOngoingAppraisals = employee == null ? 0 : _unitOfWork.Appraisal.GetEmployeeOngoingAppraisals(employee.Id).Count(),
                MySubordinates = employee == null ? 0 : _unitOfWork.Appraisal.GetMyAppraisees(userId).Count(),
                DeactivatedEmployees = _unitOfWork.Office.GetDeactivatedEmployees().Count()
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
    
        public ActionResult ShowLinkIfSupervisor()
        {
            string userId = User.Identity.GetUserId();
            int countAppraisees = _unitOfWork.Appraisal.GetMyAppraisees(userId).Count();
            ShowLinkIfSupervisorVM link = new ShowLinkIfSupervisorVM
            {
                IsSupervisor = countAppraisees > 0
            };
            return PartialView(link);
        }
    }
}