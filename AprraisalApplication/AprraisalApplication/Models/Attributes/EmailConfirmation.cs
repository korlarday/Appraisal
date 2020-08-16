using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprraisalApplication.Models.Attributes
{
    public class EmailConfirmation : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
           
            if (user != null && user.EmailConfirmed == false)
            {
                string confirmationUrl = "/Account/email-confirmation";

                filterContext.Result = new RedirectResult(confirmationUrl);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}