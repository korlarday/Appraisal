using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ApiParameters
{
    public class SectionScoresParams
    {
        public int AppraiseeId { get; set; }
        public List<SectionScoresResult> SectionScoresResults { get; set; }
        public string RejectionReason { get; set; }
        public string RejectionType { get; set; }
        public string AppraiserComment { get; set; }
        public BdsTrackerParam BdsTracker { get; set; }
    }
    public class SectionScoresResult
    {
        public List<SectionDetailsScore> SectionDetailsScore { get; set; }
        public double SectionPercentageScore { get; set; }
        public double SectionTotalScore { get; set; }
        public int SectionResultId { get; set; }
        public int SectionTypeId { get; set; }
    }
    public class SectionDetailsScore
    {
        public int SectionResultDetailId { get; set; }
        public int Score { get; set; }
        public string ResultAchieved { get; set; }
        public List<Breakdown> Breakdowns { get; set; }
    }
}