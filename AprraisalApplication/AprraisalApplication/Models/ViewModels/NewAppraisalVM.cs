using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprraisalApplication.Models.ViewModels
{
    public class NewAppraisalVM
    {
        [Required]
        [Display(Name = "Appraisal Title")]
        public string AppraisalTitle { get; set; }

        public List<State> States { get; set; }
        public List<string> State { get; set; }

        [Display(Name = "Due Date (Optional)")]
        public DateTime? DueDate { get; set; }

        [Required]
        [Display(Name = "Appraisal Type")]
        public byte AppraisalTypeId { get; set; }
        // for loading Appraisal Types
        public IEnumerable<SelectListItem> AppraisalTypes { get; set; }

        [Required]
        [Display(Name = "From")]
        public DateTime AppraisalPeriodStartDate { get; set; }

        [Required]
        [Display(Name = "To")]
        public DateTime AppraisalPeriodEndDate { get; set; }

        public List<Department> Departments { get; set; }
        public List<string> Department { get; set; }
        public bool IsSubmitted { get; set; }

        public List<Employee> AppraisalEmployees { get; set; }
        public List<AppraisalTemplate> AppraisalTemplates { get; set; }
    }
}