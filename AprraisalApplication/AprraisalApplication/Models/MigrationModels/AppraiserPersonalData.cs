using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraiserPersonalData
    {
        public int Id { get; set; }
        public Employee Appraiser { get; set; }
        public int AppraiserId { get; set; }
        public JobTitle JobTitle { get; set; }
        public int JobTitleId { get; set; }
        public Grade Grade { get; set; }
        public int GradeId { get; set; }
    }
}