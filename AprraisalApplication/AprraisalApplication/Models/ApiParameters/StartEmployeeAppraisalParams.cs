using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ApiParameters
{
    public class StartEmployeeAppraisalParams
    {
        public int EmployeeId { get; set; }
        public int NewAppraisalId { get; set; }
    }
}