using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ApiModels
{
    public class NewHistoryApiVM
    {
        public NewHistoryApiVM()
        {

        }
        public NewHistoryApiVM(CareerHistory history)
        {
            Id = history.Id;
            EmployeeId = history.EmployeeId;
            Date = history.Date;
            Year = history.Date.Year;
            Month = history.Date.Month;
            Day = history.Date.Day;
            Department = history.Department;
            Grade = history.Grade;
            GradeId = history.GradeId;
            DepartmentId = history.DepartmentId;
            TrainingAttended = history.TrainingAttended;
        }

        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public DateTime Date { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public Department Department { get; set; }

        public Grade Grade { get; set; }
        public int GradeId { get; set; }

        public int DepartmentId { get; set; }
        public string TrainingAttended { get; set; }
    }
}