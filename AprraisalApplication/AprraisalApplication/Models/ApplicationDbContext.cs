using AprraisalApplication.Models.MigrationModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<State> States { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<EmployeeQualification> EmployeeQualifications { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}