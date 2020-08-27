using AprraisalApplication.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public AppraisalType AppraisalType { get; set; }
        public byte AppraisalTypeId { get; set; }
        public DateTime AppraisalPeriodStartDate { get; set; }
        public DateTime AppraisalPeriodEndDate { get; set; }
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
    }
}