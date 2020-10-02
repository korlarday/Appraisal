using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class SetupEmployeeAppraiserVM
    {
        public List<Employee> UserAppraisers { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Employee> Supervisors { get; set; }
    }
}