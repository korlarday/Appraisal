using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraiseePersonalData
    {
        public AppraiseePersonalData()
        {

        }
        public AppraiseePersonalData(int employeeId, int gradeId)
        {
            EmployeeId = employeeId;
            GradeId = gradeId;
        }

        public int Id { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        public Grade Grade { get; set; }
        public int GradeId { get; set; }
    }
}