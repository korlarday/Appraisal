using AprraisalApplication.Models.ApiParameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraisalDepartmentTemplate
    {
        public AppraisalDepartmentTemplate()
        {

        }
        public AppraisalDepartmentTemplate(AppraisalDepartmentItem item)
        {
            DepartmentId = item.DepartmentId;
            AppraisalTemplateId = (int)item.TemplateId;
        }

        public int Id { get; set; }

        public Department Department { get; set; }
        [Required]
        public int DepartmentId { get; set; }

        public AppraisalTemplate AppraisalTemplate { get; set; }
        [Required]
        public int AppraisalTemplateId { get; set; }
    }
}