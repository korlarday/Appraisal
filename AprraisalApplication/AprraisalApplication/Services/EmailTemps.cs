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
            message += "<p>Kindly login to view the appraisal</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/appraisal/appraise-members'>Click here to view appraisal</a></p>";
            return message;
        }

        public static string SupervisorSubmitsToAppraiseeForReview()
        {
            string message = "<p>This is to notify you that your supervisor has just submitted your appraisal to you for your comments.</p>";
            message += "<p>Kindly login to view the appraisal</p>";
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
            string message = "<p>This is to notify you that " + name + "'s appraisal has been submitted to you for your review.</p>";
            message += "<p>Kindly login to comment on the appraisal</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/departmentAppraisal/department-initiated-appraisals'>Click here to view appraisal</a></p>";
            return message;
        }

        public static string SupervisorSubmitsToHr(string name)
        {
            string message = "<p>This is to notify you that  " + name + "'s appraisal has been submitted to you for your review.</p>";
            message += "<p>Kindly login to comment on the appraisal</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/appraisal/initiated-appraisals'>Click here to view appraisal</a></p>";
            return message;
        }

        internal static string HrSubmitsCommentsToMd(string name, string department)
        {
            string message = "<p>This is to notify you that " + name + "'s appraisal has been submitted to you by the HR for your review.</p>";
            message += "<p>Department: " + department + "</p>";
            message += "<p>Kindly login to comment on the appraisal</p>";
            message += "<p><a href='https://localhost:44359/mdappraisal/initiated-appraisals-md'>Click here to view appraisal</a></p>";
            return message;
        }

        internal static string MdCommentsOnAppraisal(string name, string department)
        {
            string message = "<p>This is to notify you that the MD has just commented on " + name + "'s appraisal.</p>";
            message += "<p>Department: " + department + "</p>";
            message += "<p>Kindly login to view</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/appraisal/initiated-appraisals'>Click here to view appraisal</a></p>";
            return message;
        }

        internal static string HODCommentsOnAppraisal(string appraiseeName, string departmentName, string hodName)
        {
            string message = "<p>This is to notify you that the HOD, " + hodName + " has just submitted an employee's appraisal form to you.</p>";
            message += "<p>Employee's Name: " + appraiseeName + "</p>";
            message += "<p>Department: " + departmentName + "</p>";
            message += "<p>Kindly login to view</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/appraisal/initiated-appraisals'>Click here to view appraisal</a></p>";
            return message;
        }

        internal static string SupervisorRejectsAppraisalToAppraisee(string rejectionReason)
        {
            string message = "<p>This is to notify you that your supervisor has rejected your appraisal due to the reason below:</p>";
            message += "<p>Rejection Reason: " + rejectionReason + "</p>";
            message += "<p>Kindly login to correct the appraisal</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/appraisal/ongoing-appraisals-all'>Click here to make corrections</a></p>";
            return message;
        }

        internal static string AppraiseeResubmitsToSupervisor(string name)
        {
            string message = "<p>This is to notify you that your team member, " + name + " has just re-submitted his/her appraisal to you.</p>";
            message += "<p>Kindly login to view the appraisal</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/appraisal/appraise-members'>Click here to view appraisal</a></p>";
            return message;
        }

        internal static string HodRejectsAppraisalToAppraisee(string rejectionReason)
        {
            string message = "<p>This is to notify you that the HOD has rejected your appraisal due to the reason below:</p>";
            message += "<p>Rejection Reason: " + rejectionReason + "</p>";
            message += "<p>Kindly login to correct the appraisal</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/appraisal/ongoing-appraisals-all'>Click here to make corrections</a></p>";
            return message;
        }

        internal static string HodRejectsAppraisalToSupervisor(string appraiseeName, string rejectionReason)
        {
            string message = "<p>This is to notify you that the HOD has rejected " + appraiseeName + "'s appraisal form to you due to the reason below:</p>";
            message += "<p>Rejection Reason: " + rejectionReason + "</p>";
            message += "<p>Kindly login to view the appraisal</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/appraisal/ongoing-appraisals-all'>Click here to make corrections</a></p>";
            return message;
        }

        internal static string HRRejectsAppraisalToAppraisee(string rejectionReason)
        {
            string message = "<p>This is to notify you that the HR has rejected your appraisal due to the reason below:</p>";
            message += "<p>Rejection Reason: " + rejectionReason + "</p>";
            message += "<p>Kindly login to correct the appraisal</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/appraisal/ongoing-appraisals-all'>Click here to make corrections</a></p>";
            return message;
        }

        internal static string NotifySupervisorAboutHRRejectsAppraisalToAppraisee(string name, string rejectionReason)
        {
            string message = "<p>This is to notify you that the HR has returned the appraisal back to the appraisee due to the reason below:</p>";
            message += "<p>Appraisee: " + name + "</p>";
            message += "<p>Rejection Reason: " + rejectionReason + "</p>";
            return message;
        }

        internal static string HRRejectsAppraisalToSupervisor(string appraiseeName, string rejectionReason)
        {
            string message = "<p>This is to notify you that the HR has rejected " + appraiseeName + "'s appraisal due to the reason below:</p>";
            message += "<p>Rejection Reason: " + rejectionReason + "</p>";
            message += "<p>Kindly login to correct the appraisal</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/appraisal/appraise-members'>Click here to make corrections</a></p>";
            return message;
        }

        internal static string NotifyHodAboutHRRejectsAppraisalToSupervisor(string name, string rejectionReason)
        {
            string message = "<p>This is to notify you that the HR has returned the appraisal back to the appraisee supervisor due to the reason below:</p>";
            message += "<p>Appraisee: " + name + "</p>";
            message += "<p>Rejection Reason: " + rejectionReason + "</p>";
            return message;
        }

        internal static string HRRejectsAppraisalToHod(string appraiseeName, string rejectionReason)
        {
            string message = "<p>This is to notify you that the HR has rejected " + appraiseeName + "'s appraisal due to the reason below:</p>";
            message += "<p>Rejection Reason: " + rejectionReason + "</p>";
            message += "<p>Kindly login to view the appraisal</p>";
            message += "<p><a href='http://ffpro.ieianchorpensions.com/appraisal/departmentAppraisal/department-initiated-appraisals'>Click here to make corrections</a></p>";
            return message;
        }
    }
}