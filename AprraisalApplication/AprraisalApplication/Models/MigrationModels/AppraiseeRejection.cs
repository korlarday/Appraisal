using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraiseeRejection
    {
        
        public int Id { get; set; }
        public int AppraiseeId { get; set; }
        public bool New { get; set; }
        public DateTime DateRejected { get; set; }
        public string RejectionReason { get; set; }
        public Employee RejectedBy { get; set; }
        public int RejectedById { get; set; }
        public string RejectedByPosition { get; set; }
        public AppraiseeRejection()
        {

        }
        public AppraiseeRejection(int appraiseeId, string rejectionReason, int employeeId, string rejectedByPosition)
        {
            AppraiseeId = appraiseeId;
            RejectionReason = rejectionReason;
            RejectedById = employeeId;
            DateRejected = DateTime.Now;
            New = true;
            RejectedByPosition = rejectedByPosition;
        }
    }
}