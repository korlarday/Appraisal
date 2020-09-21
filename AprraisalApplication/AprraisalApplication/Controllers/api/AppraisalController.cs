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
            string result = _unitOfWork.Appraisal.UpdateAppraisal(model);
            return Ok(result);
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

        public IHttpActionResult PostScoreAppraisalSections([FromBody] SectionScoresParams model)
        {
            _unitOfWork.Appraisal.ScoreAppraisalSections(model);
            return Ok();
        }

        public IHttpActionResult PostReScoreAppraisalSections([FromBody] SectionScoresParams model)
        {
            _unitOfWork.Appraisal.ReScoreAppraisalSections(model);
            return Ok();
        }

        public IHttpActionResult PostRejectAppraisalToAppraisee([FromBody] SectionScoresParams model)
        {
            _unitOfWork.Appraisal.RejectAppraisalToAppraisee(model);
            return Ok();
        }
        
        public IHttpActionResult PostRejectAppraisalCommentsToAppraisee([FromBody] SectionScoresParams model)
        {
            _unitOfWork.Appraisal.RejectAppraisalCommentToAppraisee(model);
            return Ok();
        }
        //
        public IHttpActionResult PostRejectAppraisalToSupervisor([FromBody] SectionScoresParams model)
        {
            _unitOfWork.Appraisal.RejectAppraisalToSupervisor(model);
            return Ok();
        }

        public IHttpActionResult PostRejectAppraisalToHod([FromBody] SectionScoresParams model)
        {
            _unitOfWork.Appraisal.RejectAppraisalToHod(model);
            return Ok();
        }

        public IHttpActionResult PostRemoveSectionResultDetail([FromBody] SectionResultParams model)
        {
            _unitOfWork.Appraisal.DeleteSectionResultDetailItem(model);
            return Ok();
        }

        public IHttpActionResult PostResubmitAppraisalToSupervisor([FromBody] SubmitAppraisalParams model)
        {
            _unitOfWork.Appraisal.ResubmitAppraisalToSupervisor(model);
            return Ok();
        }

        public IHttpActionResult PostEnterAppraiseeComment([FromBody] SubmitAppraisalParams model)
        {
            _unitOfWork.Appraisal.SaveAppraiseeComment(model);
            return Ok();
        }

        public IHttpActionResult PostEnterAppraiserComment([FromBody] SubmitAppraisalParams model)
        {
            _unitOfWork.Appraisal.SaveAppraiserComment(model);
            return Ok();
        }

        public IHttpActionResult PostEnterHodComment([FromBody] SubmitAppraisalParams model)
        {
            _unitOfWork.Appraisal.SaveHodComment(model);
            return Ok();
        }
        public IHttpActionResult PostEnterHRComment([FromBody] SubmitAppraisalParams model)
        {
            _unitOfWork.Appraisal.SaveHrComment(model);
            return Ok();
        }

        public IHttpActionResult PostEnterMdComment([FromBody] SubmitAppraisalParams model)
        {
            _unitOfWork.Appraisal.SaveMdComment(model);
            return Ok();
        }
    }
}
