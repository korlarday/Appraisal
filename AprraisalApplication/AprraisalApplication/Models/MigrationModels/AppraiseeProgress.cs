using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraiseeProgress
    {
        public AppraiseeProgress()
        {
            AppraiseeSubmit = false;
            SupervisorSubmit = false;
            SupervisorReject = false;
            HODSubmit = false;
            HODReject = false;
            HRSubmit = false;
            HRReject = false;
            MDAcknowledgement = false;
        }
        public int Id { get; set; }
        public bool AppraiseeSubmit { get; set; }
        public bool SupervisorSubmit { get; set; }
        public bool SupervisorReject { get; set; }
        public bool HODSubmit { get; set; }
        public bool HODReject { get; set; }
        public bool HRSubmit { get; set; }
        public bool HRReject { get; set; }
        public bool MDAcknowledgement { get; set; }
    }
}