using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class ViewEmployeesRoles
    {
        public Employee Employee { get; set; }
        public List<string> Roles { get; set; }
    }
}