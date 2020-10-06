using AprraisalApplication.Models;
using AprraisalApplication.Models.Attributes;
using AprraisalApplication.Models.MigrationModels;
using AprraisalApplication.Models.ViewModels;
using AprraisalApplication.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AprraisalApplication.Controllers
{
    [Authorize(Roles = "HR")]
    [EmailConfirmation]
    [CompleteYourProfile]
    public class HRController : Controller
    {
        public readonly ApplicationDbContext db;
        public readonly UnitOfWork _unitOfWork;
        public HRController()
        {
            db = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(db);
        }
        // GET: HumanResource

        [ActionName("all-employees")]
        public ActionResult AllEmployees()
        {
            List<Employee> employees = _unitOfWork.Office.GetAllEmployees();
            return View("AllEmployees",employees);
        }

        [ActionName("view-profile")]
        public ActionResult ViewProfile(string slug)
        {
            string userId = slug;
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(userId);
            if(employee == null)
            {
                return HttpNotFound();
            }
            return View("ViewProfile", employee);
        }

        [ActionName("edit-profile")]
        public ActionResult EditProfile(string slug)
        {
            string userId = slug;
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(userId);
            if(employee == null)
            {
                return HttpNotFound();
            }
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
                TempData["SM"] = "employee's profile was updated successfully";
                return RedirectToAction("view-profile", new { slug = model.ApplicationUserId });
            }
            else
            {
                model = PopulateSelectList(model);
                ModelState.AddModelError("", "Sorry! An error occured while saving employee's profile");
                return View("EditProfile", model);
            }
        }

        [ActionName("edit-career-history-hr")]
        public ActionResult EditCareerHistory(string slug)
        {
            var userId = slug;
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(userId);
            if(employee == null)
            {
                return HttpNotFound();
            }
            EditCareerHistoryVM model = new EditCareerHistoryVM
            {
                Departments = new SelectList(_unitOfWork.Resources.GetAllDepartments(), "Id", "Name"),
                Grades = new SelectList(_unitOfWork.Resources.GetGrades(), "Id", "Name"),
                Employee = employee
            };
            return View("EditCareerHistory", model);
        }

        [ActionName("employees-by-department")]
        public ActionResult EmployeesByDepartment()
        {
            List<DepartmentAndParticipants> participants = _unitOfWork.Office.GetDepartmentAndEmployeesCount();
            return View("EmployeesByDepartment", participants);
        }

        [ActionName("view-employees-in-department")]
        public ActionResult ViewEmployeesInDepartment(int slug)
        {
            int departmentId = slug;
            Department department = _unitOfWork.Resources.GetDepartmentById(departmentId);
            if(department == null)
            {
                return HttpNotFound();
            }
            List<Employee> employees = _unitOfWork.Office.GetAllEmployeesInDepartment(departmentId);
            EmployeesInDepartmentVM model = new EmployeesInDepartmentVM
            {
                Employees = employees,
                Department = department
            };
            return View("ViewEmployeesInDepartment", model);
        }

        [ActionName("employees-by-location")]
        public ActionResult EmployeesByLocation()
        {
            List<LocationAndEmployees> locations = _unitOfWork.Office.GetLocationAndEmployees();
            return View("EmployeesByLocation", locations);
        }

        [ActionName("view-employees-in-location")]
        public ActionResult ViewEmployeesInLocation(int slug)
        {
            int stateId = slug;
            State state = _unitOfWork.Resources.GetStateById(stateId);
            if (state == null)
            {
                return HttpNotFound();
            }
            List<Employee> employees = _unitOfWork.Office.GetAllEmployeesInState(stateId);
            LocationAndEmployees model = new LocationAndEmployees
            {
                Employees = employees,
                State = state
            };
            return View("ViewEmployeesInLocation", model);
        }

        [ActionName("deactivated-employees")]
        public ActionResult DeactivatedEmployees()
        {
            List<Employee> employees = _unitOfWork.Office.GetDeactivatedEmployees();
            return View("DeactivatedEmployees", employees);
        }

        [ActionName("set-hod-supervisors")]
        public ActionResult SetHodSupervisors()
        {
            List<Employee> supervisors = _unitOfWork.Office.GetAllHodsAndHigherRanks();
            List<Employee> employees = _unitOfWork.Office.GetAllHods();
            SetupEmployeeAppraiserVM model = new SetupEmployeeAppraiserVM
            {
                UserAppraisers = employees,
                Employees = employees,
                Supervisors = supervisors
            };
            return View("SetHodSupervisors", model);
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