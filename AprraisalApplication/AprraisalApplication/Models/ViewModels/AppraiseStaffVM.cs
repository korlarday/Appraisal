using AprraisalApplication.Models.Attributes;
using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class AppraiseStaffVM
    {
        public InitiatedAppraisalTemplate InitiatedAppraisalTemplate { get; set; }
        public Appraisee Appraisee { get; set; }
        public NewAppraisal NewAppraisal { get; set; }
        public Employee Employee { get; set; }
        public List<DefaultRating> DefaultRatings { get; set; }
        public List<TemplateSummaryRating> SummaryRatings { get; set; }
        public Employee HodEmployee { get; set; }
        public Employee HrEmployee { get; set; }
        public Employee MdEmployee { get; set; }
        public BdsPerformanceTracker BdsTracker { get; set; }
        public bool FromAllParticipants { get; set; }
    }
}