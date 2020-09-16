using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraisalStaff
    {
        public int Id { get; set; }
        public int NewAppraisalId { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DateCreated { get; set; }
        public AppraisalStaff()
        {

        }
        public AppraisalStaff(int newAppraisalId, int employeeId)
        {
            NewAppraisalId = newAppraisalId;
            EmployeeId = employeeId;
            IsCompleted = false;
            DateCreated = DateTime.Now;
        }
    }
}