using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraisalTemplateSection
    {
        public int Id { get; set; }
        public int AppraisalTemplateId { get; set; }
        public SectionType SectionType { get; set; }
        public byte SectionTypeId { get; set; }
        public string SectionTitle { get; set; }
        public string SectionInstructions { get; set; }
        public string FirstColumnHeader { get; set; }
        public string SecondColumnHeader { get; set; }
        public int TotalMarkObtainable { get; set; }
        public int TotalPercentageObtainable { get; set; }
        public ICollection<AppraisalSectionDetail> AppraisalSectionDetails { get; set; }
        public AppraisalTemplateSection()
        {
            AppraisalSectionDetails = new Collection<AppraisalSectionDetail>();
        }
    }
}