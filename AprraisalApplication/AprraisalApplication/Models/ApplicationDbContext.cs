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
        public DbSet<CareerHistory> CareerHistories { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<DefaultRating> DefaultRatings { get; set; }
        public DbSet<DefaultSummaryRating> DefaultSummaryRatings { get; set; }
        public DbSet<AppraisalTemplate> AppraisalTemplates { get; set; }
        public DbSet<AppraisalTemplateSection> AppraisalTemplateSections { get; set; }
        public DbSet<SectionType> SectionTypes { get; set; }
        public DbSet<AppraisalSectionDetail> AppraisalSectionDetails { get; set; }
        public DbSet<SectionDetailBreakdown> SectionDetailBreakdowns { get; set; }
        public DbSet<ExpectedValue> ExpectedValues { get; set; }
        public DbSet<TemplateRating> TemplateRatings { get; set; }
        public DbSet<TemplateSummaryRating> TemplateSummaryRatings { get; set; }

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