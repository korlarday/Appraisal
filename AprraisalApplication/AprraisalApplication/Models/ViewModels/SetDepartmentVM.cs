using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class SetDepartmentVM
    {
        public List<Department> Departments { get; set; }
        public Department CreateDepartment { get; set; }
    }
}