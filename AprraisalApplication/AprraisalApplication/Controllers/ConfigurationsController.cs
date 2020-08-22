using AprraisalApplication.Models;
using AprraisalApplication.Models.MigrationModels;
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
    }
}