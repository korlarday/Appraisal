using AprraisalApplication.Models.ViewModels;
using EllipticCurve;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class Employee
    {
        public int Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        public DefaultUserAppraiser DefaultUserAppraiser { get; set; }
        public int DefaultUserAppraiserId { get; set; }
        public bool AccountDisabled { get; set; }

        [Required]
        [MaxLength(255)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(255)]
        public string Lastname { get; set; }

        [MaxLength(255)]
        public string Othername { get; set; }

        [MaxLength(20)]
        public string StaffCodeNo { get; set; }

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

        public byte[] PassportImage { get; set; }

        public byte[] SignatureImage { get; set; }


        public Department Department { get; set; }
        public int DepartmentId { get; set; }

        public Grade Grade { get; set; }
        [Required]
        public int GradeId { get; set; }


        public ICollection<EmployeeQualification> EmployeeQualifications { get; set; }
        public ICollection<CareerHistory> CareerHistories { get; set; }

        public Employee()
        {
            EmployeeQualifications = new Collection<EmployeeQualification>();
            CareerHistories = new Collection<CareerHistory>();
        }

        public Employee(CreateEmployeeProfileVM model, int defaultUserAppraiserId)
        {
            ApplicationUserId = HttpContext.Current.User.Identity.GetUserId();
            DefaultUserAppraiserId = defaultUserAppraiserId;
            Firstname = model.Firstname;
            Lastname = model.Lastname;
            Othername = model.Othername;
            StateId = model.StateId;
            GenderId = model.GenderId;
            BranchId = model.BranchId;
            GradeId = model.GradeId;
            JobTitleId = model.JobTitleId;
            DateOfEmployment = model.DateOfEmployment;
            DateOfLastPromotion = model.DateOfLastPromotion;
            TitleId = model.TitleId;
            DepartmentId = model.DepartmentId;
            StaffCodeNo = model.StaffCodeNo;
            if(model.UploadPassport != null)
            {
                PassportImage = new byte[model.UploadPassport.ContentLength];
                model.UploadPassport.InputStream.Read(PassportImage, 0, model.UploadPassport.ContentLength);
            }
            //if (model.UploadSignature != null)
            //{
            //    SignatureImage = new byte[model.UploadSignature.ContentLength];
            //    model.UploadSignature.InputStream.Read(SignatureImage, 0, model.UploadSignature.ContentLength);
            //}
        }

        internal void UpdateEmployee(CreateEmployeeProfileVM model)
        {
            Firstname = model.Firstname;
            Lastname = model.Lastname;
            Othername = model.Othername;
            StateId = model.StateId;
            GenderId = model.GenderId;
            BranchId = model.BranchId;
            GradeId = model.GradeId;
            JobTitleId = model.JobTitleId;
            DateOfEmployment = model.DateOfEmployment;
            DateOfLastPromotion = model.DateOfLastPromotion;
            TitleId = model.TitleId;
            DepartmentId = model.DepartmentId;
            StaffCodeNo = model.StaffCodeNo;
            if (model.UploadPassport != null)
            {
                PassportImage = new byte[model.UploadPassport.ContentLength];
                model.UploadPassport.InputStream.Read(PassportImage, 0, model.UploadPassport.ContentLength);
            }
            //if (model.UploadSignature != null)
            //{
            //    SignatureImage = new byte[model.UploadSignature.ContentLength];
            //    model.UploadSignature.InputStream.Read(SignatureImage, 0, model.UploadSignature.ContentLength);
            //}
        }
    }
}