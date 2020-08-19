using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraisalSectionDetail
    {
        public int Id { get; set; }
        public int AppraisalTemplateSectionId { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public int MaxScore { get; set; }
        public bool HasBreakDown { get; set; }
        public ICollection<SectionDetailBreakdown> SectionDetailBreakdowns { get; set; }
        public AppraisalSectionDetail()
        {
            SectionDetailBreakdowns = new Collection<SectionDetailBreakdown>();
        }
    }
}