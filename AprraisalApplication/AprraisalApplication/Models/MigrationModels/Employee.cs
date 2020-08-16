using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class Employee
    {
        public int Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public int ApplicationUserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(255)]
        public string Lastname { get; set; }

        [MaxLength(255)]
        public string Othername { get; set; }

        public State State { get; set; }
        [Required]
        public int StateId { get; set; }

        public Gender Gender { get; set; }
        [Required]
        public byte GenderId { get; set; }

        public Branch Branch { get; set; }
        [Required]
        public int BranchId { get; set; }

        public JobTitle JobTitle { get; set; }
        public int JobTitleId { get; set; }

        [Required]
        public DateTime DateOfEmployment { get; set; }

        public DateTime? DateOfLastPromotion { get; set; }

        public Title Title { get; set; }
        public byte TitleId { get; set; }


        public ICollection<EmployeeQualification> EmployeeQualifications { get; set; }

        public Employee()
        {
            EmployeeQualifications = new Collection<EmployeeQualification>();
        }
    }
}