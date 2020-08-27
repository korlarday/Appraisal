using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class SetupEmployeeTemplateVM
    {
        public List<AppraisalTemplate> Templates { get; set; }
        public List<EmployeeAndTemplate> EmployeeAndTemplates { get; set; }
    }
    public class EmployeeAndTemplate
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int TemplateId { get; set; }
    }
}