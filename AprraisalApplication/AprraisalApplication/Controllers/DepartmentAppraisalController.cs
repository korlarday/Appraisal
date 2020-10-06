using AprraisalApplication.Models;
using AprraisalApplication.Models.Attributes;
using AprraisalApplication.Models.MigrationModels;
using AprraisalApplication.Models.ViewModels;
using AprraisalApplication.Persistence;
using Microsoft.AspNet.Identity;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprraisalApplication.Controllers
{
    [Authorize(Roles = "HOD")]
    [EmailConfirmation]
    [CompleteYourProfile]
    public class DepartmentAppraisalController : Controller
    {
        public readonly ApplicationDbContext db;
        public readonly UnitOfWork _unitOfWork;
        public DepartmentAppraisalController()
        {
            db = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(db);
        }

        [ActionName("department-initiated-appraisals")]
        public ActionResult DepartmentInitiatedAppraisals()
        {
            string userId = User.Identity.GetUserId();
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(userId);
            if(employee == null)
            {
                return HttpNotFound();
            }
            DepartmentInitiatedAppraisalsVM model = new DepartmentInitiatedAppraisalsVM
            {
                NewAppriasalAndParticipants = _unitOfWork.Appraisal.GetDepartmentInitiatedAppraisals(employee.DepartmentId)
            };
            return View("DepartmentInitiatedAppraisals", model);
        }
        
        [ActionName("appraisal-details")]
        public ActionResult AppraisalDetails(string slug)
        {
            NewAppraisal appraisal = _unitOfWork.Appraisal.GetAppraisalBySlug(slug);
            if (appraisal == null)
            {
                return HttpNotFound();
            }
            ViewAppraisalVM model = new ViewAppraisalVM
            {
                Appraisal = appraisal,
                AppraisalParticipants = _unitOfWork.Appraisal.GetAppraisalParticipants(slug)
            };
            return View("AppraisalDetails", model);
        }
    
        [ActionName("view-participants")]
        public ActionResult ViewParticipants(string slug)
        {
            string userId = User.Identity.GetUserId();
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(userId);
            NewAppraisal appraisal = _unitOfWork.Appraisal.GetAppraisalBySlug(slug);
            if (appraisal == null || employee == null)
            {
                return HttpNotFound();
            }
            AppraiseMembersVM model = new AppraiseMembersVM()
            {
                AppraiseeAndProgresses = _unitOfWork.Appraisal.GetDepartmentAppraisees(employee, appraisal),
                NewAppraisal = appraisal
            };
            return View("ViewParticipants", model);
        }
    
        [ActionName("hod-enter-comments")]
        public ActionResult HodEnterComments(string u, string s)
        {
            string appraiseeUserId = u;
            string newAppraisalSlug = s;
            NewAppraisal newAppraisal = _unitOfWork.Appraisal.GetAppraisalBySlug(s);
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(appraiseeUserId);
            if (employee == null || newAppraisal == null)
            {
                return HttpNotFound();
            }
            AppraisalStaff appraisal = _unitOfWork.Appraisal.GetEmployeeOngoingAppraisal(employee.Id, newAppraisal.Id);
            Appraisee appraisee = _unitOfWork.Appraisal.GetAppraisee(employee.Id, appraisal.NewAppraisalId);
            InitiatedAppraisalTemplate InitiatedAppraisalTemplate = _unitOfWork.AppraisalTemplate
                                                    .GetInitiatedAppraisalTemplateById(appraisee.InitiatedAppraisalTemplateId);
            AppraiseStaffVM model = new AppraiseStaffVM
            {
                DefaultRatings = _unitOfWork.Resources.GetDefaultRatings(),
                NewAppraisal = newAppraisal,
                Employee = employee,
                Appraisee = appraisee,
                InitiatedAppraisalTemplate = InitiatedAppraisalTemplate,
                BdsTracker = InitiatedAppraisalTemplate.IncludeBdsTracker ? _unitOfWork.Appraisal.GetBdsTracker(appraisee.BdsPerformanceTrackerId) : null
            };
            return View("HodEnterComments", model);
        }

        [ActionName("view-appraisal-pdf")]
        public ActionResult ViewAppraisalPDF(string u, string s)
        {
            string userId = u;
            string newAppraisalSlug = s;
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(userId);
            NewAppraisal newAppraisal = _unitOfWork.Appraisal.GetAppraisalBySlug(newAppraisalSlug);
            if (employee == null || newAppraisal == null)
            {
                return HttpNotFound();
            }

            Appraisee appraisee = _unitOfWork.Appraisal.GetAppraisee(employee.Id, newAppraisal.Id);
            InitiatedAppraisalTemplate InitiatedAppraisalTemplate = _unitOfWork.AppraisalTemplate
                                                    .GetInitiatedAppraisalTemplateById(appraisee.InitiatedAppraisalTemplateId);
            int hodEmployeeId = 0;
            int hrEmployeeId = 0;
            int mdEmployeeId = 0;

            if (appraisee.AppraiseeComments.HodEmployeeId != null)
            {
                hodEmployeeId = (int)appraisee.AppraiseeComments.HodEmployeeId;
            }
            if(appraisee.AppraiseeComments.HrEmployeeId != null)
            {
                hrEmployeeId = (int)appraisee.AppraiseeComments.HrEmployeeId;
            }
            if (appraisee.AppraiseeComments.MdEmployeeId != null)
            {
                mdEmployeeId = (int)appraisee.AppraiseeComments.MdEmployeeId;
            }
            AppraiseStaffVM model = new AppraiseStaffVM
            {
                DefaultRatings = _unitOfWork.Resources.GetDefaultRatings(),
                NewAppraisal = newAppraisal,
                Employee = employee,
                Appraisee = appraisee,
                InitiatedAppraisalTemplate = InitiatedAppraisalTemplate,
                HodEmployee = hodEmployeeId != 0 ? _unitOfWork.Account.GetEmployeeById(hodEmployeeId) : null,
                HrEmployee = hrEmployeeId != 0 ? _unitOfWork.Account.GetEmployeeById(hrEmployeeId) : null,
                MdEmployee = mdEmployeeId != 0 ? _unitOfWork.Account.GetEmployeeById(mdEmployeeId) : null,
                SummaryRatings = _unitOfWork.Resources.GetTemplateSummaryRatings(InitiatedAppraisalTemplate.AppraisalTemplateId),
                BdsTracker = InitiatedAppraisalTemplate.IncludeBdsTracker ? _unitOfWork.Appraisal.GetBdsTracker(appraisee.BdsPerformanceTrackerId) : null
            };
            return new ViewAsPdf("ViewAppraisalPDF", model)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4

            };
        }
    
        [ActionName("employees-in-department")]
        public ActionResult EmployeesInDepartment()
        {
            string userId = User.Identity.GetUserId();
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(userId);
            EmployeesInDepartmentVM model = new EmployeesInDepartmentVM
            {
                Department = _unitOfWork.Resources.GetDepartmentById(employee.DepartmentId),
                Employees = _unitOfWork.Office.GetAllEmployeesInDepartment(employee.DepartmentId)
            };
            return View("EmployeesInDepartment", model);
        }

        [ActionName("view-employee")]
        public ActionResult ViewEmployee(string slug)
        {
            string userId = slug;
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(userId);
            if(employee == null)
            {
                return HttpNotFound();
            }
            return View("ViewEmployee", employee);
        }


        
        [ActionName("employees-by-department-hod")]
        public ActionResult EmployeesByDepartment()
        {
            List<DepartmentAndParticipants> participants = _unitOfWork.Office.GetDepartmentAndEmployeesCount();
            return View("EmployeesByDepartment", participants);
        }

        [ActionName("view-employees-in-department-hod")]
        public ActionResult ViewEmployeesInDepartment(int slug)
        {
            int departmentId = slug;
            Department department = _unitOfWork.Resources.GetDepartmentById(departmentId);
            if (department == null)
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

        [ActionName("employees-by-location-hod")]
        public ActionResult EmployeesByLocation()
        {
            List<LocationAndEmployees> locations = _unitOfWork.Office.GetLocationAndEmployees();
            return View("EmployeesByLocation", locations);
        }

        [ActionName("view-employees-in-location-hod")]
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

        [ActionName("all-employees-hod")]
        public ActionResult AllEmployees()
        {
            List<Employee> employees = _unitOfWork.Office.GetAllEmployees();
            return View("AllEmployees", employees);
        }

        [ActionName("view-profile-hod")]
        public ActionResult ViewProfile(string slug)
        {
            string userId = slug;
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(userId);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View("ViewProfile", employee);
        }

        [ActionName("deactivated-employees-hod")]
        public ActionResult DeactivatedEmployees()
        {
            List<Employee> employees = _unitOfWork.Office.GetDeactivatedEmployees();
            return View("DeactivatedEmployees", employees);
        }

    }
}