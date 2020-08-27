using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ApiParameters
{
    public class AppraisalDepartmentParams
    {
        public List<AppraisalDepartmentItem> Items { get; set; }
    }
    public class AppraisalDepartmentItem
    {
        public int DepartmentId { get; set; }
        public int? TemplateId { get; set; }
    }
}