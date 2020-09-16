using AprraisalApplication.Models.MigrationModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class EditEmployeeRolesVM
    {
        public List<IdentityRole> Roles { get; set; }
        public List<string> SelectedRoles { get; set; }
        public string EmployeeUserId { get; set; }
        public Employee Employee { get; set; }
        public List<string> NewRoles { get; set; }
    }
}