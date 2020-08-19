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
        public TemplateRating TemplateRating { get; set; }
        public int TemplateRatingId { get; set; }

        public ICollection<AppraisalTemplateSection> AppraisalTemplateSections { get; set; }
        public ICollection<TemplateRating> TemplateRatings { get; set; }
        public ICollection<TemplateSummaryRating> TemplateSummaryRatings { get; set; }
        public AppraisalTemplate()
        {
            AppraisalTemplateSections = new Collection<AppraisalTemplateSection>();
            TemplateRatings = new Collection<TemplateRating>();
            TemplateSummaryRatings = new Collection<TemplateSummaryRating>();
        }
    }
}