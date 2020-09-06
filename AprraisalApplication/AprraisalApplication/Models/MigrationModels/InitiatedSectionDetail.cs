using AprraisalApplication.Models.ApiParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class InitiatedSectionDetail
    {
        public int Id { get; set; }
        public int InitiatedTemplateSectionId { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public int MaxScore { get; set; }
        public bool HasBreakDown { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<InitiatedSectionDetailBreakdown> InitiatedSectionDetailBreakdowns { get; set; }
        public InitiatedSectionDetail()
        {
            InitiatedSectionDetailBreakdowns = new Collection<InitiatedSectionDetailBreakdown>();
        }

        public InitiatedSectionDetail(AppraisalSectionDetail detail, int initiatedSectionId)
        {
            InitiatedTemplateSectionId = initiatedSectionId;
            Title1 = detail.Title1;
            Title2 = detail.Title2;
            MaxScore = detail.MaxScore;
            HasBreakDown = detail.HasBreakDown;
            IsDeleted = detail.IsDeleted;
        }

        internal void Update(QualitativeDetail qualitativeDetail)
        {
            Title1 = qualitativeDetail.Title;
            MaxScore = qualitativeDetail.Weight;
        }
    }
}