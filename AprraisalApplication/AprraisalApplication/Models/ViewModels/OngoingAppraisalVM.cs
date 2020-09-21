using AprraisalApplication.Models.Attributes;
using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class OngoingAppraisalVM
    {
        public bool IsThereAnOngoingAppraisal { get; set; }
        public bool IsAppraisalInitialized { get; set; }
        public InitiatedAppraisalTemplate InitiatedAppraisalTemplate { get; set; }
        public Appraisee Appraisee { get; set; }
        public NewAppraisal NewAppraisal { get; set; }
        public Employee Employee { get; set; }
        public List<DefaultRating> DefaultRatings { get; set; }
        public BdsPerformanceTracker BdsTracker { get; set; }
    }
}