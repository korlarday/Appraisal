using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;

namespace AprraisalApplication.Models.ApiParameters
{
    public class AppraisalTemplateParams
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateDescription { get; set; }
        public List<AppraisalSectionParam> AppraisalSectionParams { get; set; }
    }
    public class AppraisalSectionParam
    {
        public int? SectionId { get; set; }
        public int SectionSetupId { get; set; }
        public string SectionTitle { get; set; }
        public string SectionInstructions { get; set; }
        public string SectionFirstColHeader { get; set; }
        public string SectionSecondColHeader { get; set; }
        public int SectionPercentageScore { get; set; }
        public int SectionTotalPoints { get; set; }
        public string SectionType { get; set; }
        public byte SectionTypeId { get; set; }
        public int DerivedSection { get; set; }
        public List<QualitativeDetail> QualitativeDetails { get; set; }
    }

    public class QualitativeDetail
    {
        public int? DetailId { get; set; }
        public int SetupDetailId { get; set; }
        public string Title { get; set; }
        public int Weight { get; set; }
        public List<ItemBreakDown> ItemBreakDowns { get; set; }
    }
    public class ItemBreakDown
    {
        public int? BreakdownId { get; set; }
        public string ItemText { get; set; }
        public string ValueType { get; set; }
    }
}