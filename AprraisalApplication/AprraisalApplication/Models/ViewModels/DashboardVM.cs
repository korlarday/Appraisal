using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class DashboardVM
    {
        public int NumberOfEmployees { get; set; }
        public int CompletedAppraisals { get; set; }
        public int OngoingAppraisals { get; set; }
        public int EmployeesInDept { get; set; }
        public int MyOngoingAppraisals { get; set; }
        public int MySubordinates { get; set; }
        public int DeactivatedEmployees { get; set; }
    }
}