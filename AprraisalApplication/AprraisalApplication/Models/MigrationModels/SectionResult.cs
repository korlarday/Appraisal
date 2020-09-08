using AprraisalApplication.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class SectionResult
    {

        public int Id { get; set; }
        public int AppraiseeId { get; set; }
        public InitiatedTemplateSection InitiatedTemplateSection { get; set; }
        public int InitiatedTemplateSectionId { get; set; }
        public double PercentageScore { get; set; }
        public double TotalScore { get; set; }
        public ICollection<SectionDetailResult> SectionDetailResults { get; set; }
        public SectionResult()
        {
            SectionDetailResults = new Collection<SectionDetailResult>();
        }

        public SectionResult(int appraiseeId, int initiatedTemplateSectionId)
        {
            AppraiseeId = appraiseeId;
            InitiatedTemplateSectionId = initiatedTemplateSectionId;
            PercentageScore = 0;
            TotalScore = 0;
        }
    }
}