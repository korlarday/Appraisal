using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Services
{
    public class ComposeEmail
    {
        public static string EmailVerification(string token)
        {
            string content = "Use " + token + " as your email verification token for the IEI appraisal application, <br><br>IEI Anchor Pensions Limited.";
            return content;
        }
        public static string ForgotPasswordToken(string token)
        {
            string content = "Your reset password token is " + token + " <br><br>IEI Anchor Pensions Limited.";
            return content;
        }
    }
}