using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class SectionDetailBreakdown
    {
        public int Id { get; set; }
        public int AppraisalSectionDetailId { get; set; }
        public string Title { get; set; }
        public int MaxScore { get; set; }
        public ExpectedValue ExpectedValue { get; set; }
        public byte ExpectedValueId { get; set; }
    }
}