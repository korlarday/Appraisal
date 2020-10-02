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
    [Authorize]
    [EmailConfirmation]
    [CompleteYourProfile]
    public class AppraisalController : Controller
    {
        public readonly ApplicationDbContext db;
        public readonly UnitOfWork _unitOfWork;
        public AppraisalController()
        {
            db = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(db);
        }

        [ActionName("new-appraisal")]
        public ActionResult NewAppraisal()
        {
            NewAppraisalVM model = new NewAppraisalVM
            {
                States = _unitOfWork.Resources.GetAllStates(),
                Departments = _unitOfWork.Resources.GetAllDepartments(),
                AppraisalTypes = new SelectList(_unitOfWork.Appraisal.GetAppraisalTypes(), "Id", "Name"),
                IsSubmitted = false,
                State = new List<string>(),
                Department = new List<string>(),
                AppraisalPeriodStartDate = DateTime.Now,
                AppraisalPeriodEndDate = DateTime.Now
            };
            return View("NewAppraisal", model);
        }

        [ActionName("new-appraisal")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewAppraisal(NewAppraisalVM model)
        {
            model.States = _unitOfWork.Resources.GetAllStates();
            model.Departments = _unitOfWork.Resources.GetAllDepartments();
            model.AppraisalTypes = new SelectList(_unitOfWork.Appraisal.GetAppraisalTypes(), "Id", "Name");
            if (!ModelState.IsValid)
            {
                return View("NewAppraisal", model);
            }
            // if department is not selected
            if(model.Department == null || model.Department.Count() < 1)
            {
                ModelState.AddModelError("Department", "Please select a department for the appraisal");
                return View("NewAppraisal", model);
            }
            // if state/location is not selected
            if (model.State == null || model.State.Count() < 1)
            {
                ModelState.AddModelError("State", "Please select the location for the appraisal");
                return View("NewAppraisal", model);
            }
            // check if appraisal name is unique
            NewAppraisal newAppraisal = _unitOfWork.Appraisal.GetNewAppraisalByTitle(model.AppraisalTitle);
            if(newAppraisal != null)
            {
                ModelState.AddModelError("AppraisalTitle", "Appraisal title already exists");
                return View("NewAppraisal", model);
            }

            // fetch all employees with the department and location selected
            model.AppraisalEmployees = _unitOfWork.Office.SearchEmployeesUsingDeptAndState(model.Department, model.State);
            model.IsSubmitted = true;

            return View("NewAppraisal", model);
        }
    
        [ActionName("initiated-appraisals")]
        public ActionResult InitiatedAppraisals()
        {
            List<NewAppraisal> model = _unitOfWork.Appraisal.GetAllInitiatedAppraisals();
            return View("InitiatedAppraisals", model);
        }

        [ActionName("view-appraisal")]
        public ActionResult ViewAppraisal(string slug)
        {
            NewAppraisal appraisal = _unitOfWork.Appraisal.GetAppraisalBySlug(slug);
            if(appraisal == null)
            {
                return HttpNotFound();
            }
            ViewAppraisalVM model = new ViewAppraisalVM
            {
                Appraisal = appraisal,
                AppraisalParticipants = _unitOfWork.Appraisal.GetAppraisalParticipants(slug)
            };
            return View("ViewAppraisal", model);
        }

        [ActionName("edit-appraisal")]
        public ActionResult EditAppraisal(string slug)
        {
            NewAppraisal appraisal = _unitOfWork.Appraisal.GetAppraisalBySlug(slug);
            if (appraisal == null)
            {
                return HttpNotFound();
            }
            List<Employee> selectedEmployee = new List<Employee>();
            List<Employee> otherEmployee = new List<Employee>();

            List<Employee> employees = _unitOfWork.Office.GetAllEmployees();
            foreach (var employee in employees)
            {
                if(appraisal.AppraisalStaffs.Select(x => x.EmployeeId).Contains(employee.Id))
                {
                    selectedEmployee.Add(employee);
                }
                else
                {
                    otherEmployee.Add(employee);
                }
            }
            ViewAppraisalVM model = new ViewAppraisalVM
            {
                Appraisal = appraisal,
                SelectedEmployees = selectedEmployee,
                OtherEmployees = otherEmployee,
                AppraisalTypes = new SelectList(_unitOfWork.Appraisal.GetAppraisalTypes(), "Id", "Name")
            };
            return View("EditAppraisal", model);
        }
    
        [ActionName("ongoing-appraisal")]
        public ActionResult OngoingAppraisal(string slug)
        {
            string newAppraisalSlug = slug;
            NewAppraisal newAppraisal = _unitOfWork.Appraisal.GetAppraisalBySlug(newAppraisalSlug);
            if(newAppraisal == null)
            {
                return HttpNotFound();
            }
            string userId = User.Identity.GetUserId();
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(userId);

            // initialize the ViewModel
            OngoingAppraisalVM model = new OngoingAppraisalVM();
            model.InitiatedAppraisalTemplate = new InitiatedAppraisalTemplate();
            model.Employee = employee;
            model.DefaultRatings = _unitOfWork.Resources.GetDefaultRatings();

            // is there an ongoing appraisal?
            AppraisalStaff appraisal = _unitOfWork.Appraisal.GetEmployeeOngoingAppraisal(employee.Id, newAppraisal.Id);
            if (appraisal == null)
                model.IsThereAnOngoingAppraisal = false;
            else 
                model.IsThereAnOngoingAppraisal = true;

            // check if appraisal has been initialized
            Appraisee appraisee = new Appraisee();
            if (model.IsThereAnOngoingAppraisal)
            {
                model.NewAppraisal = newAppraisal;
                appraisee = _unitOfWork.Appraisal.GetAppraisee(employee.Id, appraisal.NewAppraisalId);
                if (appraisee == null)
                    model.IsAppraisalInitialized = false;
                else
                    model.IsAppraisalInitialized = true;
            }
            
            if(model.IsAppraisalInitialized && model.IsThereAnOngoingAppraisal)
            {
                model.InitiatedAppraisalTemplate = _unitOfWork.AppraisalTemplate
                                                    .GetInitiatedAppraisalTemplateById(appraisee.InitiatedAppraisalTemplateId);
                model.Appraisee = appraisee;
                model.BdsTracker = _unitOfWork.Appraisal.GetBdsTracker(appraisee.BdsPerformanceTrackerId);
            }
            
            if(model.IsThereAnOngoingAppraisal && model.IsAppraisalInitialized && model.Appraisee.AppraiseeProgress.AppraiseeSubmit)
            {
                if(model.Appraisee.AppraiseeProgress.SupervisorReject && model.Appraisee.AppraiseeProgress.HODReject)
                {
                    return View("HodAndSupervisorReject", model);
                }
                if (model.Appraisee.AppraiseeProgress.SupervisorReject)
                {
                    return View("SupervisorReject", model); 
                }
                if (model.Appraisee.AppraiseeProgress.SupervisorAskForFeedback)
                {
                    return View("AppraiseeComment", model);
                }
                return View("AppraisalSubmitted", model);
            }
            return View("OngoingAppraisal", model);
        }
        
        [ActionName("previous-appraisals")]
        public ActionResult PreviousAppraisals()
        {
            string userId = User.Identity.GetUserId();
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(userId);
            List<NewAppraisal> newAppraisals = _unitOfWork.Appraisal.GetEmployeeCompletedAppraisals(employee.Id);
            return View("PreviousAppraisals", newAppraisals);
        }
        public ActionResult Appraisees()
        {
            string applicationUserId = User.Identity.GetUserId();
            AppraiseesVM model = new AppraiseesVM()
            {
                Appraisees = _unitOfWork.Appraisal.GetMyAppraisees(applicationUserId)
            };
            return View(model);
        }
    
        [ActionName("appraise-members")]
        public ActionResult AppraiseMembers()
        {
            string userId = User.Identity.GetUserId();
            AppraiseMembersVM model = new AppraiseMembersVM()
            {
                AppraiseeAndProgresses = _unitOfWork.Appraisal.GetAppraiseesAndProgress(userId)
            };
            return View("AppraiseMembers", model);
        }

        [ActionName("appraise-staff")]
        public ActionResult AppraiseStaff(string u, string s)
        {
            string appraiseeUserId = u;
            string newAppraisalSlug = s;
            NewAppraisal newAppraisal = _unitOfWork.Appraisal.GetAppraisalBySlug(newAppraisalSlug);
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
                NewAppraisal = _unitOfWork.Appraisal.GetNewAppraisalById(appraisal.NewAppraisalId),
                Employee = employee,
                Appraisee = appraisee,
                InitiatedAppraisalTemplate = InitiatedAppraisalTemplate,
                BdsTracker = appraisee.InitiatedAppraisalTemplate.IncludeBdsTracker ? _unitOfWork.Appraisal.GetBdsTracker(appraisee.BdsPerformanceTrackerId) : null
            };
            return View("AppraiseStaff", model);
        }
    
        [ActionName("appraiser-comments")]
        public ActionResult AppraiserComments(string u, string s)
        {
            string appraiseeUserId = u;
            string newAppraisalSlug = s;
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(appraiseeUserId);
            NewAppraisal newAppraisal = _unitOfWork.Appraisal.GetAppraisalBySlug(newAppraisalSlug);
            if(employee == null || newAppraisal == null)
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
            return View("AppraiserComments", model);
        }
    
        [ActionName("supervisor-makes-corrections")]
        public ActionResult SupervisorMakesCorrections(string u, string s)
        {
            string appraiseeUserId = u;
            string newAppraisalSlug = s;
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(appraiseeUserId);
            NewAppraisal newAppraisal = _unitOfWork.Appraisal.GetAppraisalBySlug(newAppraisalSlug);
            if(employee == null || newAppraisal == null)
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
                InitiatedAppraisalTemplate = InitiatedAppraisalTemplate
            };
            return View("SupervisorMakesCorrections", model);
        }
        
        [ActionName("group-participants")]
        public ActionResult GroupParticipants(string slug)
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
            return View("GroupParticipants", model);
        }
        
        [ActionName("view-participants-hr")]
        public ActionResult ViewParticipants(int d, string s)
        {
            Department department = _unitOfWork.Resources.GetDepartmentById(d);
            NewAppraisal appraisal = _unitOfWork.Appraisal.GetAppraisalBySlug(s);
            if(department == null || appraisal == null)
            {
                return HttpNotFound();
            }

            AppraiseMembersVM model = new AppraiseMembersVM()
            {
                AppraiseeAndProgresses = _unitOfWork.Appraisal.GetAppraiseesAndProgressInDepartment(department, appraisal),
                NewAppraisal = appraisal
            };
            return View("ViewParticipants", model);
        }
        
        [ActionName("hr-comments")]
        public ActionResult HRComments(string slug, string n)
        {
            string appraiseeUserId = slug;
            string newAppraisalSlug = n;
            NewAppraisal newAppraisal = _unitOfWork.Appraisal.GetAppraisalBySlug(newAppraisalSlug);
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(appraiseeUserId);
            if (employee == null || newAppraisal == null)
            {
                return HttpNotFound();
            }
            Appraisee appraisee = _unitOfWork.Appraisal.GetAppraisee(employee.Id, newAppraisal.Id);
            InitiatedAppraisalTemplate InitiatedAppraisalTemplate = _unitOfWork.AppraisalTemplate
                                                    .GetInitiatedAppraisalTemplateById(appraisee.InitiatedAppraisalTemplateId);
            //int hodEmployeeId = (int)appraisee.AppraiseeComments.HodEmployeeId;
            AppraiseStaffVM model = new AppraiseStaffVM
            {
                DefaultRatings = _unitOfWork.Resources.GetDefaultRatings(),
                NewAppraisal = newAppraisal,
                Employee = employee,
                Appraisee = appraisee,
                InitiatedAppraisalTemplate = InitiatedAppraisalTemplate,
                BdsTracker = InitiatedAppraisalTemplate.IncludeBdsTracker ? _unitOfWork.Appraisal.GetBdsTracker(appraisee.BdsPerformanceTrackerId) : null
            };
            return View("HRComments", model);
        }
    
        [ActionName("view-previous-appraisal")]
        public ActionResult ViewPreviousAppraisal(string slug)
        {
            string appraiseeUserId = User.Identity.GetUserId();
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(appraiseeUserId);
            if (employee == null)
            {
                return HttpNotFound();
            }
            NewAppraisal newAppraisal = _unitOfWork.Appraisal.GetAppraisalBySlug(slug);
            Appraisee appraisee = _unitOfWork.Appraisal.GetAppraisee(employee.Id, newAppraisal.Id);
            
            InitiatedAppraisalTemplate InitiatedAppraisalTemplate = _unitOfWork.AppraisalTemplate
                                                    .GetInitiatedAppraisalTemplateById(appraisee.InitiatedAppraisalTemplateId);
            AppraiseStaffVM model = new AppraiseStaffVM
            {
                DefaultRatings = _unitOfWork.Resources.GetDefaultRatings(),
                NewAppraisal = newAppraisal,
                Employee = employee,
                Appraisee = appraisee,
                InitiatedAppraisalTemplate = InitiatedAppraisalTemplate,
                SummaryRatings = _unitOfWork.Resources.GetTemplateSummaryRatings(InitiatedAppraisalTemplate.AppraisalTemplateId),
                BdsTracker = InitiatedAppraisalTemplate.IncludeBdsTracker ? _unitOfWork.Appraisal.GetBdsTracker(appraisee.BdsPerformanceTrackerId) : null
            };
            //return View("ViewPreviousAppraisal", model);
            return new ViewAsPdf("ViewPreviousAppraisal", model)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4

            };
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
            return new ViewAsPdf("ViewAppraisalPDF", model)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4

            };
        }
    
        [ActionName("ongoing-appraisals-all")]
        public ActionResult OngoingAppraisalsAll()
        {
            string userId = User.Identity.GetUserId();
            Employee employee = _unitOfWork.Account.GetEmployeeByUserId(userId);
            AppraiseMembersVM model = new AppraiseMembersVM()
            {
                AppraiseeAndProgresses = _unitOfWork.Appraisal.GetMyOngoingAppraisalsAndProgress(employee)
            };
            return View("OngoingAppraisalsAll", model);
        }
    }
}