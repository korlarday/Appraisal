using AprraisalApplication.Models.ApiParameters;
using AprraisalApplication.Models.Attributes;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class NewAppraisal
    {
        public int Id { get; set; }
        public ApplicationUser InitiatedBy { get; set; }
        public string InitiatedById { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime DateInitiated { get; set; }
        public AppraisalType AppraisalType { get; set; }
        [Display(Name = "Appraisal Title")]
        public string AppraisalTitle { get; set; }
        [Display(Name = "Appraisal Type")]
        public byte AppraisalTypeId { get; set; }
        [Display(Name = "Period Start Date")]
        public DateTime AppraisalPeriodStartDate { get; set; }
        [Display(Name = "Period End Date")]
        public DateTime AppraisalPeriodEndDate { get; set; }
        public string Slug { get; set; }
        public ICollection<Appraisee> Appraisees { get; set; }
        public ICollection<AppraisalDepartment> AppraisalDepartments { get; set; }
        public ICollection<AppraisalLocation> AppraisalLocations { get; set; }
        public ICollection<AppraisalStaff> AppraisalStaffs { get; set; }
        public NewAppraisal()
        {
            AppraisalDepartments = new Collection<AppraisalDepartment>();
            AppraisalLocations = new Collection<AppraisalLocation>();
            AppraisalStaffs = new Collection<AppraisalStaff>();
            Appraisees = new Collection<Appraisee>();
        }

        public NewAppraisal(NewAppraisalParams model)
        {
            InitiatedById = HttpContext.Current.User.Identity.GetUserId();
            AppraisalTypeId = model.AppraisalTypeId;
            AppraisalTitle = model.AppraisalTitle;
            Slug = model.AppraisalTitle.Replace(" ", "-").Replace(",", "-").Replace(".", "-").ToLower();
            DateInitiated = DateTime.Now;

            string startDate = model.StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            AppraisalPeriodStartDate = Convert.ToDateTime(startDate);

            string endDate = model.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            AppraisalPeriodEndDate = Convert.ToDateTime(endDate);

            if(model.DueDate != null)
            {
                string dueDateModal = model.DueDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                DueDate = Convert.ToDateTime(dueDateModal);
            }
        }

        internal void Update(NewAppraisalParams model)
        {
            AppraisalTypeId = model.AppraisalTypeId;
            AppraisalTitle = model.AppraisalTitle;
            Slug = model.AppraisalTitle.Replace(" ", "-").Replace(",", "-").Replace(".", "-").ToLower();

            string startDate = model.StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            AppraisalPeriodStartDate = Convert.ToDateTime(startDate);

            string endDate = model.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            AppraisalPeriodEndDate = Convert.ToDateTime(endDate);

            if (model.DueDate != null)
            {
                string dueDateModal = model.DueDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                DueDate = Convert.ToDateTime(dueDateModal);
            }
            else
            {
                model.DueDate = null;
            }
        }
    }
}