using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class AppraiseMembersVM
    {
        public List<AppraiseeAndProgress> AppraiseeAndProgresses { get; set; }

    }
    public class AppraiseeAndProgress
    {
        public Employee Employee { get; set; }
        public AppraiseeProgress AppraiseeProgress { get; set; }
    }
}