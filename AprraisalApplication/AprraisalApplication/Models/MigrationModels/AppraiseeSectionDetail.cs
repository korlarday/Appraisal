using AprraisalApplication.Models.ApiParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraiseeSectionDetail
    {
        public AppraiseeSectionDetail()
        {

        }
        //public AppraiseeSectionDetail(TaskPerformed item, int appraiseeId, int templateSectionId)
        //{
        //    Title1 = item.Task;
        //    Title2 = item.TaskResult;
        //    Number = item.Number;
        //    AppraiseeId = appraiseeId;
        //    AppraisalTemplateSectionId = templateSectionId;
        //}

        public int Id { get; set; }
        public int AppraiseeId { get; set; }
        public int AppraisalTemplateSectionId { get; set; }
        public int Number { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public int Score { get; set; }
    }
}