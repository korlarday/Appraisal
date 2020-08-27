using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class SectionDetailResult
    {
        public int Id { get; set; }
        public int SectionResultId { get; set; }
        public int AppraisalSectionDetailId { get; set; }
        public AppraisalSectionDetail AppraisalSectionDetail { get; set; }
        public double Score { get; set; }
        public ICollection<ItemBreakdownResult> ItemBreakdownResults { get; set; }
        public SectionDetailResult()
        {
            ItemBreakdownResults = new Collection<ItemBreakdownResult>();
        }
    }
}