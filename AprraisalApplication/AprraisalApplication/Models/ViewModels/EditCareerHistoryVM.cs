using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprraisalApplication.Models.ViewModels
{
    public class EditCareerHistoryVM
    {
        public Employee Employee { get; set; }

        [Display(Name = "Department")]
        [Required]
        public int DepartmentId { get; set; }
        // for loading title dropdown
        public IEnumerable<SelectListItem> Departments { get; set; }

        [Display(Name = "Grade")]
        [Required]
        public int GradeId { get; set; }
        // for loading grade dropdown
        public IEnumerable<SelectListItem> Grades { get; set; }

        public CareerHistory CreateCareerHistory { get; set; }
    }
}