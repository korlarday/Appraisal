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
            _unitOfWork.AppraisalTemplate.SaveNewAppraisalTemplate(model);
            return Ok();
        }
    }
}
