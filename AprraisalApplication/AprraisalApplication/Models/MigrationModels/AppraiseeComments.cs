using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraiseeComments
    {
        public AppraiseeComments()
        {
            TrainingNeeds = "";
            AppraiseeComment = "";
            AppraiseeCommentDate = DateTime.Now;
            AppraiserComment = "";
            AppraiserCommentDate = DateTime.Now;
            HodComment = "";
            HodCommentDate = DateTime.Now;
            HrComment = "";
            HrCommentDate = DateTime.Now;
            MdComment = "";
            MdCommentDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string TrainingNeeds { get; set; }
        public string AppraiseeComment { get; set; }
        public DateTime AppraiseeCommentDate { get; set; }
        public string AppraiserComment { get; set; }
        public DateTime AppraiserCommentDate { get; set; }
        public string HodComment { get; set; }
        public DateTime HodCommentDate { get; set; }
        public int? HodEmployeeId { get; set; }
        public string HrComment { get; set; }
        public DateTime HrCommentDate { get; set; }
        public int? HrEmployeeId { get; set; }
        public string MdComment { get; set; }
        public DateTime MdCommentDate { get; set; }
        public int? MdEmployeeId { get; set; }
    }
}