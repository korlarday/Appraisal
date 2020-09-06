using AprraisalApplication.Models;
using AprraisalApplication.Models.ApiParameters;
using AprraisalApplication.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AprraisalApplication.Controllers.api
{
    public class AppraisalController : ApiController
    {
        public readonly ApplicationDbContext db;
        public readonly UnitOfWork _unitOfWork;
        public AppraisalController()
        {
            db = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(db);
        }

        public IHttpActionResult PostInitialiseAppraisalExercise([FromBody] NewAppraisalParams model)
        {
            _unitOfWork.Appraisal.InitiateNewAppraisal(model);
            return Ok();
        }

        public IHttpActionResult PostUpdateAppraisalExercise([FromBody] NewAppraisalParams model)
        {
            _unitOfWork.Appraisal.UpdateAppraisal(model);
            return Ok();
        }

        public IHttpActionResult PostStartEmployeeAppraisal([FromBody] StartEmployeeAppraisalParams model)
        {
            string response = _unitOfWork.Appraisal.StartEmployeeAppraisal(model.EmployeeId, model.NewAppraisalId);
            return Ok(response);
        }

        public IHttpActionResult PostSubmitAppraisalToSupervisor([FromBody] SubmitAppraisalParams model)
        {
            _unitOfWork.Appraisal.SubmitAppraisalToSupervisor(model);
            return Ok();
        }
    }
}
