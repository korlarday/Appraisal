﻿using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprraisalApplication.Models.Attributes
{
    public class CompleteYourProfile : ActionFilterAttribute
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            if (user != null && user.EmployeeId == null)
            {
                string profileURL = "/Users/user-profile";

                filterContext.Result = new RedirectResult(profileURL);
            }
            // for account disabling
            if(user != null && user.AccountDisabled)
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                filterContext.Result = new RedirectResult("/account/login");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}