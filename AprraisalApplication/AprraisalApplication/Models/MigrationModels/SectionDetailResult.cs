using AprraisalApplication.Models.ApiParameters;
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
        public int SectionDetailId { get; set; }
        public double Score { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public int Number { get; set; }
        public ICollection<ItemBreakdownResult> ItemBreakdownResults { get; set; }
        public SectionDetailResult()
        {
            ItemBreakdownResults = new Collection<ItemBreakdownResult>();
        }

        public SectionDetailResult(TaskPerformed item, int sectionResultId, int templateSectionId)
        {
            Title1 = item.Task;
            Title2 = item.TaskResult;
            Number = item.Number;
            SectionResultId = sectionResultId;
            SectionDetailId = templateSectionId;
            Score = 0;
        }

        public SectionDetailResult(InitiatedSectionDetail item, int sectionResultId)
        {
            SectionResultId = sectionResultId;
            SectionDetailId = item.Id;
            Score = 0;
            Title1 = item.Title1;
            Title2 = item.Title2;
            Number = item.Id;
        }
    }
}