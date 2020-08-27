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
            string userId = User.Identity.GetUserId();
            ApplicationUser user = _unitOfWork.Account.GetUserById(userId);
            if(user.EmployeeId == null)
            {
                CreateEmployeeProfileVM model = new CreateEmployeeProfileVM();
                model = PopulateSelectList(model);
                return View("UserProfile", model);
            }
            else
            {
                Employee employee = _unitOfWork.Account.GetEmployeeByUserId(userId);
                return View("ViewProfile", employee);
            }
        }

        [ActionName("user-profile")]
        [ValidateAntiForgeryToken, HttpPost]
        public ActionResult UserProfile(CreateEmployeeProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                model = PopulateSelectList(model);
                return View("UserProfile", model);
            }

            bool result = _unitOfWork.Account.SaveEmployeeDetails(model);
            if (result)
            {
                TempData["SM"] = "your profile was updated successfully";
                return RedirectToAction("user-profile");
            }
            else
            {
                model = PopulateSelectList(model);
                ModelState.AddModelError("", "Sorry! An error occured while saving your profile");
                return View("UserProfile", model);
            }
        }

        [ActionName("edit-profile")]
        public ActionResult EditProfile()
        {
            string userId = User.Identity.GetUserId();
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(userId);
            CreateEmployeeProfileVM model = new CreateEmployeeProfileVM(employee);
            model = PopulateSelectList(model);
            return View("EditProfile", model);
        }

        [ActionName("edit-profile")]
        [ValidateAntiForgeryToken, HttpPost]
        public ActionResult EditProfile(CreateEmployeeProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                model = PopulateSelectList(model);
                return View("EditProfile", model);
            }

            bool result = _unitOfWork.Account.UpdateEmployeeDetails(model);
            if (result)
            {
                TempData["SM"] = "your profile was updated successfully";
                return RedirectToAction("user-profile");
            }
            else
            {
                model = PopulateSelectList(model);
                ModelState.AddModelError("", "Sorry! An error occured while saving your profile");
                return View("EditProfile", model);
            }
        }

        [ActionName("edit-career-history")]
        public ActionResult EditCareerHistory()
        {
            var userId = User.Identity.GetUserId();
            EditCareerHistoryVM model = new EditCareerHistoryVM
            {
                Departments = new SelectList(_unitOfWork.Resources.GetAllDepartments(), "Id", "Name"),
                Grades = new SelectList(_unitOfWork.Resources.GetGrades(), "Id", "Name"),
                Employee = _unitOfWork.Account.GetEmployeeByUserId(userId)
            };
            return View("EditCareerHistory", model);
        }

        private CreateEmployeeProfileVM PopulateSelectList(CreateEmployeeProfileVM model)
        {
            model.States = new SelectList(_unitOfWork.Resources.GetAllStates(), "Id", "Description");
            model.Genders = new SelectList(_unitOfWork.Resources.GetGenders(), "Id", "Name");
            model.Branches = new SelectList(_unitOfWork.Resources.GetAllBranches(), "Id", "Description");
            model.JobTitles = new SelectList(_unitOfWork.Resources.GetAllJobTitles(), "Id", "Name");
            model.Titles = new SelectList(_unitOfWork.Resources.GetTitles(), "Id", "Name");
            model.Departments = new SelectList(_unitOfWork.Resources.GetAllDepartments(), "Id", "Name");
            model.Qualifications = _unitOfWork.Resources.GetAllQualifications();
            model.Grades = new SelectList(_unitOfWork.Resources.GetGrades(), "Id", "Name");
            return model;
        }
    }
}