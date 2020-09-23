using AprraisalApplication.Models;
using AprraisalApplication.Models.ApiParameters;
using AprraisalApplication.Models.MigrationModels;
using AprraisalApplication.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AprraisalApplication.Controllers.api
{
    public class ConfigurationsController : ApiController
    {
        public readonly ApplicationDbContext db;
        public readonly UnitOfWork _unitOfWork;
        public ConfigurationsController()
        {
            db = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(db);
        }


        public IHttpActionResult PostSaveAppraisalTemplate([FromBody] AppraisalTemplateParams model)
        {
            string result = _unitOfWork.AppraisalTemplate.SaveNewAppraisalTemplate(model);
            return Ok(result);
        }

        public IHttpActionResult PostEditAppraisalTemplate([FromBody] AppraisalTemplateParams model)
        {
            string response = _unitOfWork.AppraisalTemplate.EditAppraisalTemplate(model);
            return Ok(response);
        }

        public IHttpActionResult PostDeleteAppraisalSection([FromBody] AppraisalSectionParam model)
        {
            _unitOfWork.AppraisalTemplate.DeleteAppraisalSection((int)model.SectionId);
            return Ok();
        }

        public IHttpActionResult PostDeleteAppraisalSectionDetail([FromBody] QualitativeDetail model)
        {
            _unitOfWork.AppraisalTemplate.DeleteSectionDetail((int)model.DetailId);
            return Ok();
        }

        public IHttpActionResult PostDeleteSectionBreakdown([FromBody] ItemBreakDown model)
        {
            _unitOfWork.AppraisalTemplate.DeleteSectionBreakdown((int)model.BreakdownId);
            return Ok();
        }

        public IHttpActionResult PostDeleteAppraisalTemplate([FromBody] AppraisalTemplate model)
        {
            _unitOfWork.AppraisalTemplate.DeleteAppraisalTemplate(model.Id);
            return Ok();
        }

        public IHttpActionResult PostUpdateDepartmentAppraisal([FromBody] AppraisalDepartmentParams model)
        {
            _unitOfWork.AppraisalTemplate.UpdateDepartmentAppraisals(model);
            return Ok();
        }

        public IHttpActionResult PostUpdateEmployeeAppraisal([FromBody] AppraisalUserParams model)
        {
            _unitOfWork.AppraisalTemplate.UpdateUserAppraisal(model);
            return Ok();
        }

        public IHttpActionResult PostSetEmployeeAppraiser([FromBody] UserAppraiserParams model)
        {
            _unitOfWork.Office.SetEmployeesAppraiser(model);
            return Ok();
        }

        public IHttpActionResult PostDeactivateAccount([FromBody] DeactivateUserParams model)
        {
            _unitOfWork.Account.DeactivateUserAccount(model);
            return Ok();
        }

        public IHttpActionResult PostActivateAccount([FromBody] DeactivateUserParams model)
        {
            _unitOfWork.Account.ActivateUserAccount(model);
            return Ok();
        }
    }
}
