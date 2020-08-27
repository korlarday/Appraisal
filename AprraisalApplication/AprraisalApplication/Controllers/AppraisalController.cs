using AprraisalApplication.Models.Attributes;
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
        [ActionName("new-appraisal")]
        public ActionResult NewAppraisal()
        {
            return View();
        }
    }
}