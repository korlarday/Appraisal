using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Services
{
    public class EmailTemps
    {
        public static string Body(string mail)
        {
            string mailbody = "";
            mailbody += "<p>Hello</p>";
            mailbody += mail;
            mailbody += "<p>Thank you</p>";
            mailbody += "<p>Please do not reply to this mail</p>";
            mailbody += "<p>IEI - Anchor Pensions Manager Ltd <br />";
            mailbody += "No. 22 Otukpo street, off Gimbiya street, Area 11, Garki, Abuja <br />";
            mailbody += "website: https://www.ieianchorpensions.com <br />";
            return mailbody;
        }

        public static string AppraisalStartEmail()
        {
            string message = "<p>This is to notify you that the appraisal exercise has just commenced.</p>";
            message += "<p>Kindly login to the appraisal application to complete the exercise</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal'>Click here to login</a></p>";
            return message;
        }

        public static string AppraiseeSubmitsToSupervisor(string name)
        {
            string message = "<p>This is to notify you that your team member " + name + " has just submitted his/her appraisal to you.</p>";
            message += "<p>Kindly login to mark the appraisal</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/appraisal/appraise-members'>Click here to view appraisal</a></p>";
            return message;
        }

        public static string SupervisorSubmitsToAppraiseeForReview()
        {
            string message = "<p>This is to notify you that your supervisor has just submitted your appraisal to you for your comments.</p>";
            message += "<p>Kindly login to mark the appraisal</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/appraisal/ongoing-appraisals-all'>Click here to view appraisal</a></p>";
            return message;
        }

        public static string AppraiseeSubmitsCommentsToSupervisor(string name)
        {
            string message = "<p>This is to notify you that your team member " + name + " has just submitted his/her appraisal to you for your review.</p>";
            message += "<p>Kindly login to comment on the appraisal</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/appraisal/appraise-members'>Click here to view appraisal</a></p>";
            return message;
        }

        public static string SupervisorSubmitsToHod(string name)
        {
            string message = "<p>This is to notify you that  " + name + "'s appraisal has been submitted to you for your review.</p>";
            message += "<p>Kindly login to comment on the appraisal</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/appraisal/appraise-members'>Click here to view appraisal</a></p>";
            return message;
        }

        public static string SupervisorSubmitsToHr(string name)
        {
            string message = "<p>This is to notify you that  " + name + "'s appraisal has been submitted to you for your review.</p>";
            message += "<p>Kindly login to comment on the appraisal</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/appraisal/initiated-appraisals'>Click here to view appraisal</a></p>";
            return message;
        }
    }
}