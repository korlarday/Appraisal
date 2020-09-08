using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ApiParameters
{
    public class SubmitAppraisalParams
    {
        public int AppraiseeId { get; set; }
        public List<SectionResult> SectionResults { get; set; }
    }
    public class SectionResult
    {
        public int SectionId { get; set; }
        public List<TaskPerformed> TaskPerformed { get; set; }
    }
    public class TaskPerformed
    {
        public int? SectionDetailResultId { get; set; }
        public int Number { get; set; }
        public string Task { get; set; }
        public string TaskResult { get; set; }
        public List<Breakdown> Breakdowns { get; set; }
    }
    public class Breakdown
    {
        public int BreakdownId { get; set; }
        public string BreakdownValue { get; set; }
        public string BreakdownText { get; set; }
    }
}