using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraisalDepartment
    {
        public int Id { get; set; }
        public int NewAppraisalId { get; set; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
    }
}