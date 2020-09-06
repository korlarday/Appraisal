using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraiseeCareerHistoryWithCompany
    {
        public AppraiseeCareerHistoryWithCompany()
        {

        }
        public AppraiseeCareerHistoryWithCompany(CareerHistory history, int appraisalId)
        {
            EmployeeId = history.EmployeeId;
            AppraiseeId = appraisalId;
            Date = history.Date;
            DepartmentId = history.DepartmentId;
            GradeId = history.GradeId;
            TrainingAttended = history.TrainingAttended;
        }

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int AppraiseeId { get; set; }
        public DateTime Date { get; set; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public Grade Grade { get; set; }
        public int GradeId { get; set; }
        public string TrainingAttended { get; set; }
    }
}