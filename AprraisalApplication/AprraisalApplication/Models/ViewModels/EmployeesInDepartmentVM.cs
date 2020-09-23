using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class EmployeesInDepartmentVM
    {
        public Department Department { get; set; }
        public List<Employee> Employees { get; set; }
    }
}