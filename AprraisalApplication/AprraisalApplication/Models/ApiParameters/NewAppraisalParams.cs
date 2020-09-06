using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ApiParameters
{
    public class NewAppraisalParams
    {
        public int AppraisalId { get; set; }
        public string AppraisalTitle { get; set; }
        public byte AppraisalTypeId { get; set; }
        public List<string> States { get; set; }
        public List<string> Departments { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<int> SelectedEmployees { get; set; }
        public List<int> NewSelectedEmployee { get; set; }
    }
}