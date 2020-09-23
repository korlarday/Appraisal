using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ApiParameters
{
    public class CareerHistoryParams
    {
        public string UserId { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int DepartmentId { get; set; }
        public int GradeId { get; set; }
        public string Training { get; set; }
    }
}