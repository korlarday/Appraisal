using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class SetupDepartmentTemplateVM
    {
        public List<Department> Departments { get; set; }
        public List<AppraisalDepartmentTemplate> AppraisalDepartmentTemplates { get; set; }
        public List<AppraisalTemplate> Templates { get; set; }
        public List<DepartmentAndTemplate> DepartmentAndTemplates { get; set; }
    }
    public class DepartmentAndTemplate
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int TemplateId { get; set; }
    }
}