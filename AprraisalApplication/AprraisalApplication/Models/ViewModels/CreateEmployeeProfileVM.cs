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
        public int Id { get; set; }

        public int ApplicationUserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(255)]
        public string Lastname { get; set; }

        [MaxLength(255)]
        public string Othername { get; set; }

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
        public DateTime DateOfEmployment { get; set; }

        public DateTime? DateOfLastPromotion { get; set; }

        [Required]
        [Display(Name = "Title")]
        public byte TitleId { get; set; }
        // for loading title dropdown
        public IEnumerable<SelectListItem> Titles { get; set; }

        [Display(Name = "Upload Passport")]
        public HttpPostedFileBase UploadPassport { get; set; }

        [Display(Name = "Upload Signature")]
        public HttpPostedFileBase UploadSignature { get; set; }
    }
}