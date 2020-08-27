using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraisalStaff
    {
        public int Id { get; set; }
        public int NewAppraisalStaff { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        public bool IsCompleted { get; set; }
    }
}