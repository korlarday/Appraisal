using AprraisalApplication.Models.ApiParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraisalTemplate
    {
        public int Id { get; set; }
        public int NumberOfSections { get; set; }
        public string TemplateName { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }

        public ICollection<AppraisalTemplateSection> AppraisalTemplateSections { get; set; }
        public ICollection<TemplateRating> TemplateRatings { get; set; }
        public ICollection<TemplateSummaryRating> TemplateSummaryRatings { get; set; }
        public AppraisalTemplate()
        {
            AppraisalTemplateSections = new Collection<AppraisalTemplateSection>();
            TemplateRatings = new Collection<TemplateRating>();
            TemplateSummaryRatings = new Collection<TemplateSummaryRating>();
        }

        public AppraisalTemplate(AppraisalTemplateParams model)
        {
            NumberOfSections = model.AppraisalSectionParams.Count();
            TemplateName = model.TemplateName;
            Slug = model.TemplateName.ToLower().Replace(" ", "-").Replace(".", "-").Replace(",", "-");
            DateCreated = DateTime.Now;
            IsDeleted = false;
            Description = model.TemplateDescription;
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