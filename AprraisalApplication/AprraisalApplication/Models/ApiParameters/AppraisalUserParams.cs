using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ApiParameters
{
    public class AppraisalUserParams
    {
        public List<AppraisalUserItem> Items { get; set; }
    }
    public class AppraisalUserItem
    {
        public string ApplicationUserId { get; set; }
        public int? TemplateId { get; set; }
    }
}