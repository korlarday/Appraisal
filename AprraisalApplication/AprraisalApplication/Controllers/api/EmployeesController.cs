using AprraisalApplication.Models;
using AprraisalApplication.Models.ApiModels;
using AprraisalApplication.Models.ApiParameters;
using AprraisalApplication.Models.MigrationModels;
using AprraisalApplication.Persistence;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AprraisalApplication.Controllers.api
{
    public class EmployeesController : ApiController
    {
        public readonly ApplicationDbContext db;
        public readonly UnitOfWork _unitOfWork;
        public EmployeesController()
        {
            db = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(db);
        }

        [HttpPost]
        public IHttpActionResult PostSaveCareerHistory([FromBody] CareerHistoryParams model)
        {
            CareerHistory history = _unitOfWork.Account.SaveCareerHistory(model);
            NewHistoryApiVM historyApiVM = new NewHistoryApiVM(history);
            return Ok(historyApiVM);
        }

        [HttpPost]
        public IHttpActionResult PostSaveCareerHistoryHr([FromBody] CareerHistoryParams model)
        {
            CareerHistory history = _unitOfWork.Account.SaveCareerHistoryHr(model);
            NewHistoryApiVM historyApiVM = new NewHistoryApiVM(history);
            return Ok(historyApiVM);
        }

        [HttpPost]
        public IHttpActionResult PostEditCareerHistory([FromBody] CareerHistoryParams model)
        {
            CareerHistory history = _unitOfWork.Account.UpdateCareerHistory(model);
            NewHistoryApiVM historyApiVM = new NewHistoryApiVM(history);
            return Ok(historyApiVM);
        }

        [HttpPost]
        public IHttpActionResult PostDeleteBranch([FromBody] CareerHistoryParams model)
        {
            _unitOfWork.Account.DeleteCareerHistory(model.Id);
            return Ok();
        }

    }
}
