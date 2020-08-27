using AprraisalApplication.Models.ApiParameters;
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
        public bool IsDeleted { get; set; }
        public ICollection<SectionDetailBreakdown> SectionDetailBreakdowns { get; set; }
        public AppraisalSectionDetail()
        {
            SectionDetailBreakdowns = new Collection<SectionDetailBreakdown>();
        }

        public AppraisalSectionDetail(QualitativeDetail qualitativeDetail, int templateSectionId)
        {
            Title1 = qualitativeDetail.Title;
            Title2 = "";
            MaxScore = qualitativeDetail.Weight;
            AppraisalTemplateSectionId = templateSectionId;
        }

        internal void Update(QualitativeDetail qualitativeDetail)
        {
            Title1 = qualitativeDetail.Title;
            MaxScore = qualitativeDetail.Weight;
        }
    }
}