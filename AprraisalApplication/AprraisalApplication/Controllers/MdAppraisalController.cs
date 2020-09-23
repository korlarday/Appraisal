using AprraisalApplication.Models;
using AprraisalApplication.Models.Attributes;
using AprraisalApplication.Models.MigrationModels;
using AprraisalApplication.Models.ViewModels;
using AprraisalApplication.Persistence;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprraisalApplication.Controllers
{
    public class MdAppraisalController : Controller
    {
        public readonly ApplicationDbContext db;
        public readonly UnitOfWork _unitOfWork;
        public MdAppraisalController()
        {
            db = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(db);
        }
        [ActionName("initiated-appraisals-md")]
        public ActionResult InitiatedAppraisalsMd()
        {
            List<NewAppraisal> model = _unitOfWork.Appraisal.GetAllInitiatedAppraisals();
            return View("InitiatedAppraisalsMd", model);
        }

        [ActionName("view-appraisal-md")]
        public ActionResult ViewAppraisalMd(string slug)
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
            return View("ViewAppraisalMd", model);
        }

        [ActionName("group-participants-md")]
        public ActionResult GroupParticipantsMd(string slug)
        {
            NewAppraisal appraisal = _unitOfWork.Appraisal.GetAppraisalBySlug(slug);
            if (appraisal == null)
            {
                return HttpNotFound();
            }

            ViewAppraisalVM model = new ViewAppraisalVM
            {
                Appraisal = appraisal,
                DepartmentAndParticipants = _unitOfWork.Appraisal.GetAppraisalDeptAndParticipants(appraisal.Id)
            };
            return View("GroupParticipantsMd", model);
        }
    
        [ActionName("view-participants-md")]
        public ActionResult ViewParticipantsMd(int d, string s)
        {
            Department department = _unitOfWork.Resources.GetDepartmentById(d);
            NewAppraisal appraisal = _unitOfWork.Appraisal.GetAppraisalBySlug(s);
            if (department == null || appraisal == null)
            {
                return HttpNotFound();
            }

            AppraiseMembersVM model = new AppraiseMembersVM()
            {
                AppraiseeAndProgresses = _unitOfWork.Appraisal.GetAppraiseesAndProgressInDepartment(department, appraisal),
                NewAppraisal = appraisal
            };
            return View("ViewParticipantsMd", model);
        }
    
        [ActionName("md-comments")]
        public ActionResult MdComments(string slug, string n)
        {
            string appraiseeUserId = slug;
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(appraiseeUserId);
            NewAppraisal newAppraisal = _unitOfWork.Appraisal.GetAppraisalBySlug(n);
            if (employee == null || newAppraisal == null)
            {
                return HttpNotFound();
            }

            Appraisee appraisee = _unitOfWork.Appraisal.GetAppraisee(employee.Id, newAppraisal.Id);
            InitiatedAppraisalTemplate InitiatedAppraisalTemplate = _unitOfWork.AppraisalTemplate
                                                    .GetInitiatedAppraisalTemplateById(appraisee.InitiatedAppraisalTemplateId);
            int hodEmployeeId = (int)appraisee.AppraiseeComments.HodEmployeeId;
            int hrEmployeeId = (int)appraisee.AppraiseeComments.HrEmployeeId;
            AppraiseStaffVM model = new AppraiseStaffVM
            {
                DefaultRatings = _unitOfWork.Resources.GetDefaultRatings(),
                NewAppraisal = newAppraisal,
                Employee = employee,
                Appraisee = appraisee,
                InitiatedAppraisalTemplate = InitiatedAppraisalTemplate,
                HodEmployee = _unitOfWork.Account.GetEmployeeById(hodEmployeeId),
                HrEmployee = _unitOfWork.Account.GetEmployeeById(hrEmployeeId),
                BdsTracker = InitiatedAppraisalTemplate.IncludeBdsTracker ? _unitOfWork.Appraisal.GetBdsTracker(appraisee.BdsPerformanceTrackerId) : null
            };
            return View("MdComments", model);
        }

        [ActionName("view-appraisal-md-pdf")]
        public ActionResult ViewAppraisalMdPDF(string u, string s)
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
            if (appraisee.AppraiseeComments.HrEmployeeId != null)
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
            return new ViewAsPdf("ViewAppraisalMdPDF", model)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4

            };
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

        [ActionName("all-employees")]
        public ActionResult AllEmployees()
        {
            List<Employee> employees = _unitOfWork.Office.GetAllEmployees();
            return View("AllEmployees", employees);
        }

        [ActionName("view-profile")]
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

    }
}