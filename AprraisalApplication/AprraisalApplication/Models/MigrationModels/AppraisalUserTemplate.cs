using AprraisalApplication.Models.ApiParameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraisalUserTemplate
    {
        public AppraisalUserTemplate()
        {

        }
        public AppraisalUserTemplate(AppraisalUserItem item)
        {
            ApplicationUserId = item.ApplicationUserId;
            AppraisalTemplateId = (int)item.TemplateId;
        }

        public int Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }

        public AppraisalTemplate AppraisalTemplate { get; set; }
        [Required]
        public int AppraisalTemplateId { get; set; }
    }
}