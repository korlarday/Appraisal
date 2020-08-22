using AprraisalApplication.Models.ApiParameters;
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
        public bool Optional { get; set; }
        public int SetupId { get; set; }
        public int? DerivedSectionSetupId { get; set; }
        public ICollection<AppraisalSectionDetail> AppraisalSectionDetails { get; set; }
        public AppraisalTemplateSection()
        {
            AppraisalSectionDetails = new Collection<AppraisalSectionDetail>();
        }

        public AppraisalTemplateSection(AppraisalSectionParam item, int appraisalTemplateId)
        {
            AppraisalTemplateId = appraisalTemplateId;
            SectionTypeId = item.SectionTypeId;
            SectionTitle = item.SectionTitle;
            SectionInstructions = item.SectionInstructions;
            FirstColumnHeader = item.SectionFirstColHeader;
            SecondColumnHeader = item.SectionSecondColHeader;
            TotalMarkObtainable = item.SectionTotalPoints;
            TotalPercentageObtainable = item.SectionPercentageScore;
            Optional = item.SectionType == "optional";
            SetupId = item.SectionSetupId;
            if (item.SectionType == "optional") {
                DerivedSectionSetupId = item.DerivedSection;
            } 
            else 
            { 
                DerivedSectionSetupId = null; 
            }
        }
    }
}