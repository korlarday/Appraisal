using AprraisalApplication.Models.ApiParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class InitiatedAppraisalTemplate
    {
        public int Id { get; set; }
        public int NewAppraiserId { get; set; }
        public int AppraisalTemplateId { get; set; }
        public int NumberOfSections { get; set; }
        public string TemplateName { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }

        public ICollection<InitiatedTemplateSection> InitiatedTemplateSections { get; set; }
        public ICollection<TemplateRating> TemplateRatings { get; set; }
        public ICollection<TemplateSummaryRating> TemplateSummaryRatings { get; set; }
        public InitiatedAppraisalTemplate()
        {
            InitiatedTemplateSections = new Collection<InitiatedTemplateSection>();
            TemplateRatings = new Collection<TemplateRating>();
            TemplateSummaryRatings = new Collection<TemplateSummaryRating>();
        }

        public InitiatedAppraisalTemplate(AppraisalTemplate template, int newAppraisalId)
        {
            AppraisalTemplateId = template.Id;
            NewAppraiserId = newAppraisalId;
            NumberOfSections = template.NumberOfSections;
            TemplateName = template.TemplateName;
            Slug = template.Slug;
            Description = template.Description;
            DateCreated = DateTime.Now;
            IsDeleted = template.IsDeleted;
        }

        internal void UpdateTemplate(AppraisalTemplateParams model)
        {
            NumberOfSections = model.AppraisalSectionParams.Count();
            TemplateName = model.TemplateName;
            Slug = model.TemplateName.ToLower().Replace(" ", "-").Replace(".", "-").Replace(",", "-");
            Description = model.TemplateDescription;
        }
    }
}