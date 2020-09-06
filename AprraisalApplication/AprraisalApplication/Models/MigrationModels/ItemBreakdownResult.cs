using AprraisalApplication.Models.ApiParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class ItemBreakdownResult
    {
        public ItemBreakdownResult()
        {

        }

        public ItemBreakdownResult(InitiatedSectionDetailBreakdown breakdown, int sectionDetailResultId)
        {
            SectionDetailResultId = sectionDetailResultId;
            InitiatedSectionDetailBreakdownId = breakdown.Id;
            Score = 0;
            Title = breakdown.Title;
            ExpectedValueId = 1;
        }

        public ItemBreakdownResult(InitiatedSectionDetailBreakdown breakdown, Breakdown result, int sectionDetailResultId)
        {
            SectionDetailResultId = sectionDetailResultId;
            InitiatedSectionDetailBreakdownId = breakdown.Id;
            Score = 0;
            Title = breakdown.Title;
            ExpectedValueId = breakdown.ExpectedValueId;
            Value = result.BreakdownValue;
        }

        public int Id { get; set; }
        public int SectionDetailResultId { get; set; }
        public InitiatedSectionDetailBreakdown InitiatedSectionDetailBreakdown { get; set; }
        public int InitiatedSectionDetailBreakdownId { get; set; }
        public double Score { get; set; }
        public string Title { get; set; }
        public ExpectedValue ExpectedValue { get; set; }
        public byte ExpectedValueId { get; set; }
        public string Value { get; set; }
    }
}