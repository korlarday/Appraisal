using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class DefaultUserAppraiser
    {
        public int Id { get; set; }
        public string AppraiseeId { get; set; }
        public ApplicationUser Appraisee { get; set; }
        public string AppraiserId { get; set; }
        public bool ToTeamLeader { get; set; }
        public bool SubmissionToHOD { get; set; } // to define if the Appraiser will go to HOD. HOD can do the appraiser exercise, to this will be false for him/her.
        public bool SubmissionToHR { get; set; } // dis will be true for HODs
        public bool SubmissionToMD { get; set; } // In the case of HR's appraiser, he submits to the MD directly
        public DefaultUserAppraiser()
        {
            AppraiseeId = HttpContext.Current.User.Identity.GetUserId();
            AppraiserId = String.Empty;
            ToTeamLeader = false;
            SubmissionToHOD = false;
            SubmissionToHR = false;
            SubmissionToMD = false;
        }
    }
}