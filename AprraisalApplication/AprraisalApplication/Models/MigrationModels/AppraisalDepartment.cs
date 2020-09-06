using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraisalDepartment
    {
        public AppraisalDepartment()
        {

        }
        public AppraisalDepartment(int newAppraisalId, int departmentId)
        {
            NewAppraisalId = newAppraisalId;
            DepartmentId = departmentId;
        }

        public int Id { get; set; }
        public int NewAppraisalId { get; set; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
    }
}