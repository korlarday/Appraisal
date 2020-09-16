using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprraisalApplication.Models.ViewModels
{
    public class ViewAppraisalVM
    {
        public NewAppraisal Appraisal { get; set; }
        public List<AppraisalParticipants> AppraisalParticipants { get; set; }
        public List<Employee> SelectedEmployees { get; set; }
        public List<Employee> OtherEmployees { get; set; }
        // for loading Appraisal Types
        public IEnumerable<SelectListItem> AppraisalTypes { get; set; }
        public List<DepartmentAndParticipants> DepartmentAndParticipants { get; set; }
    }
    public class AppraisalParticipants
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public AppraisalTemplate AppraisalTemplate { get; set; }
    }

    public class DepartmentAndParticipants
    {
        public Department Department { get; set; }
        public int NumberOfParticipants { get; set; }
    }
}