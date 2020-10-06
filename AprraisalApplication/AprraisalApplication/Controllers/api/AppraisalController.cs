using AprraisalApplication.Models;
using AprraisalApplication.Models.ApiParameters;
using AprraisalApplication.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AprraisalApplication.Controllers.api
{
    [Authorize]
    public class AppraisalController : ApiController
    {
        public readonly ApplicationDbContext db;
        public readonly UnitOfWork _unitOfWork;
        public AppraisalController()
        {
            db = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(db);
        }

        public async Task<IHttpActionResult> PostInitialiseAppraisalExercise([FromBody] NewAppraisalParams model)
        {
            await _unitOfWork.Appraisal.InitiateNewAppraisal(model);
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

        public async Task<IHttpActionResult> PostSubmitAppraisalToSupervisor([FromBody] SubmitAppraisalParams model)
        {
            string response = await _unitOfWork.Appraisal.SubmitAppraisalToSupervisor(model);
            return Ok(response);
        }

        public async Task<IHttpActionResult> PostScoreAppraisalSections([FromBody] SectionScoresParams model)
        {
            string response = await _unitOfWork.Appraisal.ScoreAppraisalSections(model);
            return Ok(response);
        }

        public IHttpActionResult PostReScoreAppraisalSections([FromBody] SectionScoresParams model)
        {
            _unitOfWork.Appraisal.ReScoreAppraisalSections(model);
            return Ok();
        }

        public async Task<IHttpActionResult> PostRejectAppraisalToAppraisee([FromBody] SectionScoresParams model)
        {
            var response = await _unitOfWork.Appraisal.RejectAppraisalToAppraisee(model);
            return Ok(response);
        }
        
        public async Task<IHttpActionResult> PostRejectAppraisalCommentsToAppraisee([FromBody] SectionScoresParams model)
        {
            var response = await _unitOfWork.Appraisal.RejectAppraisalCommentToAppraisee(model);
            return Ok(response);
        }
        //
        public async Task<IHttpActionResult> PostRejectAppraisalToSupervisor([FromBody] SectionScoresParams model)
        {
            var response = await _unitOfWork.Appraisal.RejectAppraisalToSupervisor(model);
            return Ok(response);
        }

        public async Task<IHttpActionResult> PostRejectAppraisalToHod([FromBody] SectionScoresParams model)
        {
            var response = await _unitOfWork.Appraisal.RejectAppraisalToHod(model);
            return Ok(response);
        }

        public IHttpActionResult PostRemoveSectionResultDetail([FromBody] SectionResultParams model)
        {
            _unitOfWork.Appraisal.DeleteSectionResultDetailItem(model);
            return Ok();
        }

        public async Task<IHttpActionResult> PostResubmitAppraisalToSupervisor([FromBody] SubmitAppraisalParams model)
        {
            var response = await _unitOfWork.Appraisal.ResubmitAppraisalToSupervisor(model);
            return Ok(response);
        }

        public async Task<IHttpActionResult> PostEnterAppraiseeComment([FromBody] SubmitAppraisalParams model)
        {
            string response = await _unitOfWork.Appraisal.SaveAppraiseeComment(model);
            return Ok(response);
        }

        public async Task<IHttpActionResult> PostEnterAppraiserComment([FromBody] SubmitAppraisalParams model)
        {
            string response = await _unitOfWork.Appraisal.SaveAppraiserComment(model);
            return Ok(response);
        }

        public async Task<IHttpActionResult> PostEnterHodComment([FromBody] SubmitAppraisalParams model)
        {
            var response = await _unitOfWork.Appraisal.SaveHodComment(model);
            return Ok(response);
        }
        public async Task<IHttpActionResult> PostEnterHRComment([FromBody] SubmitAppraisalParams model)
        {
            var response = await _unitOfWork.Appraisal.SaveHrComment(model);
            return Ok(response);
        }

        public async Task<IHttpActionResult> PostEnterMdComment([FromBody] SubmitAppraisalParams model)
        {
            var response = await _unitOfWork.Appraisal.SaveMdComment(model);
            return Ok(response);
        }
    }
}
