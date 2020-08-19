using AprraisalApplication.Models.ApiParameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class CareerHistory
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        [Required]
        public DateTime Date { get; set; }
        public Department Department { get; set; }

        public Grade Grade { get; set; }
        [Required]
        [Display(Name = "Grade")]
        public int GradeId { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public string TrainingAttended { get; set; }


        public CareerHistory()
        {

        }
        public CareerHistory(CareerHistoryParams model, int employeeId)
        {
            EmployeeId = employeeId;
            string hold = model.Date.ToString("yyyy-MM-dd HH:mm:ss");
            Date = Convert.ToDateTime(hold);
            DepartmentId = model.DepartmentId;
            GradeId = model.GradeId;
            TrainingAttended = model.Training;
        }
        internal void Update(CareerHistoryParams model)
        {
            string hold = model.Date.ToString("yyyy-MM-dd HH:mm:ss");
            Date = Convert.ToDateTime(hold);
            DepartmentId = model.DepartmentId;
            GradeId = model.GradeId;
            TrainingAttended = model.Training;
        }
    }
}