using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprraisalApplication.Models.ViewModels
{
    public class CreateEmployeeProfileVM
    {
        public CreateEmployeeProfileVM()
        {

        }
        public CreateEmployeeProfileVM(Employee employee)
        {
            ApplicationUserId = employee.ApplicationUserId;
            Firstname = employee.Firstname;
            Lastname = employee.Lastname;
            Othername = employee.Othername;
            StateId = employee.StateId;
            GenderId = employee.GenderId;
            BranchId = employee.BranchId;
            GradeId = employee.GradeId;
            JobTitleId = employee.JobTitleId;
            DateOfEmployment = employee.DateOfEmployment;
            DateOfLastPromotion = employee.DateOfLastPromotion;
            TitleId = employee.TitleId;
            DepartmentId = employee.DepartmentId;
            SelectedQualifications = employee.EmployeeQualifications.Select(x => x.QualificationId).ToList();
            PassportImage = employee.PassportImage;
            SignatureImage = employee.SignatureImage;
            StaffCodeNo = employee.StaffCodeNo;
        }

        public int Id { get; set; }

        public string ApplicationUserId { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Last Name")]
        public string Lastname { get; set; }

        [MaxLength(255)]
        [Display(Name = "Other Name")]
        public string Othername { get; set; }

        [MaxLength(15)]
        [Display(Name = "Staff Code No")]
        [Required]
        public string StaffCodeNo { get; set; }

        [Required]
        [Display(Name = "State Of Posting")]
        public int StateId { get; set; }
        // for loading state dropdown 
        public IEnumerable<SelectListItem> States { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public byte GenderId { get; set; }
        // for loading gender dropdown
        public IEnumerable<SelectListItem> Genders { get; set; }


        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        // for loading state dropdown
        public IEnumerable<SelectListItem> Branches { get; set; }

        [Required]
        [Display(Name = "Job Title")]
        public int JobTitleId { get; set; }
        // for loading jobtitle dropdown
        public IEnumerable<SelectListItem> JobTitles { get; set; }

        [Required]
        [Display(Name = "Date Of Employment")]
        public DateTime DateOfEmployment { get; set; }

        [Display(Name = "Date Of Last Promotion")]
        public DateTime? DateOfLastPromotion { get; set; }

        [Required]
        [Display(Name = "Title")]
        public byte TitleId { get; set; }
        // for loading title dropdown
        public IEnumerable<SelectListItem> Titles { get; set; }

        [Display(Name = "Department")]
        [Required]
        public int DepartmentId { get; set; }
        // for loading title dropdown
        public IEnumerable<SelectListItem> Departments { get; set; }

        [Display(Name = "Grade")]
        [Required]
        public int GradeId { get; set; }
        // for loading grade dropdown
        public IEnumerable<SelectListItem> Grades { get; set; }

        public List<Qualification> Qualifications { get; set; }
        public List<int> SelectedQualifications { get; set; }

        [Display(Name = "Upload Passport (Optional)")]
        public HttpPostedFileBase UploadPassport { get; set; }

        [Display(Name = "Upload Signature")]
        public HttpPostedFileBase UploadSignature { get; set; }

        public byte[] PassportImage { get; set; }

        public byte[] SignatureImage { get; set; }
    }
}