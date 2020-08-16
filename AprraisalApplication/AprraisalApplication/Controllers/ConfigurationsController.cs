using AprraisalApplication.Models;
using AprraisalApplication.Models.ViewModels;
using AprraisalApplication.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprraisalApplication.Controllers
{
    public class ConfigurationsController : Controller
    {
        public readonly ApplicationDbContext db;
        public readonly UnitOfWork _unitOfWork;
        public ConfigurationsController()
        {
            db = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(db);
        }

        // GET: Configurations
        [ActionName("set-departments")]
        public ActionResult Departments()
        {
            SetDepartmentVM model = new SetDepartmentVM
            {
                Departments = _unitOfWork.Resources.GetAllDepartments()
            };
            return View("Departments", model);
        }

        [ActionName("set-branches")]
        public ActionResult SetBranches()
        {
            SetBranchVM model = new SetBranchVM
            {
                States = new SelectList(_unitOfWork.Resources.GetAllStates(), "Id", "Description"),
                Branches = _unitOfWork.Resources.GetAllBranches()
            };
            return View("SetBranches", model);
        }
    }
}