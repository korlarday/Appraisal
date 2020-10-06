using AprraisalApplication.Models;
using AprraisalApplication.Models.Attributes;
using AprraisalApplication.Models.MigrationModels;
using AprraisalApplication.Models.ViewModels;
using AprraisalApplication.Persistence;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;
using AprraisalApplication.Models.Constants;

namespace AprraisalApplication.Controllers
{
    [Authorize(Roles = "HR")]
    [EmailConfirmation]
    [CompleteYourProfile]
    public class ConfigurationsController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public readonly ApplicationDbContext db;
        public readonly UnitOfWork _unitOfWork;
        public ConfigurationsController()
        {
            db = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(db);
        }


        public ConfigurationsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
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

        [ActionName("set-states")]
        public ActionResult SetStates()
        {
            SetBranchVM model = new SetBranchVM
            {
                Allstates = _unitOfWork.Resources.GetAllStates()
            };
            return View("SetStates", model);
        }

        [ActionName("set-job-titles")]
        public ActionResult SetJobTitles()
        {
            SetJobTitlesVM model = new SetJobTitlesVM
            {
                JobTitles = _unitOfWork.Resources.GetAllJobTitles()
            };
            return View("SetJobTitles", model);
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
        public async Task<ActionResult> DepartmentSetup()
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = _unitOfWork.Account.GetUserById(userId);
            Employee userEmpDetails = _unitOfWork.Office.GetUserEmployeeDetails((int)user.EmployeeId);
            if (userEmpDetails == null)
            {
                return HttpNotFound();
            }
            List<Employee> supervisors = await _unitOfWork.Office.GetAllEmployeesInDepartmentAndHigherRanks(userEmpDetails.DepartmentId);
            List<Employee> employees = await _unitOfWork.Office.GetAllEmployeesInDepartmentWithoutHod(userEmpDetails.DepartmentId);
            SetupEmployeeAppraiserVM model = new SetupEmployeeAppraiserVM
            {
                UserAppraisers = employees,
                Employees = employees,
                Supervisors = supervisors
            };
            return View("DepartmentSetup", model);
        }
        
        [ActionName("manage-roles")]
        public ActionResult ManageRoles()
        {
            List<Employee> employees = _unitOfWork.Office.GetAllEmployees();

            List<ViewEmployeesRoles> employeesRoles = new List<ViewEmployeesRoles>();
            foreach (var employee in employees)
            {
                var roles = UserManager.GetRoles(employee.ApplicationUserId);
                ViewEmployeesRoles empRoles = new ViewEmployeesRoles
                {
                    Employee = employee,
                    Roles = roles == null ? null : roles.ToList()
                };
                employeesRoles.Add(empRoles);
            }
            return View("ManageRoles", employeesRoles);
        }
    
        [ActionName("edit-employee-roles")]
        public ActionResult EditEmployeeRoles(string slug)
        {
            string userId = slug;
            var roles = UserManager.GetRoles(userId);
            EditEmployeeRolesVM model = new EditEmployeeRolesVM
            {
                Employee = _unitOfWork.Account.GetEmployeeByUserId(userId),
                SelectedRoles = roles != null ? roles.ToList() : null,
                EmployeeUserId = userId,
                Roles = db.Roles.Where(x => x.Name != "SUPERVISOR").ToList()
            };
            return View("EditEmployeeRoles", model);
        }

        [ActionName("edit-employee-roles")]
        [ValidateAntiForgeryToken, HttpPost]
        public ActionResult EditEmployeeRoles(EditEmployeeRolesVM model)
        {
            var user = _unitOfWork.Account.GetEmployeeRoles(model.EmployeeUserId);
            var userRoles = user.Roles.ToList();
            var dbRoles = db.Roles.ToList();

            foreach (var role in dbRoles)
            {
                if(model.NewRoles != null && model.NewRoles.Contains(role.Id))
                {
                    // check if role exists
                    if(!UserManager.IsInRole(user.Id, role.Name))
                    {
                        UserManager.AddToRole(user.Id, role.Name);
                    }
                }
                else
                {
                    // check if role exists
                    if (UserManager.IsInRole(user.Id, role.Name))
                    {
                        if(role.Name != RoleModel.Supervisor)
                        {
                            UserManager.RemoveFromRole(user.Id, role.Name);
                        }
                    }
                }
            }

            TempData["SM"] = "The employee role has been updated successfully.";
            return RedirectToAction("manage-roles");
        }
    }
}