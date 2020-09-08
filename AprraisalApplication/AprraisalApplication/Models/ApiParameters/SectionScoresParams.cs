﻿using System;
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
    }
    public class SectionScoresResult
    {
        public List<SectionDetailsScore> SectionDetailsScore { get; set; }
        public double SectionPercentageScore { get; set; }
        public double SectionTotalScore { get; set; }
        public int SectionResultId { get; set; }
    }
    public class SectionDetailsScore
    {
        public int SectionResultDetailId { get; set; }
        public int Score { get; set; }
    }
}