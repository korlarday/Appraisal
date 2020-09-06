using AprraisalApplication.Models.ApiParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class InitiatedTemplateSection
    {
        public int Id { get; set; }
        public int InitiatedAppraisalTemplateId { get; set; }
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
        public bool IsDeleted { get; set; }
        public string BreakdownValueBy { get; set; }
        public ICollection<InitiatedSectionDetail> InitiatedSectionDetails { get; set; }
        public InitiatedTemplateSection()
        {
            InitiatedSectionDetails = new Collection<InitiatedSectionDetail>();
        }

        public InitiatedTemplateSection(AppraisalTemplateSection section, int initiatedAppraisalTemplateId)
        {
            InitiatedAppraisalTemplateId = initiatedAppraisalTemplateId;
            SectionTypeId = section.SectionTypeId;
            SectionTitle = section.SectionTitle;
            SectionInstructions = section.SectionInstructions;
            FirstColumnHeader = section.FirstColumnHeader;
            SecondColumnHeader = section.SecondColumnHeader;
            TotalMarkObtainable = section.TotalMarkObtainable;
            TotalPercentageObtainable = section.TotalPercentageObtainable;
            Optional = section.Optional;
            SetupId = section.SetupId;
            BreakdownValueBy = section.BreakdownValueBy;
            DerivedSectionSetupId = section.DerivedSectionSetupId;
        }

        internal void Update(AppraisalSectionParam item)
        {
            SectionTitle = item.SectionTitle;
            SectionInstructions = item.SectionInstructions;
            FirstColumnHeader = item.SectionFirstColHeader;
            SecondColumnHeader = item.SectionSecondColHeader;
            TotalMarkObtainable = item.SectionTotalPoints;
            TotalPercentageObtainable = item.SectionPercentageScore;
            Optional = item.SectionType == "optional";
            SetupId = item.SectionSetupId;
            BreakdownValueBy = item.BreakdownValueBy;
            if (item.SectionType == "optional")
            {
                DerivedSectionSetupId = item.DerivedSection;
            }
            else
            {
                DerivedSectionSetupId = null;
            }
        }
    }
}