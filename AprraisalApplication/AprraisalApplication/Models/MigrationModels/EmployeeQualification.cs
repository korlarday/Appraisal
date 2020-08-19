using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class EmployeeQualification
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Qualification Qualification { get; set; }
        public int QualificationId { get; set; }

        public EmployeeQualification()
        {

        }
        public EmployeeQualification(int employeeId, int qualificationId)
        {
            EmployeeId = employeeId;
            QualificationId = qualificationId;
        }
    }
}