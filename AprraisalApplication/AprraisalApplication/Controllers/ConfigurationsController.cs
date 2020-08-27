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

        [ActionName("appraisal-template-setup")]
        public ActionResult AppraisalTemplateSetup()
        {
            AppraisalTemplateSetupVM model = new AppraisalTemplateSetupVM
            {
                SectionTypes = _unitOfWork.Resources.GetSectionTypes(),
                ExpectedValues = _unitOfWork.Resources.GetExpectedValues(),
                DefaultRatings = _unitOfWork.Resources.GetDefaultRatings()
            };
            return View("AppraisalTemplateSetup", model);
        }

        [ActionName("appraisal-templates")]
        public ActionResult AppraisalTemplates()
        {
            List<AppraisalTemplate> model = _unitOfWork.AppraisalTemplate.GetAllTemplates();
            return View("AppraisalTemplates", model);
        }

        [ActionName("edit-appraisal-template")]
        public ActionResult EditAppraisalTemplate(string slug)
        {
            AppraisalTemplate appraisalTemplate = _unitOfWork.AppraisalTemplate.GetAppraisalTemplateBySlug(slug);
            if(appraisalTemplate == null)
            {
                return HttpNotFound();
            }
            AppraisalTemplateSetupVM model = new AppraisalTemplateSetupVM
            {
                SectionTypes = _unitOfWork.Resources.GetSectionTypes(),
                ExpectedValues = _unitOfWork.Resources.GetExpectedValues(),
                DefaultRatings = _unitOfWork.Resources.GetDefaultRatings(),
                AppraisalTemplate = appraisalTemplate
            };
            return View("EditAppraisalTemplate", model);
        }

        [ActionName("view-appraisal-template")]
        public ActionResult ViewAppraisalTemplate(string slug)
        {
            AppraisalTemplate appraisalTemplate = _unitOfWork.AppraisalTemplate.GetAppraisalTemplateBySlug(slug);
            if (appraisalTemplate == null)
            {
                return HttpNotFound();
            }
            AppraisalTemplateSetupVM model = new AppraisalTemplateSetupVM
            {
                SectionTypes = _unitOfWork.Resources.GetSectionTypes(),
                ExpectedValues = _unitOfWork.Resources.GetExpectedValues(),
                DefaultRatings = _unitOfWork.Resources.GetDefaultRatings(),
                AppraisalTemplate = appraisalTemplate
            };
            return View("ViewAppraisalTemplate", model);
        }
    
        [ActionName("setup-department-template")]
        public ActionResult SetupDepartmentTemplate()
        {
            List<Department> departments = _unitOfWork.Resources.GetAllDepartments();
            List<AppraisalTemplate> templates = _unitOfWork.AppraisalTemplate.GetAllTemplates();
            List<AppraisalDepartmentTemplate> appraisalDepartmentTemplates = _unitOfWork.AppraisalTemplate.GetAppraisalDepartmentTemplates();
            List<DepartmentAndTemplate> departmentAndTemplates = new List<DepartmentAndTemplate>();

            foreach (var item in departments)
            {
                var search = appraisalDepartmentTemplates.Find(x => x.DepartmentId == item.Id);
                DepartmentAndTemplate dt = new DepartmentAndTemplate
                {
                    DepartmentId = item.Id,
                    DepartmentName = item.Name,
                    TemplateId = search != null ? search.AppraisalTemplateId : 0
                };
                departmentAndTemplates.Add(dt);
            }
            SetupDepartmentTemplateVM setup = new SetupDepartmentTemplateVM
            {
                AppraisalDepartmentTemplates = appraisalDepartmentTemplates,
                Departments = departments,
                Templates = templates,
                DepartmentAndTemplates = departmentAndTemplates
            };
            return View("SetupDepartmentTemplate", setup);
        }
    
        [ActionName("setup-employee-template")]
        public ActionResult SetupEmployeeTemplate()
        {
            List<ApplicationUser> users = _unitOfWork.Account.GetAllUsers();
            List<AppraisalTemplate> templates = _unitOfWork.AppraisalTemplate.GetAllTemplates();
            List<AppraisalUserTemplate> appraisalUserTemplates = _unitOfWork.AppraisalTemplate.GetAppraisalUserTemplates();
            List<EmployeeAndTemplate> employeeAndTemplates = new List<EmployeeAndTemplate>();

            foreach (var item in users)
            {
                var search = appraisalUserTemplates.Find(x => x.ApplicationUserId == item.Id);
                var employee = _unitOfWork.Account.GetEmployeeByUserId(item.Id);
                EmployeeAndTemplate et = new EmployeeAndTemplate
                {
                    UserId = item.Id,
                    UserName = employee.Firstname + " " + employee.Lastname + " " + employee.Othername,
                    TemplateId = search != null ? search.AppraisalTemplateId : 0
                };
                employeeAndTemplates.Add(et);
            }
            SetupEmployeeTemplateVM setup = new SetupEmployeeTemplateVM
            {
                Templates = templates,
                EmployeeAndTemplates = employeeAndTemplates
            };
            return View("SetupEmployeeTemplate", setup);
        }

        [ActionName("department-setup")]
        public ActionResult DepartmentSetup()
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = _unitOfWork.Account.GetUserById(userId);
            Employee userEmpDetails = _unitOfWork.Office.GetUserEmployeeDetails((int)user.EmployeeId);
            if (userEmpDetails == null)
            {
                return HttpNotFound();
            }
            List<Employee> employees = _unitOfWork.Office.GetAllEmployeesInDepartment(userEmpDetails.DepartmentId);

            SetupEmployeeAppraiserVM model = new SetupEmployeeAppraiserVM
            {
                UserAppraisers = employees,
                Employees = employees
            };
            return View("DepartmentSetup", model);
        }
    }
}