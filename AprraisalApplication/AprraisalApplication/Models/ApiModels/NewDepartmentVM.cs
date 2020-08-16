using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ApiModels
{
    public class NewDepartmentVM
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string DepartmentName { get; set; }
        public string Feedback { get; set; }
    }
}